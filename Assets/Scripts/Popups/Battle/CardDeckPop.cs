using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class CardDeckPop : PopupBase
{
    [SerializeField] private CardController cardPrefab;

    [SerializeField] private Transform cardTran;

    [SerializeField] private Text txtCardCount;

    [SerializeField] private DescriptionPop descriptionPop;

    public ReactiveProperty<int> cardCount = new();

    private List<CardData> generatedCardList = new();


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        cardCount
            .Subscribe(_ => txtCardCount.text = $"{cardCount}/{GameDataManager.instance.gameData.handCardCapacity}")
            .AddTo(this);
    }

    /// <summary>
    /// カード追加
    /// </summary>
    /// <param name="data"></param>
    public void AddMyCard(CardData data)
    {
        // myCardListにカードを追加
        GameDataManager.instance.gameData.myCardList.Add(GameDataManager.instance.gameData.CurrentSerialNo, data.id);

        // カードデッキにカードを追加
        AddCardToCardDeck(data);

        GameDataManager.instance.gameData.CurrentSerialNo++;
    }

    /// <summary>
    /// カードデッキにカードを追加
    /// </summary>
    /// <param name="data"></param>
    private void AddCardToCardDeck(CardData data)
    {
        // デッキにカードを生成
        var cardObj = Instantiate(cardPrefab, cardTran);
        cardObj.SetUp(data, descriptionPop, GameDataManager.instance.gameData.CurrentSerialNo);

        generatedCardList.Add(cardObj.CardData);

        // 生成したカードをレベルが低い順(昇順)に並べ替え
        List<CardData> sortedList = generatedCardList.OrderBy(card => card.level).ToList();  // TODO 同じカードがあれば、並ぶようにする

        // 追加したカードの要素番号を取得
        int index = sortedList.FindIndex(x => x == data);

        // 指定したインデックスの位置にカードを移動
        cardObj.transform.SetSiblingIndex(index);  // TODO 適切な位置にカードが移動するか確認する

        cardCount.Value++;
    }

    /// <summary>
    /// TODO カードデッキからカードを削除
    /// </summary>
    public async UniTask RemoveCardFromCardDeck(CardController tapCard)
    {
        // カードを捨てるか確認用のポップアップを開く
        DiscardSubmitPop discardSubmitPop = (DiscardSubmitPop)PopupManager.instance.Show<DiscardSubmitPop>(true, false);

        bool btnResult = await discardSubmitPop.WaitConfirm();

        // 捨てるを選んだ場合
        if (btnResult)
        {
            // 捨てる際の処理
        }
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
