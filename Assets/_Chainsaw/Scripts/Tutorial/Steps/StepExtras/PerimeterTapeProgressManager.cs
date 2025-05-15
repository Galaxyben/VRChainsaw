using System;
using _Chainsaw.Scripts.Tutorial.Steps.StepExtras;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class PerimeterTapeProgressManager : MonoBehaviour
{
    [SerializeField] private PerimeterTapeProgressPoint headPoint;
    [SerializeField] private PerimeterTape tape;
    public UnityEvent OnProgressFull;
    
    private PerimeterTapeProgressPoint currentPoint;
    
    private InputDevice rightHand;
    private bool isProgressing;

    private bool wasTriggerDown;
    
    void OnEnable()
    {
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitializeRightHand();
    }

    void OnDisable()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
    }
    
    private void Start()
    {
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightHand.isValid)
        {
            Debug.Log("Right hand device is valid: " + rightHand.name);
        }
        else
        {
            Debug.Log("Right hand device not valid yet. Will wait for deviceConnected event.");
        }
    }

    void Update()
    {
        if (isProgressing)
        {
            CheckTrigger(ref wasTriggerDown);
        }
    }

    public void StartProgress()
    {
        isProgressing = true;
        currentPoint = headPoint;
        headPoint.completeOnlyOnSelect = true;
        
        tape.SetTapeProgress(0);
        tape.gameObject.SetActive(true);

        currentPoint.Activate();
        currentPoint.IsPressDown = IsTriggerDown();
        currentPoint.CompletedEvent.AddListener(OnPointCompleted);
    }

    void CheckTrigger(ref bool previousState)
    {
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed))
        {
            if (isPressed && !previousState)
            {
                previousState = true;
                OnTriggerDown();
            }
            else if (!isPressed && previousState)
            {
                previousState = false;
                OnTriggerUp();
            }
        }
    }

    bool IsTriggerDown()
    {
        if(rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed))
            return isPressed;
        return false;
    }
    
    private void OnTriggerDown()
    {
        currentPoint.IsPressDown = true;
    }

    private void OnTriggerUp()
    {
        currentPoint.IsPressDown = false;
        //currentPoint.Cancel();
        //PrevPoint();
    }

    private void NextPoint()
    {
        currentPoint.Deactivate();
        currentPoint.CompletedEvent.RemoveListener(OnPointCompleted);
        
        if (currentPoint.nextPoint != null)
        {
            currentPoint = currentPoint.nextPoint;
            currentPoint.Activate();
            currentPoint.IsPressDown = IsTriggerDown();
            currentPoint.CompletedEvent.AddListener(OnPointCompleted);
            
        }
    }

    private void PrevPoint()
    {
        currentPoint.Deactivate();
        currentPoint.CompletedEvent.RemoveListener(OnPointCompleted);

        if (currentPoint.previousPoint != null)
        {
            currentPoint = currentPoint.previousPoint;  
            currentPoint.Activate();
            currentPoint.IsPressDown = IsTriggerDown();
            currentPoint.CompletedEvent.AddListener(OnPointCompleted);
        }
    }

    private void OnPointCompleted()
    {
        if (currentPoint.nextPoint != null)
        {
            NextPoint(); //enable next collider to touch
        }
        else
        {
            currentPoint.gameObject.SetActive(false);
            isProgressing = false;
            OnProgressFull?.Invoke();
        }
    }
    
    
    
    void TryInitializeRightHand()
    {
        var device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (device.isValid)
        {
            rightHand = device;
            Debug.Log("Right hand initialized: " + device.name);
        }
    }

    void OnDeviceConnected(InputDevice device)
    {
        if ((device.characteristics & InputDeviceCharacteristics.Right) != 0 &&
            (device.characteristics & InputDeviceCharacteristics.Controller) != 0)
        {
            rightHand = device;
            Debug.Log("Right hand device connected: " + device.name);
        }
    }
}
