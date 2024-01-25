using UnityEngine;
using UniRx;
using DG.Tweening;

public class TitlePop : PopupBase
{
    [SerializeField] private CanvasGroup tapPromptGroup;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        PopupManager.instance.Show<TitlePop>(false, false);

        btnClose.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                PopupManager.instance.Show<HomeAlwaysPop>(false);
                PopupManager.instance.Show<MyRoomPop>(false, false);
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public override void ShowPopUp(CardData data = null)
    {
        canvasGroup.DOFade(1, 0.5f)
            .SetEase(ease)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = true;
                tapPromptGroup.DOFade(1, 1f).SetEase(ease).SetLoops(5, LoopType.Yoyo);
            });
    }
}
