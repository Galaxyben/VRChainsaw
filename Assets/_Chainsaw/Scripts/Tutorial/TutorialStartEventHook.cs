using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Chainsaw.Scripts.Tutorial
{
    public class TutorialStartEventHook : MonoBehaviour
    {
        [SerializeField] TutorialSequencer tutorialSequencer;

        [SerializeField] UnityEvent<ITutorialStep> unityEvent;

        private void OnValidate()
        {
            if(!tutorialSequencer)
                tutorialSequencer = GetComponent<TutorialSequencer>();
        }

        private void OnEnable()
        {
            tutorialSequencer.TutorialStartEvent += OnTutorialStartHandler;
        }
    
        private void OnDisable()
        {
            tutorialSequencer.TutorialStartEvent -= OnTutorialStartHandler;
        }


        private void OnTutorialStartHandler(ITutorialStep step)
        {
            unityEvent?.Invoke(step);
        }
    }
}