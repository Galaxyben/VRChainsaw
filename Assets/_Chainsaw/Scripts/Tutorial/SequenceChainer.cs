using System;
using _Chainsaw.Scripts.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(TutorialSequencer))]
public class SequenceChainer : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonTmp;
    [SerializeField] private LocalizedString buttonText;
    [SerializeField] private TutorialSequencer tutorialSequencer;
    [SerializeField] private TutorialSequencer nextTutorialSequencer;
    [SerializeField] private OverlayFade overlayFade;
    [SerializeField] private bool fadeOutBeforeNext = true;

    private void OnValidate()
    {
        if(!tutorialSequencer)
            tutorialSequencer = GetComponent<TutorialSequencer>();
        
        if(!overlayFade)
            overlayFade = FindFirstObjectByType<OverlayFade>();
    }

    private void OnEnable()
    {
        tutorialSequencer.TutorialCompletedEvent += TutorialCompletedEvent;
    }

    private void OnDisable()
    {
        tutorialSequencer.TutorialCompletedEvent -= TutorialCompletedEvent;
    }
    
    private void TutorialCompletedEvent(ITutorialStep step)
    {
        
        buttonTmp.text = buttonText.GetLocalizedString();
        button.gameObject.SetActive(true);
        
        if(button.onClick == null)
            button.onClick = new Button.ButtonClickedEvent();
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(GoNextSequence);
    }

    private void GoNextSequence()
    {
        button.gameObject.SetActive(false);
        button.onClick.RemoveListener(GoNextSequence);
        if (fadeOutBeforeNext)
        {
            overlayFade.ToggleFade(true).OnComplete(() =>
            {
                Debug.Log("Starting next tutorial from overlay fade tween OnComplete");
                nextTutorialSequencer.StartTutorial();
            });
        }
        else
        {
            nextTutorialSequencer.StartTutorial();
        }
    }
}
