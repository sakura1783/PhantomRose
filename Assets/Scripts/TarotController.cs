using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

/// <summary>
/// タロットの種類
/// </summary>
public enum TarotType
{
    Circle,
    Triangle,
    Square,

    count,
}

public class TarotController : MonoBehaviour
{
    [SerializeField] private Button btnTarot;

    [SerializeField] private Image imgTarotDesign;

    [SerializeField] private CanvasGroup frontGroup;
    public CanvasGroup FrontGroup
    {
        get => frontGroup;
        set => frontGroup = value;
    }

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Sprite triangleSprite;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name=""></param>
    public void SetUp(MiniGamePop miniGamePop, TarotType tarotType)
    {
        // タロットの模様の設定
        if (tarotType == TarotType.Circle)
        {
            imgTarotDesign.sprite = circleSprite;
        }
        else if (tarotType == TarotType.Triangle)
        {
            imgTarotDesign.sprite = triangleSprite;
        }

        // ボタンの購読処理
        btnTarot.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                frontGroup.DOFade(1, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    frontGroup.blocksRaycasts = true;
                    DOVirtual.DelayedCall(0.5f, () => miniGamePop.selectedTarotList.Add(tarotType));  // 少し待ってからリストに要素を増加(めくったあと、すぐに裏返さない)
                });
            })
            .AddTo(this);
    }
}
