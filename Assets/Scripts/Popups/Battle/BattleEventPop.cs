using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BattleEventPop : PopupBase
{
    [SerializeField] private Button btnHideCardDescription;

    [SerializeField] private CardController cardPrefab;

    [SerializeField] private DescriptionPop descriptionPop;

    //TODO テスト。終わったら修正する
    //[SerializeField] private Transform cardTran;


    //TODO テスト用。終わったら消す
    //void Start()
    //{
    //    foreach (var data in DataBaseManager.instance.cardDataSO.cardDataList)
    //    {
    //        CardController card = Instantiate(cardPrefab, cardTran);

    //        card.SetUp(data);
    //    }
    //}

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

    //TODO ShowPopUpのオーバーライド
}
