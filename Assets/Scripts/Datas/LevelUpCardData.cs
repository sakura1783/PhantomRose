using UnityEngine;

[System.Serializable]
public class LevelUpCardData
{
    public int cardId;

    public int attackPower;
    public int shieldPower;
    public int recoveryPower;

    public int stateDuration;

    [Multiline] public string description;


    /// <summary>
    /// コンストラクタ。GSSから情報を取得し、SOへ代入
    /// </summary>
    /// <param name="datas"></param>
    public LevelUpCardData(string[] datas)
    {
        cardId = int.Parse(datas[0]);
        attackPower = int.Parse(datas[1]);
        shieldPower = int.Parse(datas[2]);
        recoveryPower = int.Parse(datas[3]);
        stateDuration = int.Parse(datas[4]);
        description = datas[5];
    }
}
