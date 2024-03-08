using System;
using System.Linq;

/// <summary>
/// 難易度の種類
/// </summary>
public enum DifficultyType
{
    Silver,
    Gold,
    Diamond,
}

[Serializable]
public class StageDetailData
{
    public DifficultyType difficultyType;
    public int stageNo;
    //public int routeIndex;

    //public int purpleRewardCount;
    //public int goldRewardCount;
    //public int diamondRewardCount;
    //public int rubyRewardCount;

    public int[] enemyIds;
    public int bossId;
    public int[] dropCards;  // 探索イベントやステージ終了後にもらえるカードの種類


    /// <summary>
    /// コンストラクタ。スプレッドシートから情報を取得
    /// </summary>
    /// <param name="datas"></param>
    public StageDetailData(string[] datas)
    {
        difficultyType = (DifficultyType)Enum.Parse(typeof(DifficultyType), datas[0]);
        stageNo = int.Parse(datas[1]);
        //routeIndex = int.Parse(datas[2]);
        //purpleRewardCount = int.Parse(datas[2]);
        //goldRewardCount = int.Parse(datas[3]);
        //diamondRewardCount = int.Parse(datas[4]);
        //rubyRewardCount = int.Parse(datas[5]);

        // 敵のIDを / の位置で分割して配列化
        string[] enemyArray = datas[2].Split("/");
        enemyIds = enemyArray.Select(int.Parse).ToArray();  // 要素をすべて変換する場合、Select(data => int.Parse(data))と書かなくても良い

        bossId = int.Parse(datas[3]);

        string[] dropCardArray = datas[4].Split("/");
        dropCards = dropCardArray.Select(int.Parse).ToArray();
    }
}
