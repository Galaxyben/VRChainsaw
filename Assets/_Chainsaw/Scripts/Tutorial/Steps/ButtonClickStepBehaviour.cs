using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace _Chainsaw.Scripts.Tutorial.Steps
{
    public class ButtonClickStepBehaviour : MonoBehaviourStepWrapper<ButtonClickStep> { }

    [Serializable]
    public class ButtonClickStep : BasicStep, ITutorialStep
    {
        [SerializeField] private LocalizedString buttonText;
        [SerializeField] private Button.ButtonClickedEvent clickedEvent;
        [SerializeField] private Button overrideButton;
        
        private TutorialDisplayUtilities displayUtilities;

        private Button MyButton => overrideButton ? overrideButton : displayUtilities.button;
        
        void Start()
        {
            
        }
        
        public override void Initialize(TutorialDisplayUtilities _displayUtilities)
        {
            base.Initialize(_displayUtilities);

            displayUtilities = _displayUtilities;
            
            clickedEvent.AddListener(Finish);
            MyButton.onClick = clickedEvent;
            if (!buttonText.IsEmpty)
            {
                displayUtilities.buttonTmp.text = buttonText.GetLocalizedString();
                MyButton.gameObject.SetActive(true);
            }

            Started?.Invoke();
        }

        public void Cancel()
        {
            base.Finish();
            clickedEvent.RemoveListener(Finish);
        }

        public void Tick(float deltaTime) { }
        public void ShouldGoBackCheck(float deltaTime)
        {
            //Can't go back here
        }

        public void BackStepCheckStarted()
        {
            
        }

        public void BackStepCheckFinished()
        {
            
        }

        public override void Finish()
        {
            base.Finish();
            
            clickedEvent.RemoveListener(Finish);
            MyButton.onClick = null;
            MyButton.gameObject.SetActive(false);
            
            Finished?.Invoke();
        }

        public Action Started { get; set; }
        public Action Finished { get; set; }
        public Action GoBack { get; set; }
        public bool IsRunning { get; set; }
    }
}