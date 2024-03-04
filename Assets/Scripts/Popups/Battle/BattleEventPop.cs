using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class BattleEventPop : PopupBase
{
    [SerializeField] private CardController cardPrefab;

    [SerializeField] private Button btnHideCardDescription;

    [SerializeField] private Text txtStageNo;

    [SerializeField] private BattleEventManager battleEventManager;

    [SerializeField] private DescriptionPop descriptionPop;


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

        // スロットにセットされているすべてのカードを破棄
        battleEventManager.ResetBattleField();

        // 各カードデッキにカードを生成
        battleEventManager.Initialize(token).Forget(); //HidePopUp)

        base.ShowPopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public override void HidePopUp()
    {
        descriptionPop.HidePopUp();
        base.HidePopUp();
    }
}
