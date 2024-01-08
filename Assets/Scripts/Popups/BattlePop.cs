using UnityEngine;

public class BattlePop : PopupBase
{
    [SerializeField] private CardPrefab cardPrefab;

    //TODO テスト。終わったら修正する
    [SerializeField] private Transform cardTran;


    //TODO テスト用。終わったら消す
    void Start()
    {
        foreach (var data in DataBaseManager.instance.cardDataSO.cardDataList)
        {
            CardPrefab card = Instantiate(cardPrefab, cardTran);

            card.SetUp(data);
        }
    }
}
