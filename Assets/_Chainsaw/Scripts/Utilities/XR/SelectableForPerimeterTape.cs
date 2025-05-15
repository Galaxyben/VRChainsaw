using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace _Chainsaw.Scripts.Utilities.XR
{
    public class SelectableForPerimeterTape : XRSimpleInteractable
    {
        public override bool IsHoverableBy(IXRHoverInteractor interactor)
        {
            return interactor.handedness == InteractorHandedness.Right;
        }

        public override bool IsSelectableBy(IXRSelectInteractor interactor)
        {
            return false;
        }
    }
}
