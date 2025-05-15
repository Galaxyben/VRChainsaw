using UnityEngine;
using UnityEngine.Rendering;

public class Gazeable : MonoBehaviour
{
    public GameObject icon;
    private Vector3 m_ogScale;

    private void Start()
    {
        m_ogScale = icon.transform.localScale;
    }

    public void Gazing(float _gazeValue)
    {
        icon.transform.localScale = m_ogScale - (_gazeValue * Vector3.one * 0.01f);
    }

    public void ResetGaze()
    {
        icon.transform.localScale = m_ogScale;
    }
}
