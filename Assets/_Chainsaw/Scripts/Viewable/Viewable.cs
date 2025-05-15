using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Chainsaw.Scripts.Viewable
{
    public class Viewable : MonoBehaviour
    {
        [SerializeField] private Camera viewerCamera;
        [SerializeField] private Transform target;
        [SerializeField] [Range(0, 1f)] float viewingPrecision = 0.5f;
        [SerializeField] private UnityEvent viewedEvent;
    
        private bool isViewed = false;

        private void OnValidate()
        {
            if(!viewerCamera)
                viewerCamera = Camera.main;
        }

        private void Update()
        {
            if (isViewed) return;
        
            Vector3 targetDir = (target.position - viewerCamera.transform.position).normalized;
            if (Vector3.Dot(viewerCamera.transform.forward, targetDir) > viewingPrecision)
            {
                isViewed = true;
                viewedEvent?.Invoke();
            }
        }
    }
}
