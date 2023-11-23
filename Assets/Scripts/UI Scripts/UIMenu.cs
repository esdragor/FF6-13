using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private IntroAnimation introAnimation;
    [SerializeField] private Image fadeImage;
    [Header("Buttons")]
    [SerializeField] private UIButton playButton;
    [SerializeField] private UIButton loadButton;
    [SerializeField] private UIButton quitButton;
    [Header("Settings")]
    [SerializeField] private float fadeTime = 1f;

    [Header("Debug")]
    [SerializeField] private bool canLoadGame = false;
    
    private void Start()
    {
        playButton.OnButtonSelected += UICursor.SetSelectable;
        loadButton.OnButtonSelected += UICursor.SetSelectable;
        quitButton.OnButtonSelected += UICursor.SetSelectable;
        
        playButton.Button.onClick.AddListener(StartGame);
        loadButton.Button.onClick.AddListener(LoadGame);
        quitButton.Button.onClick.AddListener(QuitGame);
        
        EnableLoadGame(canLoadGame);
    }

    private void EnableLoadGame(bool value)
    {
        loadButton.Button.interactable = value;

        if (!value) return;

        loadButton.TextMeshProUGUI.color = Color.black;
        loadButton.Button.Select();
    }

    private void LoadGame()
    {
        
    }
    
    private void StartGame()
    {
        playButton.OnButtonSelected -= UICursor.SetSelectable;
        loadButton.OnButtonSelected -= UICursor.SetSelectable;
        quitButton.OnButtonSelected -= UICursor.SetSelectable;
        
        Fade(fadeTime,OnGameStarted);
    }
    
    private void QuitGame()
    {
        Fade(fadeTime,Application.Quit);
    }

    private void Fade(float duration,Action callback)
    {
        fadeImage.DOKill();
        fadeImage.color = Color.clear;
        fadeImage.DOFade(1,duration).OnComplete(() => callback?.Invoke());
    }

    private void OnGameStarted()
    {
        gameObject.SetActive(false);
        introAnimation.StartAnimation();
    }

}
