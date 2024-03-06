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


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        cardCount
            .Subscribe(_ => txtCardCount.text = $"{cardCount}/{GameData.instance.handCardCapacity}")
            .AddTo(this);
    }

    /// <summary>
    /// カードデッキにカードを追加
    /// Character.HandCardListに新しいカードの情報を追加してから実行する
    /// </summary>
    /// <param name="data"></param>
    public void AddCardToCardDeck(CardData data)
    {
        // Listをレベルが低い順(昇順)に並べ替え
        List<CardData> sortedList = GameData.instance.GetPlayer().HandCardList.OrderBy(card => card.level).ToList();

        // 追加するカードの要素番号を取得
        int index = sortedList.FindIndex(x => x == data);

        var cardObj = Instantiate(cardPrefab, cardTran);
        cardObj.SetUp(data, descriptionPop);
        cardObj.transform.SetSiblingIndex(index);  // 指定したインデックスの位置にカードを生成

        cardCount.Value++;
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
