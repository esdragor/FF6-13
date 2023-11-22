using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Interaction;
using Scriptable_Objects.Interaction.IneractionActions;
using Unity.VisualScripting;
using UnityEngine;

namespace Narative
{
    public class StoryInteractor : MonoBehaviour, Interactor
    {
        [SerializeField] private InteractionSO interactionSo;
        [SerializeField] private bool destroyAfterInteraction = true;
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private Entity monsterPrefab;

        private Dictionary<string, Entity> entities = new();
        private List<InteractionAction> interactionActionsRemaining = new();
        private bool waitForInteractions = false;
        private bool inBattle = false;

        private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log($"Trigger go with {other}");
            Interact(other.GetComponent<Entity>());
        }

        private void OnCollision2DEnter(Collision2D other)
        {
            Debug.Log($"Collision go with {other}");
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
            entities.Add("player", player);
            
            interactionActionsRemaining = new List<InteractionAction>(interactionSo.InteractionActions);
            
            StartCoroutine( DoInteractions(interactionActionsRemaining) );
        }

        private IEnumerator DoInteractions(List<InteractionAction> interactionActions)
        {
            foreach (var interactionAction in interactionActions)
            {
                if (interactionAction.timeBefore > 0) yield return new WaitForSeconds(interactionAction.timeBefore);
                ResolveInteraction(interactionAction.interaction);
                if (interactionAction.timeAfter > 0) yield return new WaitForSeconds(interactionAction.timeAfter);

                if (waitForInteractions) yield return new WaitUntil(() => !waitForInteractions);
            }
            FinishInteraction();
        }

        private void ResolveInteraction(InteractionActionSO interactionAction)
        {
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
            }
        }

        private void SetCinematic(InteractionSetCinematicSO cinematicSo)
        {
            //entities["player"].SetCinematic(cinematicSo.SetCinematic, !inBattle);
        }

        private void StartBattle(InteractionStartBattleSO battleSo)
        {
            //Register to battle manager's onBattleEnded
        }

        private void Spawn(IneractionSpawnSO spawnSO)
        {
            if (spawnSO.StoreUnitAsId is "player" or "" || entities.ContainsKey(spawnSO.StoreUnitAsId)) return;
            
            Entity newMonster = Instantiate(monsterPrefab, spawnSO.SpawnPos, Quaternion.identity);
            
            newMonster.Init(spawnSO.UnitToSpawn, true);
            entities.Add(spawnSO.StoreUnitAsId, newMonster);
        }
        
        private void Move(InteractionMoveSO moveSO)
        {
            if (moveSO.UnitId is "" || !entities.ContainsKey(moveSO.UnitId)) return;
            if (moveSO.WaitTillFinish)
            {
                waitForInteractions = true;
                //entities["player"]. += resumeInteraction;
            }
        }
        
        private void resumeInteraction()
        {
            waitForInteractions = false;
            //entities["player"]. -= resumeInteraction;
        }
        
        private void Destroy(InteractionDestroySO destroySo)
        {
            if (destroySo.DestroyId is "player" or "" || !entities.ContainsKey(destroySo.DestroyId)) return;

            Destroy(entities[destroySo.DestroyId].gameObject);
            entities.Remove(destroySo.DestroyId);
        }
        
        private void Say(InteractionSaySO saySo)
        {
            //TODO:
        }
        
        
        

        private void FinishInteraction()
        {
            if (destroyAfterInteraction) Destroy(gameObject);
        }
    }
    
    public interface Interactor
    {
        void Interact(Entity entity);
    }
}