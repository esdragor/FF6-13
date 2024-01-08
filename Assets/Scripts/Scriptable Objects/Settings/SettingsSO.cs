using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SettingsSO", menuName = "Scriptable Objects/Settings/SettingsSO", order = 0)]
public class SettingsSO : ScriptableObject
{
    [Serializable]
    public struct IllustratedAction
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public InputActionReference Action { get; private set; }
        
        public IllustratedAction(Sprite sprite, InputActionReference action)
        {
            Sprite = sprite;
            Action = action;
        }
    }
    
    [SerializeField] private List<IllustratedAction> illustratedActions;
    public IReadOnlyList<IllustratedAction> IllustratedActions => illustratedActions;
    
    public Sprite GetSpriteForAction(InputActionReference action)
    {
        return illustratedActions.FirstOrDefault(illustratedAction => illustratedAction.Action == action).Sprite;
    }


#if UNITY_EDITOR
    [SerializeField] private InputActionAsset inputActionAsset;
    [ContextMenu("fill")]
    private void Fill()
    {
        var subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(inputActionAsset));
        List<InputActionReference> inputActionReferences = new List<InputActionReference>();
        foreach (var obj in subAssets) {
            // there are 2 InputActionReference returned for each InputAction in the asset, need to filter to not add the hidden one generated for backward compatibility
            if (obj is InputActionReference inputActionReference && (inputActionReference.hideFlags & HideFlags.HideInHierarchy) == 0) {
                inputActionReferences.Add(inputActionReference);
            }
        }
        
        illustratedActions.Clear();
        foreach (var action in inputActionReferences)
        {
            var illustratedAction = new IllustratedAction(null, action);
            illustratedActions.Add(illustratedAction);
        }
    }
#endif
}
