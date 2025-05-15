using System;
using UnityEngine;

namespace _Chainsaw.Scripts.Tutorial
{
    public class TutorialStepSfx : MonoBehaviour
    {
        [Header("References")]
        ITutorialStep tutorialStep;
        [SerializeField] AudioSource audioSource;
    
        [Header("Settings")]
        [SerializeField] private AudioClip successClip;

        private void OnValidate()
        {
            if(!audioSource)
                audioSource = GetComponentInParent<AudioSource>();
        }

        private void OnEnable()
        {
            tutorialStep ??= GetComponent<ITutorialStep>();

            tutorialStep.Finished += StepFinishedEventHandler;
        }

        private void OnDisable()
        {
            tutorialStep.Finished -= StepFinishedEventHandler;
        }

        private void StepFinishedEventHandler()
        {
            audioSource.PlayOneShot(successClip);
        }
    }
}
