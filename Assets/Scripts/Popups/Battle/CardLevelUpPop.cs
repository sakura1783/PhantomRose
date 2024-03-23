using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CardLevelUpPop : PopupBase
{
    [SerializeField] private Transform cardPlace;

    [SerializeField] private LevelupCardObjController levelupCardPrefab;

    [SerializeField] private LevelupDetailPop levelupDetailPop;
    public LevelupDetailPop LevelupDetailPop => levelupDetailPop;

    [SerializeField] private int generateCardCount;


    public override void SetUp()
    {
        base.SetUp();

        // テスト用
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => PopupManager.instance.Show<CardLevelUpPop>(false, false))
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        // 各Canvasの表示を初期化
        levelupDetailPop.CanvasGroup.alpha = 0;
        levelupDetailPop.CanvasGroup.blocksRaycasts = false;

        // 前回生成したカードを破棄
        foreach (Transform child in cardPlace)
        {
            Destroy(child.gameObject);
        }

        GenerateRandomCard();

        base.ShowPopUp();
    }

    /// <summary>
    /// ランダムなカードを生成する。すでにレベルアップされているカードは生成しない
    /// </summary>
    private void GenerateRandomCard()
    {
        // 持っている全カードの通し番号の情報を持つListを作成
        List<int> myCardSerialList = new();
        foreach (var card in GameDataManager.instance.gameData.myCardList)
        {
            myCardSerialList.Add(card.Key);
        }

        // myCardSerialList - levelupCardSerialNumbersの差集合のList
        List<int> deltaSerialList = myCardSerialList.Except(GameDataManager.instance.gameData.levelupCardSerialNumbers).ToList();

        // 上のListをランダムに並び替え
        List<int> generateSerialList = deltaSerialList.OrderBy(_ => Random.value).Take(generateCardCount).ToList();

        // 持っているカードからランダムにカードを生成
        //Dictionary<int, int> cardList = GameDataManager.instance.gameData.myCardList.OrderBy(_ => Random.value).Take(2).ToDictionary(card => card.Value, card => card.Key);
        foreach (var serialNo in generateSerialList)
        {
            var cardObj = Instantiate(levelupCardPrefab, cardPlace);
            cardObj.SetUp(DataBaseManager.instance.cardDataSO.cardDataList[GameDataManager.instance.gameData.myCardList[serialNo]], this);
            cardObj.SerialNo = serialNo;  // 通し番号を持たせる
        }
    }
}
