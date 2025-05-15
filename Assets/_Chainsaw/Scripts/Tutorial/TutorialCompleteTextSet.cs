using _Chainsaw.Scripts.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class TutorialCompleteTextSet : MonoBehaviour
{
    [SerializeField] TutorialSequencer tutorialSequencer;
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private LocalizedString message;
    
    bool inChargeOfText = false;
    
    private void OnEnable()
    {
        tutorialSequencer.TutorialCompletedEvent += OnTutorialCompleteHandler;
        message.StringChanged += MessageOnStringChanged;
    }

    private void MessageOnStringChanged(string value)
    {
        if(inChargeOfText)
            targetText.text = value;
    }

    private void OnDisable()
    {
        tutorialSequencer.TutorialCompletedEvent -= OnTutorialCompleteHandler;
        message.StringChanged -= MessageOnStringChanged;
    }


    private void OnTutorialCompleteHandler(ITutorialStep step)
    {
        targetText.text = message.GetLocalizedString();
        inChargeOfText = true;
    }
}
