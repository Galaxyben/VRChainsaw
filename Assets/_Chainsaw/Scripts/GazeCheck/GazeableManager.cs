using UnityEngine;
using UnityEngine.Events;

public class GazeableManager : MonoBehaviour
{
    public float gazeTimer;

    public LayerMask gazablesMask;

    public bool gazingEnabled = false;

    public Camera cam;

    public UnityEvent OnGazeCompleted;

    private float lastTime;
    private bool isGazing = false;
    private Gazeable gazeable;

    private void Update()
    {
        if (gazingEnabled)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, gazablesMask))
            {
                if (!isGazing)
                {
                    isGazing = true;
                    lastTime = Time.time;

                    gazeable = hit.collider.GetComponent<Gazeable>();
                }
            } else
            {
                if (isGazing)
                {
                    isGazing = false;

                    gazeable.ResetGaze();
                    gazeable = null;
                }
            }

            if (isGazing)
            {
                gazeable.Gazing(Time.time - lastTime);
                if(Time.time > lastTime + gazeTimer)
                {
                    CompletedGaze();
                }
            }
        }
    }

    public void CompletedGaze()
    {
        isGazing = false;
        gazeable.ResetGaze();
        gazeable.gameObject.SetActive(false);
        gazeable = null;
        gazingEnabled = false;

        OnGazeCompleted.Invoke();
    }
}
