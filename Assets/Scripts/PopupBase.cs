using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class PopupBase : MonoBehaviour
{
    [SerializeField] protected Button btnClose;

    [SerializeField] protected CanvasGroup canvasGroup;

    [SerializeField] protected Ease ease;


    /// <summary>
    /// 初期設定
    /// </summary>
    public virtual void SetUp()
    {
        canvasGroup.alpha = 0;

        btnClose.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(1))
            .Subscribe(_ => HidePopUp()).AddTo(this);

        ShowPopUp();
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public virtual void ShowPopUp()
    {
        canvasGroup.DOFade(1, 0.5f)
            .SetEase(ease);
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public virtual void HidePopUp()
    {
        canvasGroup.DOFade(0, 0.5f)
            .SetEase(ease);
    }
}
