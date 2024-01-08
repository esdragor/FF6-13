using System.Linq;
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

    private PlayerCharacterData associatedUnitData;
    
    public void Change(PlayerCharacterData data)
    { 
        UnbindEvents();
        associatedUnitData = data;
        
        BindEvents();
        
        UpdateData();
    }

    private void BindEvents()
    {
        if(associatedUnitData == null) return;
        
        //associatedUnitData.On += UpdateData;
    }
    
    private void UnbindEvents()
    {
        if(associatedUnitData == null) return;
    }

    private void UpdateData()
    {
        if (associatedUnitData == null)
        {
            Button.interactable = false;
            
            portrait.gameObject.SetActive(false);
            titleText.text = "";
            nameText.text = "";
            
            hpPair.MainText.text = "";
            hpPair.SubText.text = "";
            
            mpPair.MainText.text = "";
            mpPair.SubText.text = "";
            
            lvPair.MainText.text = "";
            lvPair.SubText.text = "";
            
            nextLvPair.MainText.text = "";
            nextLvPair.SubText.text = "";
            
            return;
        }
        
        Button.interactable = true;
        portrait.gameObject.SetActive(true);
        
        var so = associatedUnitData.PlayerCharactersSo;
        
        portrait.sprite = so.Portrait;
        titleText.text = so.Title;
        nameText.text = so.Name;
        
        hpPair.MainText.text = "HP";
        hpPair.SubText.text = $"{associatedUnitData.CurrentHp}/{associatedUnitData.MaxHp}";

        var useMp = so.GrowthTable.GrowthRates.Select(rate => rate.Mp).Sum() > 0;
        
        mpPair.MainText.text =  useMp ? "MP" : "";
        mpPair.SubText.text = useMp ? $"{associatedUnitData.CurrentMp}/{associatedUnitData.MaxMp}" : "";
        
        lvPair.MainText.text = "LV";
        lvPair.SubText.text = $"{associatedUnitData.Level}";
        
        nextLvPair.MainText.text = "Next Level in";
        nextLvPair.SubText.text = $"{associatedUnitData.GetXpToNextLevel()}";
    }
}
