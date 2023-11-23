using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    
    private void LoadGameScene()
    {
        Debug.Log("Loading Scene");
        SceneManager.LoadScene(1);
    }
}
