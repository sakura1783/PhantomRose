using System;
using System.Linq;

/// <summary>
/// 難易度の種類
/// </summary>
public enum DifficultyType
{
    Silver,
    Gold,
    Diamond1,
    Diamond2,
    Diamond3,
}

[Serializable]
public class StageDetailData
{
    public DifficultyType difficultyType;
    public int stageNo;
    public int routeIndex;

    public int purpleRewardCount;
    public int goldRewardCount;
    public int diamondRewardCount;
    public int rubyRewardCount;

    public int[] enemyIds;
    public int bossId;
    public int[] dropCards;


    /// <summary>
    /// コンストラクタ。スプレッドシートから情報を取得
    /// </summary>
    /// <param name="datas"></param>
    public StageDetailData(string[] datas)
    {
        difficultyType = (DifficultyType)Enum.Parse(typeof(DifficultyType), datas[0]);
        stageNo = int.Parse(datas[1]);
        routeIndex = int.Parse(datas[2]);
        purpleRewardCount = int.Parse(datas[3]);
        goldRewardCount = int.Parse(datas[4]);
        diamondRewardCount = int.Parse(datas[5]);
        rubyRewardCount = int.Parse(datas[6]);

        // 敵のIDを / の位置で分割して配列化
        string[] enemyArray = datas[7].Split("/");
        enemyIds = enemyArray.Select(int.Parse).ToArray();  // 要素をすべて変換する場合、Select(data => int.Parse(data))と書かなくても良い

        bossId = int.Parse(datas[8]);

        string[] dropCardArray = datas[9].Split("/");
        dropCards = dropCardArray.Select(int.Parse).ToArray();
    }
}
