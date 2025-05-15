using System;
using UnityEngine;
using UnityEngine.Events;

public class WalkTarget : MonoBehaviour
{
    [SerializeField] private Transform walker;
    [SerializeField] private float triggerDistance = 0.5f;
    
    public UnityEvent TriggeredEvent;

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, walker.position) <= triggerDistance)
        {
            TriggeredEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
