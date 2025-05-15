using UnityEngine;

public class Removable : MonoBehaviour
{
    public void RemoveObject()
    {
        gameObject.SetActive(false);
    }
}
