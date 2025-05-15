using System;
using UnityEngine;

namespace _Chainsaw.Scripts.Tutorial.Steps.StepExtras
{
    public class InStepChainsawHighlighter : MonoBehaviour
    {
        [SerializeField] private Chainsaw chainsaw;
        [SerializeField] private Chainsaw.ChainsawParts partToHighlight;
        ITutorialStep tutorialStep;

        public Chainsaw.ChainsawParts PartToHighlight => partToHighlight;

        private void OnValidate()
        {
            if(!chainsaw)
                chainsaw = FindAnyObjectByType<Chainsaw>();
        }

        private void OnEnable()
        {
            if (tutorialStep == null)
                tutorialStep = GetComponent<ITutorialStep>();

            tutorialStep.Started += OnStepStart;
            tutorialStep.Finished += OnStepFinish;
        }

        private void OnDisable()
        {
            tutorialStep.Started -= OnStepStart;
            tutorialStep.Finished -= OnStepFinish;
        }

        void OnStepStart()
        {
            chainsaw.HighlightPart(partToHighlight);
        }

        void OnStepFinish()
        {
            chainsaw.HidePartHighlight();
        }
    }
}
