using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects.Unit;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfoBasic : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [field: SerializeField] public Button Button { get; private set; }
    
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [SerializeField] private UITextPair hpPair;
    [SerializeField] private UITextPair mpPair;
    [SerializeField] private UITextPair lvPair;
    [SerializeField] private UITextPair nextLvPair;

    private UnitData associatedUnitData;
    
    public void Change(UnitData data)
    { 
        UnbindEvents();
        associatedUnitData = data;
        
        BindEvents();
        
        UpdateData();
    }

    private void BindEvents()
    {
        if(associatedUnitData == null) return;
    }
    
    private void UnbindEvents()
    {
        if(associatedUnitData == null) return;
    }

    private void UpdateData()
    {
        if(associatedUnitData == null) return;
        
        var so = associatedUnitData.UnitSo;
        
        portrait.sprite = so.Portrait;
        titleText.text = so.Title;
        nameText.text = so.Name;
        
        hpPair.MainText.text = "HP";
        hpPair.SubText.text = $"{associatedUnitData.CurrentHp}/{associatedUnitData.MaxHp}";

        var useMp = false;
        if(so is PlayerCharactersSO playerCharactersSo)
        {
            useMp = playerCharactersSo.GrowthTable.GrowthRates.Select(rate => rate.Mp).Sum() > 0;
        }
        
        mpPair.MainText.text =  useMp ? "MP" : "";
        mpPair.SubText.text = useMp ? $"{associatedUnitData.CurrentMp}/{associatedUnitData.MaxMp}" : "";
        
        lvPair.MainText.text = "LV";
        lvPair.SubText.text = $"{associatedUnitData.Level}";
        
        nextLvPair.MainText.text = "Next Level in";
        //nextLvPair.SubText.text = $"{associatedUnitData.ExperienceToNextLevel - associatedUnitData.Experience}";
    }
}
