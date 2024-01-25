using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class PopupBase : MonoBehaviour
{
    [SerializeField] protected Button btnClose;

    [SerializeField] protected Canvas canvas;

    [SerializeField] protected CanvasGroup canvasGroup;

    [SerializeField] protected Ease ease;


    protected virtual void Reset()
    {
        if (TryGetComponent(out canvas))
        {
            Debug.Log($"{canvas} 取得しました");
        }
        else
        {
            Debug.Log($"{this} Canvasが取得できません");
        }

        if (TryGetComponent(out canvasGroup))
        {
            Debug.Log($"{canvasGroup} 取得しました");
        }
        else
        {
            Debug.Log($"{this} CanvasGroupが取得できません");
        }
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public virtual void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        btnClose.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2))
            .Subscribe(_ => PopupManager.instance.GoBack())
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public virtual void ShowPopUp(CardData data = null)  // DescriptionPopでCardData型が必要なので、省略可能な引数を追加
    {
        canvasGroup.DOFade(1, 0.5f)
            .SetEase(ease)
            .OnComplete(() => canvasGroup.blocksRaycasts = true);
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public virtual void HidePopUp()
    {
        canvasGroup.DOFade(0, 0.5f)
            .SetEase(ease)
            .OnComplete(() => canvasGroup.blocksRaycasts = false);
    }
}
