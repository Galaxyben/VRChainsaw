using UnityEngine;

public class Cap : MonoBehaviour
{
    public GameObject[] meshes;
    private bool m_enabled = true;

    /// <summary>
    /// syncs gameobject enabled to first mesh
    /// </summary>
    public void ToggleAllMeshes()
    {
        m_enabled = !meshes[0].activeSelf;
        foreach (GameObject go in meshes)
        {
            go.SetActive(m_enabled);
        }
    }

    public bool IsOpen()
    {
        return !m_enabled;
    }
}
