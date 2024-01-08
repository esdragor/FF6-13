using System.Collections.Generic;
using UnityEngine;

public class UIIGMenu : MonoBehaviour
{
    [SerializeField] private SettingsSO settingsSo;
    [SerializeField] private UISelectionPanel menuSelectionPanel;
    [Space]
    [SerializeField] private UITextPair timeTextPair;
    [SerializeField] private UITextPair stepsTextPair;
    [SerializeField] private UITextPair gilTextPair;
    [SerializeField] private UITextPair locationTextPair;
    [Space]
    [SerializeField] private UIRefs uiControlCloseMenu;
    [SerializeField] private UIRefs uiControlFormation;
    [SerializeField] private UIRefs uiControlConfirm;
    [SerializeField] private UIRefs uiControlBack;
    [Space]
    [SerializeField] private UICharacterInfoBasic[] uiCharacterInfoBasics;
    
    
    private void Start()
    {
        var selectionOptions = new List<UISelectionPanel.SelectionOption>
        {
            new ("Items", null),
            new ("Abilities", null),
            new ("Equip",null),
            new ("Status",null),
            new ("Formation",null),
            new ("Configuration",null),
            new ("Quick Save",null),
            new ("Save",null),
            new ("Back",null)
        };

        menuSelectionPanel.SetSelectionOptions(selectionOptions);
        menuSelectionPanel.UpdateSelectionOptions();
        
        menuSelectionPanel.UIButtons[6].SetInteractable(false);
        menuSelectionPanel.UIButtons[7].SetInteractable(false);
        
        menuSelectionPanel.ApplyNavigation();
        
        timeTextPair.MainText.text = "Time";
        stepsTextPair.MainText.text = "Steps";
        gilTextPair.MainText.text = "Gil";
        locationTextPair.MainText.text = "Current Location :";
        
        uiControlCloseMenu.SetSprite(settingsSo.GetSpriteForAction(uiControlCloseMenu.InputActionReference));
        uiControlFormation.SetSprite(settingsSo.GetSpriteForAction(uiControlFormation.InputActionReference));
        uiControlConfirm.SetSprite(settingsSo.GetSpriteForAction(uiControlConfirm.InputActionReference));
        uiControlBack.SetSprite(settingsSo.GetSpriteForAction(uiControlBack.InputActionReference));

        foreach (var characterInfoBasic in uiCharacterInfoBasics)
        {
            characterInfoBasic.Change(null);
        }
    }
}
