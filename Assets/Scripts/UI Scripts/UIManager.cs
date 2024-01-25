using System.Collections.Generic;
using DG.Tweening;
using Narative;
using UnityEngine;

namespace UI_Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private DialogueDisplayer dialogueDisplayer;
        [SerializeField] private UISelectionPanel uiSelectionPanel;
        [SerializeField] private ReadJsonNara jsonNara;
        [Space]
        [SerializeField] private CanvasGroup fadeToBlackCanvasGroup;
        [SerializeField] private GameObject fadeToBlackPanel;

            
            
        private StoryInteractor subscribedStoryInteractor;
        
        private void Start()
        {
            HideDialogues();
            uiSelectionPanel.gameObject.SetActive(false);
            fadeToBlackPanel.SetActive(false);
        }
        
        private void OnEnable()
        {
            StoryInteractor.DisplayDialogue += DisplayDialogue; 
            StoryInteractor.HideDialogue += HideDialogues;
            StoryInteractor.FadeToBlack += FadeToBlack;
        }

        private void OnDisable()
        {
            StoryInteractor.DisplayDialogue -= DisplayDialogue;
            StoryInteractor.HideDialogue -= HideDialogues;
            StoryInteractor.FadeToBlack -= FadeToBlack;
        }

        private void FadeToBlack(float duration, bool fadeIn)
        {
            fadeToBlackPanel.SetActive(true);
            fadeToBlackCanvasGroup.alpha = fadeIn ? 0f : 1f;
            fadeToBlackCanvasGroup.DOFade(fadeIn ? 1f : 0f, duration).OnComplete(() => fadeToBlackPanel.SetActive(fadeIn));
        }

        private void DisplayDialogue(StoryInteractor storyInteractor, string id, bool top)
        {
            var dialog = jsonNara.allDialogs.GetDialog(id);
            if (dialog == null) return;

            dialogueDisplayer.DisplayText(dialog.dialogTxt, dialog.title, false, top);
            
            subscribedStoryInteractor = storyInteractor;
            dialogueDisplayer.OnDialogueFullyDisplayed += ResumeStoryInteractor;
        }
        
        private void ResumeStoryInteractor()
        {
            subscribedStoryInteractor.ResumeAfterDialogue();
            dialogueDisplayer.OnDialogueFullyDisplayed -= ResumeStoryInteractor;
        }

        private void HideDialogues()
        {
            dialogueDisplayer.HidePanel();
        }
    }
}