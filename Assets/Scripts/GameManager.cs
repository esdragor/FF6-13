using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private IEnumerator LaunchBattle()
    {
        yield return new WaitForSeconds(0.1f);
        int random = Random.Range(0, 100);
        if (random < 20)
        {
            Debug.Log("Battle");
        }
        else
            StartCoroutine(LaunchBattle());

    }
    
    void Start()
    {
        StartCoroutine(LaunchBattle());
    }
}
