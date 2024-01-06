using System;
using System.Collections;
using System.Collections.Generic;
using Exploration;
using Scriptable_Objects.Interaction;
using Scriptable_Objects.Interaction.IneractionActions;
using UnityEngine;

namespace Narative
{
    public class StoryInteractor : MonoBehaviour, Interactor
    {
        public event Action<StoryInteractor, string, bool> DisplayDialogue;
        public event Action HideDialogue;

        [SerializeField] private InteractionSO interactionSo;
        [SerializeField] private bool destroyAfterInteraction = true;
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private MovementSwitcher monsterPrefab;

        private Dictionary<string, MovementSwitcher> entities = new();
        private List<InteractionAction> interactionActionsRemaining = new();
        [SerializeField] private bool waitForInteractions = false;
        private bool inBattle = false;
        private string waitingFor = "";

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Trigger go with {other}");
            Interact(other.GetComponent<Entity>());
        }

        public void Interact(Entity entity)
        {
            if (entity == null || !boxCollider2D.enabled) return;

            var player = (PlayerEntity)entity;

            if (player == null) return;

            boxCollider2D.enabled = false;
            //Remove controle from player
            StartPlayerInteraction(player);
        }

        private void StartPlayerInteraction(PlayerEntity player)
        {
            Debug.Log("Interact");
            entities.Add("player", player.gameObject.GetComponent<MovementSwitcher>());
            entities["player"].PnjMovement.ClearDirections();

            interactionActionsRemaining = new List<InteractionAction>(interactionSo.InteractionActions);

            StartCoroutine(DoInteractions(interactionActionsRemaining));
        }

        private IEnumerator DoInteractions(List<InteractionAction> interactionActions)
        {
            // First, we move to the interactors' middle
            MovePlayerToInteractor();
            if (waitForInteractions) yield return new WaitUntil(() => !waitForInteractions);

            foreach (var interactionAction in interactionActions)
            {
                if (interactionAction.timeBefore > 0) yield return new WaitForSeconds(interactionAction.timeBefore);
                ResolveInteraction(interactionAction.interaction);

                if (waitForInteractions) yield return new WaitUntil(() => !waitForInteractions);
                if (interactionAction.timeAfter > 0) yield return new WaitForSeconds(interactionAction.timeAfter);

                HideDialogue?.Invoke();
            }

            FinishInteraction();
        }

        private void MovePlayerToInteractor()
        {
            var cellSize = 0.5f; //TODO: not fix

            var player = entities["player"];
            var playerPos = player.transform.position;
            var interactorPos = transform.position;

            List<Direction> dirs = new List<Direction>();
            Vector3 fictivePos = playerPos;

            while (fictivePos.x != interactorPos.x && fictivePos.y != interactorPos.y)
            {
                if (fictivePos.x < interactorPos.x)
                {
                    dirs.Add(Direction.Right);
                    fictivePos.x += cellSize;
                }
                else if (fictivePos.x > interactorPos.x)
                {
                    dirs.Add(Direction.Left);
                    fictivePos.x -= cellSize;
                }

                if (fictivePos.y < interactorPos.y)
                {
                    dirs.Add(Direction.Up);
                    fictivePos.y += cellSize;
                }
                else if (fictivePos.y > interactorPos.y)
                {
                    dirs.Add(Direction.Down);
                    fictivePos.y -= cellSize;
                }
                
                // Normalize pos to mult of cellSize
                fictivePos.x = (float) Math.Round(fictivePos.x / cellSize) * cellSize;
                fictivePos.y = (float) Math.Round(fictivePos.y / cellSize) * cellSize;
            }


            var playerMovement = player.PnjMovement;
            player.SwitchToPNJMode();
            playerMovement.ClearDirections();
            if (dirs.Count == 0) return;

            waitingFor = "player";
            playerMovement.OnMovementEnded += ResumeAfterMove;
            waitForInteractions = true;

            foreach (var direction in dirs)
            {
                playerMovement.AddDirectionToMove(direction);
            }
        }

        private void ResolveInteraction(InteractionActionSO interactionAction)
        {
            Debug.Log($"Resolve {interactionAction.name}");
            switch (interactionAction)
            {
                case IneractionSpawnSO spawnSO:
                    Spawn(spawnSO);
                    break;
                case InteractionMoveSO moveSO:
                    Move(moveSO);
                    break;
                case InteractionDestroySO destroySo:
                    Destroy(destroySo);
                    break;
                case InteractionSaySO saySo:
                    Say(saySo);
                    break;
                case InteractionSetCinematicSO cinematicSo:
                    SetCinematic(cinematicSo);
                    break;
                case InteractionStartBattleSO battleSo:
                    StartBattle(battleSo);
                    break;
                case InteractionTurnSO turnSo:
                    Turn(turnSo);
                    break;
            }
        }

        private void SetCinematic(InteractionSetCinematicSO cinematicSo)
        {
            ((PlayerEntity)entities["player"].Entity).Cinematic(cinematicSo.SetCinematic, !inBattle);
        }

        private void StartBattle(InteractionStartBattleSO battleSo)
        {
            waitForInteractions = true;
            inBattle = true;
            GameManager.Instance.LaunchBattleScriptedEncounter(battleSo.Encounter);

            BattleManager.OnBattleEnded += ContinueAfterBattle;
        }

        private void Spawn(IneractionSpawnSO spawnSO)
        {
            if (spawnSO.StoreUnitAsId is "player" or "" || entities.ContainsKey(spawnSO.StoreUnitAsId)) return;

            MovementSwitcher newMonster = Instantiate(monsterPrefab, spawnSO.SpawnPos, Quaternion.identity);

            newMonster.Entity.Init(spawnSO.UnitToSpawn, true);
            entities.Add(spawnSO.StoreUnitAsId, newMonster);
        }

        private void Move(InteractionMoveSO moveSO)
        {
            if (moveSO.UnitId is "" || !entities.ContainsKey(moveSO.UnitId)) return;
            if (moveSO.WaitTillFinish)
            {
                waitForInteractions = true;
                waitingFor = moveSO.UnitId;
                entities[moveSO.UnitId].PnjMovement.OnMovementEnded += ResumeAfterMove;
            }

            entities[moveSO.UnitId].SwitchToPNJMode();
            entities[moveSO.UnitId].PnjMovement.ClearDirections();
            foreach (var direction in moveSO.Directions)
            {
                entities[moveSO.UnitId].PnjMovement.AddDirectionToMove(direction);
            }
        }

        private void ResumeAfterMove()
        {
            entities[waitingFor].PnjMovement.OnMovementEnded -= ResumeAfterMove;
            waitForInteractions = false;
        }

        private void ContinueAfterBattle()
        {
            waitForInteractions = false;
            inBattle = false;
            BattleManager.OnBattleEnded -= ContinueAfterBattle;
        }

        private void Destroy(InteractionDestroySO destroySo)
        {
            if (destroySo.DestroyId is "player" or "" || !entities.ContainsKey(destroySo.DestroyId)) return;

            Destroy(entities[destroySo.DestroyId].gameObject);
            entities.Remove(destroySo.DestroyId);
        }

        private void Say(InteractionSaySO saySo)
        {
            HideDialogue?.Invoke();
            DisplayDialogue?.Invoke(this, saySo.TextId, saySo.Top);
            waitForInteractions = true;
        }

        private void Turn(InteractionTurnSO turnSo)
        {
            if (turnSo.UnitId is "" || !entities.ContainsKey(turnSo.UnitId)) return;

            entities[turnSo.UnitId].PnjMovement.Turn(turnSo.Direction);
        }


        public void ResumeAfterDialogue()
        {
            waitForInteractions = false;
        }


        private void FinishInteraction()
        {
            entities["player"].SwitchToPlayerMode();
            if (destroyAfterInteraction) Destroy(gameObject);
        }
    }

    public interface Interactor
    {
        void Interact(Entity entity);
    }
}