using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomizationManager : MonoBehaviour
{
    [Header("Customizables")]
    public Color mainColor;
    public Color highlightColor;
    public Color gripColor;

    public Texture logoTex;

    [Header("References")]
    public Renderer chainsawRend;
    public DecalProjector logoDecal;

    private Material m_chainsawMat;
    private Material m_logoMat;

    private void Start()
    {
        m_chainsawMat = chainsawRend.sharedMaterial;
        m_logoMat = logoDecal.material;

        SetupCustomization();
    }

    public void SetupCustomization()
    {
        m_chainsawMat.SetColor("_MainColor", mainColor);
        m_chainsawMat.SetColor("_HighlightColor", highlightColor);
        m_chainsawMat.SetColor("_GripColor", gripColor);

        m_logoMat.SetTexture("Base_Map", logoTex);
    }
}
