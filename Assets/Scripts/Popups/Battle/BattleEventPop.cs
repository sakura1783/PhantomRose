using UnityEngine;

public class BattleEventPop : PopupBase
{
    [SerializeField] private CardController cardPrefab;

    //TODO テスト。終わったら修正する
    //[SerializeField] private Transform cardTran;

    //TODO 変数追加


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
    }

    //TODO ShowPopUpのオーバーライド
}
