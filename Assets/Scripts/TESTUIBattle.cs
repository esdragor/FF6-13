using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTUIBattle : MonoBehaviour
{
    [SerializeField] private Image AttackImage;
    [SerializeField] private Image DefendImage;
    [SerializeField] private Image ItemImage;
    [SerializeField] private Image EscapeImage;
    
    private List<Image> images = new();
    private int actionIndex = 0;
    
    // Start is called before the first frame update

    private void Awake()
    {
        BattleManager.OnSelectionChanged += SelectionAction;
    }

    void OnEnable()
    {
        images.Clear(); 
        images.Add(AttackImage);
        images.Add(DefendImage);
        images.Add(ItemImage);
        images.Add(EscapeImage);
        actionIndex = 0;
        images[actionIndex].color = Color.red;
    }

    private void SelectionAction(int index)
    {
        images[actionIndex].color = Color.white;
        actionIndex = index;
        images[actionIndex].color = Color.red;
    }
    
}