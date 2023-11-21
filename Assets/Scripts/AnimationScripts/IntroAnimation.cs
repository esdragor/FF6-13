using UnityEngine;

public class IntroAnimation : MonoBehaviour
{
    [SerializeField] private Animator introAnimator;
    [SerializeField] private AudioSource audioSource;
    private static readonly int Play = Animator.StringToHash("Play");

    [ContextMenu("Play Intro Animation")]
    public void StartAnimation()
    {
        introAnimator.SetTrigger(Play);
        audioSource.Play();
    }
}
