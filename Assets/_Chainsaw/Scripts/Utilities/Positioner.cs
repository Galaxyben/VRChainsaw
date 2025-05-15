using System;
using UnityEngine;

public class Positioner : MonoBehaviour
{
    [SerializeField] Set[] positions;

    public void SetPositions()
    {
        foreach (Set set in positions)
        {
            if (set.local)
            {
                set.target.localPosition = set.position;
                set.target.localRotation = Quaternion.Euler(set.rotation);
            }
            else
            {
                set.target.position = set.position;
                set.target.rotation = Quaternion.Euler(set.rotation);
            }
        }
    }
    
    [Serializable]
    public struct Set
    {
        public Transform target;
        public Vector3 position;
        public Vector3 rotation;
        public bool local;
    }
}
