using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIDisplayer : MonoBehaviour
{
   [SerializeField] private UICursor cursor;
   [SerializeField] private UIRefs controlsControls;
   [SerializeField] private UIRefs nextControls;
   
   [Header("Main Panels")]
   [SerializeField] private GameObject teamPanelObj;
   [SerializeField] private GameObject enemyPanelObj;
   
   [Header("Selection Panels")]
   [SerializeField] private GameObject selectionPanelObj;
   [SerializeField] private GameObject fullSelectionPanelObj;
   
   [Header("Selection Info")]
   [SerializeField] private UIPanel selectionInfoPanel;
   [SerializeField] private GameObject selectionInfoPanelDisabledObj;
   
   [Header("Ability Name")]
   [SerializeField] private UIPanel abilityNamePanel;
   
   [Header("Battle End Panels")]
   [SerializeField] private GameObject endBattleTopPanel; //probably rewards
   [SerializeField] private GameObject endBattleXPPanel;
   
   public void ShowEndPanel()
   {
      endBattleTopPanel.SetActive(true);
      endBattleXPPanel.SetActive(true);
   }

   public void HideEndPanel()
   {
      endBattleTopPanel.SetActive(false);
      endBattleXPPanel.SetActive(false);
   }
}
