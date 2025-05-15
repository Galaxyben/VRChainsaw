using _Chainsaw.Scripts.Tutorial;
using UnityEngine;
using UnityEngine.Events;

public class TutorialCompleteEventHook : MonoBehaviour
{
    [SerializeField] TutorialSequencer tutorialSequencer;

    [SerializeField] UnityEvent<ITutorialStep> unityEvent;
    
    private void OnEnable()
    {
        tutorialSequencer.TutorialCompletedEvent += OnTutorialCompleteHandler;
    }
    
    private void OnDisable()
    {
        tutorialSequencer.TutorialCompletedEvent -= OnTutorialCompleteHandler;
    }


    private void OnTutorialCompleteHandler(ITutorialStep step)
    {
        unityEvent?.Invoke(step);
    }
}
