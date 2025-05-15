using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OverlayFade : MonoBehaviour
{
    public Image fade_img;

    private void Start()
    {
        ToggleFade(false);
    }

    public Tweener ToggleFade(bool _toggle)
    {
        return fade_img.DOColor(new Color(0f, 0f, 0f, _toggle ? 1f : 0f), 1f).SetEase(Ease.InCirc);
    }

    public void ToggleFadeOption(bool _toggle)
    {
        ToggleFade(_toggle);
    }
}
