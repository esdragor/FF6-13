using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private IntroAnimation introAnimation;
    [SerializeField] private RectTransform menuCursor;
    [SerializeField] private Image fadeImage;
    [Header("Buttons")]
    [SerializeField] private UIButton playButton;
    [SerializeField] private UIButton loadButton;
    [SerializeField] private UIButton quitButton;
    [Header("Settings")]
    [SerializeField] private float fadeTime = 1f;
    
    private void Start()
    {
        playButton.OnButtonSelected += OnButtonSelected;
        loadButton.OnButtonSelected += OnButtonSelected;
        quitButton.OnButtonSelected += OnButtonSelected;
        
        playButton.Button.onClick.AddListener(StartGame);
        loadButton.Button.onClick.AddListener(LoadGame);
        quitButton.Button.onClick.AddListener(QuitGame);
    }

    private void LoadGame()
    {
        
    }
    
    private void StartGame()
    {
        playButton.OnButtonSelected -= OnButtonSelected;
        loadButton.OnButtonSelected -= OnButtonSelected;
        quitButton.OnButtonSelected -= OnButtonSelected;
        
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
    
    private void OnButtonSelected(Component button)
    {
        menuCursor.SetParent(button.transform);
        menuCursor.anchoredPosition = Vector2.zero;
    }
    
    
}
