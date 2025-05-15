using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class UIProximityShow : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [SerializeField] Transform lookerTransform;

    private Tween moveTween;
    private Tween fadeTween;

    private bool isLookerInside = false;

    private void Start()
    {
        canvasGroup.enabled = false;
        canvasGroup.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, lookerTransform.position) < radius)
        {
            if (!isLookerInside)
            {
                isLookerInside = true;

                Show();
            }
        }
        else if(isLookerInside)
        {
            isLookerInside = false;

            Hide();
        }
    }
    
    
    private void Show()
    {
        moveTween?.Kill();
        fadeTween?.Kill();

        canvasGroup.enabled = true;

        fadeTween = canvasGroup.DOFade(1, 0.4f);
        moveTween = canvasGroup.transform.DOLocalMoveY(0, 0.4f).SetEase(Ease.OutSine);
    }
    
    private void Hide()
    {
        moveTween?.Kill();
        fadeTween?.Kill();

        fadeTween = canvasGroup.DOFade(0, 0.4f);
        moveTween = canvasGroup.transform.DOLocalMoveY(-0.3f, 0.4f).SetEase(Ease.InSine).OnComplete(() => canvasGroup.enabled = false);
    }
}
