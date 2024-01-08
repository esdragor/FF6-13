using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextPair : MonoBehaviour
{
    [field:SerializeField] public TextMeshProUGUI MainText { get; private set; }
    [field:SerializeField] public TextMeshProUGUI SubText { get; private set; }
}
