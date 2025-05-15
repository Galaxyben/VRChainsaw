using System;
using _Chainsaw.Scripts.Tutorial;
using UnityEngine;

public class TutorialSfx : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TutorialSequencer tutorialSequencer;
    [SerializeField] AudioSource audioSource;
    
    [Header("Settings")]
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip canceledClip;

    private void OnValidate()
    {
        var ts = GetComponent<TutorialSequencer>();
        if (ts)
            tutorialSequencer = ts;
    }

    private void OnEnable()
    {
        tutorialSequencer.StepFinishedEvent += StepFinishedEventHandler;
        tutorialSequencer.StepCanceledEvent += StepCanceledEventHandler;
    }

    private void OnDisable()
    {
        tutorialSequencer.StepFinishedEvent -= StepFinishedEventHandler;
        tutorialSequencer.StepCanceledEvent -= StepCanceledEventHandler;
    }

    private void StepFinishedEventHandler(ITutorialStep step)
    {
        audioSource.PlayOneShot(successClip);
    }
    
    private void StepCanceledEventHandler(ITutorialStep step)
    {
        audioSource.PlayOneShot(canceledClip);
    }
}
