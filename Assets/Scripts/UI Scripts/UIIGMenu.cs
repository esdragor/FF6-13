using System;
using System.Collections.Generic;
using UnityEngine;

public class UIIGMenu : MonoBehaviour
{
    private event Action ItemSelected;
    
    public static bool IsOpen = false;
    
    [SerializeField] private SettingsSO settingsSo;
    [SerializeField] private UISelectionPanel menuSelectionPanel;
    [Space]
    [SerializeField] private UITextPair timeTextPair;
    [SerializeField] private UITextPair stepsTextPair;
    [SerializeField] private UITextPair gilTextPair;
    [SerializeField] private UITextPair locationTextPair;
    [SerializeField] private UIPanel EquipPanel;
    [SerializeField] private UIPanel ControlReminderPanel;
    [Space]
    [SerializeField] private UIRefs uiControlCloseMenu;
    [SerializeField] private UIRefs uiControlFormation;
    [SerializeField] private UIRefs uiControlConfirm;
    [SerializeField] private UIRefs uiControlBack;
    [Space]
    [SerializeField] private UICharacterInfoBasic[] uiCharacterInfoBasics;
    [Space]
    [SerializeField] private UIPanel ItemPanel;
    [SerializeField] private UIPanel mainPanel;

    private void Awake()
    {
        ItemSelected = () => OpenItemPanel();
    }

    private void Start()
    {
        IsOpen = true;
        var selectionOptions = new List<UISelectionPanel.SelectionOption>
        {
            new ("Items", ()=>ItemSelected?.Invoke()),
            new ("Abilities", ()=>Debug.Log("Abilities")),
            new ("Equip",()=>Debug.Log("Equip")),
            new ("Status",()=>Debug.Log("Status")),
            new ("Formation",()=>Debug.Log("Formation")),
            new ("Configuration",()=>Debug.Log("Configuration")),
            new ("Quick Save",()=>Debug.Log("Quick Save")),
            new ("Save",()=>Debug.Log("Save")),
            new ("Back",()=>Debug.Log("Back"))
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
        
        menuSelectionPanel.UIButtons[0].Button.Select();
        
        InputManager.OnOpenCloseMenu += OnOpenCloseMenu;
        OnOpenCloseMenu();
    }
    
    private void OnOpenCloseMenu()
    {
        if (IsOpen)
        {
            menuSelectionPanel.gameObject.SetActive(false);
            uiControlCloseMenu.gameObject.SetActive(false);
            uiControlFormation.gameObject.SetActive(false);
            uiControlConfirm.gameObject.SetActive(false);
            uiControlBack.gameObject.SetActive(false);
            gilTextPair.gameObject.transform.parent.gameObject.SetActive(false);
            EquipPanel.gameObject.SetActive(false);
            locationTextPair.gameObject.SetActive(false);
            ControlReminderPanel.gameObject.SetActive(false);
            mainPanel.gameObject.SetActive(false);
            ItemPanel.gameObject.SetActive(false);
            IsOpen = false;
        }
        else
        {
            menuSelectionPanel.gameObject.SetActive(true);
            uiControlCloseMenu.gameObject.SetActive(true);
            uiControlFormation.gameObject.SetActive(true);
            uiControlConfirm.gameObject.SetActive(true);
            uiControlBack.gameObject.SetActive(true);
            gilTextPair.gameObject.transform.parent.gameObject.SetActive(true);
            locationTextPair.gameObject.SetActive(true);
            ControlReminderPanel.gameObject.SetActive(true);
            mainPanel.gameObject.SetActive(true);
            IsOpen = true;
        }
    }

    private void OpenItemPanel()
    {
        menuSelectionPanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(false);
        ItemPanel.gameObject.SetActive(true);
    }
}
