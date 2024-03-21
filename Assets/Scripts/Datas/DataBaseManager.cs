using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    // 各スクリプタブルオブジェクト
    public CardDataSO cardDataSO;
    public LevelUpCardDataSO levelUpCardDataSO;
    public StateDataSO stateDataSO;
    public ItemDataSO itemDataSO;
    public OpponentDataSO opponentDataSO;
    public StageDetailDataSO stageDetailDataSO;
    public DifficultyLevelDataSO difficultyLevelDataSO;

    public List<CardData> copyCardDataList = new();  // CardDataSOのCardDataListをディープコピーしたもの。この情報を使って、カードの生成や値の変更を行う


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
    //public void SetMyCardList()
    //{
    //    foreach (CardData cardData in cardDataSO.cardDataList)
    //    {
    //        GameData.instance.myCardList.Add(cardData);
    //    }

    //    上記をLinqを使ってリファクタリング
    //    GameData.instance.myCardList.AddRange(cardDataSO.cardDataList);

    //    プレイヤーの初期カードを設定
    //    for (int i = 0; i < 7; i++)
    //    {
    //        GameData.instance.myCardList.Add(cardDataSO.cardDataList[i]);
    //    }
    //}

    /// <summary>
    /// StageDataを取得
    /// </summary>
    /// <param name="stageDetailData"></param>
    /// <returns></returns>
    public StageDataSO GetStageDataSOFromDifficultyLevel(StageDetailData stageDetailData)
    {
        // 難易度取得
        var difficultyLevelData = difficultyLevelDataSO.difficultyLevelDataList.FirstOrDefault(data => data.difficultyType == stageDetailData.difficultyType);

        // ステージデータを取得
        return difficultyLevelData.stageDataList[stageDetailData.stageNo];

        // 上記を1行で書く場合
        //return difficultyLevelDataSO.difficultyLevelDataList.FirstOrDefault(data => data.difficultyType == stageDetailData.difficultyType).stageDataList[stageDetailData.stageNo];
    }

    /// <summary>
    /// 指定した難易度のStageDetailDataを取得
    /// </summary>
    /// <param name="difficultyType"></param>
    /// <param name="stageNo"></param>
    /// <returns></returns>
    public StageDetailData GetStageDetailData(DifficultyType difficultyType, int stageNo)
    {
        // StageDataList内から、指定した難易度のStageDetailDataのみを抽出する
        var targetDifficultyStageDetailDataList = stageDetailDataSO.stageDetailDataList.Where(data => data.difficultyType == difficultyType).ToList();

        return targetDifficultyStageDetailDataList.FirstOrDefault(data => data.stageNo == stageNo);
    }
}
