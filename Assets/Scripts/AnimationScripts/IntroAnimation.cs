using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IntroAnimation : MonoBehaviour
{
    [Serializable]
    private struct CloudLights
    {
        [field: SerializeField] public List<Light2D> Lights { get; private set; }
    }
    
    [SerializeField] private Animator animator;
    [SerializeField] private List<CloudLights> cloudLights;
}
