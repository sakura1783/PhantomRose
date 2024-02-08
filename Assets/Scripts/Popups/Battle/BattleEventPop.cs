using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class BattleEventPop : PopupBase
{
    [SerializeField] private CardController cardPrefab;

    [SerializeField] private Transform attackCardTran;
    [SerializeField] private Transform magicCardTran;

    [SerializeField] private Button btnHideCardDescription;

    [SerializeField] private Text txtStageNo;

    [SerializeField] private BattleEventManager battleEventManager;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        btnHideCardDescription.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Where(_ => descriptionPop.IsDisplayDescriptionPop)
            .Subscribe(_ => descriptionPop.HidePopUp())
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        var token = this.GetCancellationTokenOnDestroy();

        // 各カードデッキにカードを生成
        battleEventManager.Initialize(token).Forget(); //HidePopUp)

        base.ShowPopUp();
    }
}
