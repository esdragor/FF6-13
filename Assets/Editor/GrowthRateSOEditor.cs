using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(GrowthRatesSO))]
public class GrowthRateSOEditor : Editor
{
    SerializedProperty growthRates;
    
    ReorderableList list;

    private void OnEnable()
    {
        growthRates = serializedObject.FindProperty("growthRates");
        
        list = new ReorderableList(serializedObject, growthRates, true, true, true, true)
        {
            drawHeaderCallback = DrawHeader,
            drawElementCallback = DrawListItems,
            //onAddCallback = OnAdd,
            //onRemoveCallback = OnRemove
        };
    }
    
    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        var rectX = rect.x;
        
        var element = list.serializedProperty.GetArrayElementAtIndex(index); // The element in the list
        
        EditorGUI.LabelField(new Rect(rectX, rect.y, 100, EditorGUIUtility.singleLineHeight), $" [{index}]: {index+2}");
        
        
        DrawLabel("Hp","Health",50,45,100,35);
        
        DrawLabel("Mp","Mp",40,55,100,35);
        
        DrawLabel("Xp","Xp",40,30,100,35);

        return;

        void DrawLabel(string label,string field,float xLabel,float xField,float wLabel, float wField)
        {
            rectX += xLabel;
            EditorGUI.LabelField(new Rect(rectX, rect.y, wLabel, EditorGUIUtility.singleLineHeight), label);
            
            rectX += xField;
            EditorGUI.PropertyField(
                new Rect(rectX, rect.y, wField, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative($"<{field}>k__BackingField"),
                GUIContent.none
            );
        }
    }

//Draws the header
    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Grow Rates");
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        serializedObject.Update();
        
        list.DoLayoutList();
        
        serializedObject.ApplyModifiedProperties();
    }
}
