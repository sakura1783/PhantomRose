using UnityEngine;

/// <summary>
/// バフかデバフか
/// </summary>
public enum StateType
{
    バフ,
    デバフ,
}

/// <summary>
/// 状態の種類
/// </summary>
public enum ConditionType
{
    出血,
    集中,
    吸血,
    攻撃強化,
}

[System.Serializable]
public class StateData
{
    public int id;
    public ConditionType stateName;
    public StateType stateType;
    public int spriteId;

    public int attackPower;
    public int shieldPower;
    public int recoveryPower;

    [Multiline] public string description;


    /// <summary>
    /// コンストラクタ。スプレッドシートから届く情報をキャストして、各変数に代入
    /// </summary>
    /// <param name="datas"></param>
    public StateData(string[] datas)
    {
        id = int.Parse(datas[0]);
        stateName = (ConditionType)System.Enum.Parse(typeof(ConditionType), datas[1]);
        stateType = (StateType)System.Enum.Parse(typeof(StateType), datas[2]);
        spriteId = int.Parse(datas[3]);
        attackPower = int.Parse(datas[4]);
        shieldPower = int.Parse(datas[5]);
        recoveryPower = int.Parse(datas[6]);
        description = datas[7];
    }


    //TODO 追加する状態異常
    // Hpが相手よりも低い場合、残り時間に応じてシールドを獲得する
}

/// <summary>
/// 状態異常データの簡易版
/// カードごとに異なる情報を持たせたい場合に使う
/// </summary>
[System.Serializable]
public class SimpleStateData
{
    public ConditionType stateName;
    public int stateId;
    public int duration;  // 同じ状態異常でもカードごとに継続時間を変更できる(性能差を持たせる)
}
