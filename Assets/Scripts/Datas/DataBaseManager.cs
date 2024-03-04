using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    // 各スクリプタブルオブジェクト
    public CardDataSO cardDataSO;
    public StateDataSO stateDataSO;
    public ItemDataSO itemDataSO;
    public StageDetailDataSO stageDetailDataSO;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// myCardListの設定
    /// </summary>
    public void SetCardData()
    {
        //foreach (CardData cardData in cardDataSO.cardDataList)
        //{
        //    GameData.instance.myCardList.Add(cardData);
        //}

        // 上記をLinqを使ってリファクタリング
        //GameData.instance.myCardList.AddRange(cardDataSO.cardDataList);

        // プレイヤーの初期カードを設定
        for (int i = 0; i < 7; i++)
        {
            GameData.instance.myCardList.Add(cardDataSO.cardDataList[i]);
        }
    }
}
