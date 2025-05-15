using UnityEngine;

public class PerimeterTape : MonoBehaviour
{
    public MeshRenderer tapeRend;

    public void SetTapeProgress(float _progress)
    {
        _progress = Mathf.Clamp01(_progress);

        tapeRend.sharedMaterial.SetFloat("_progression", _progress);
    }
}
