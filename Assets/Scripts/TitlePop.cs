using UnityEngine;
using DG.Tweening;

public class TitlePop : PopupBase
{
    [SerializeField] private CanvasGroup tapPromptGroup;


    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public override void ShowPopUp()
    {
        canvasGroup.DOFade(1, 0.3f)
            .SetEase(ease)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = true;
                tapPromptGroup.DOFade(1, 0.5f).SetEase(ease).SetLoops(2, LoopType.Yoyo);
            });
    }
}
