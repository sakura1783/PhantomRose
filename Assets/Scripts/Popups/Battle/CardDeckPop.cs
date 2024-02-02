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

    public ReactiveProperty<int> cardCount = new();


    // TODO テスト
    private void Start()
    {
        foreach (var card in DataBaseManager.instance.cardDataSO.cardDataList)
        {
            GameData.instance.myCardList.Add(card);
        }

        // リストが変更されないように、リストをコピー
        //var copyList = GameData.instance.myCardList;  // TODO エラーになる。かといって新しくインスタンスするとデータの中身が2倍になってしまう

        //foreach (var card in copyList)
        //{
        //    AddCardToCardDeck(card);
        //}
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        cardCount
            .Subscribe(_ => txtCardCount.text = cardCount.Value.ToString())
            .AddTo(this);
    }

    /// <summary>
    /// カードデッキにカードを追加
    /// </summary>
    /// <param name="card"></param>
    public void AddCardToCardDeck(CardData data)
    {
        // GameDataのListにカードを追加
        GameData.instance.myCardList.Add(data);

        // Listをレベルが低い順(昇順)に並べ替え
        List<CardData> sortedList = GameData.instance.myCardList.OrderBy(card => card.level).ToList();

        // 追加するカードの要素番号を取得
        int index = sortedList.FindIndex(x => x == data);

        var cardObj = Instantiate(cardPrefab, cardTran);
        cardObj.SetUp(data);
        cardObj.transform.SetSiblingIndex(index);  // 指定したインデックスの位置にカードを生成

        cardCount.Value++;
    }
}
