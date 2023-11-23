using System;
using System.Collections.Generic;
using Narative;
using Unity.VisualScripting;
using UnityEngine;

namespace UI_Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private DialogueDisplayer dialogueDisplayer;
        [SerializeField] private UISelectionPanel uiSelectionPanel;
        [SerializeField] private ReadJsonNara jsonNara;
        
        [SerializeField] private List<StoryInteractor> storyInteractors;
        private StoryInteractor subscribedStoryInteractor;
        
        private void Start()
        {
            HideDialogues();
            uiSelectionPanel.gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            foreach (var storyInteractor in storyInteractors)
            {
                storyInteractor.DisplayDialogue += DisplayDialogue;
                storyInteractor.HideDialogue += HideDialogues;
            }
        }

        private void OnDisable()
        {
            foreach (var storyInteractor in storyInteractors)
            {
                storyInteractor.DisplayDialogue -= DisplayDialogue;
                storyInteractor.HideDialogue -= HideDialogues;
            }
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