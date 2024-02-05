using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// オーナーの種類
/// </summary>
public enum OwnerStatus
{
    Player,
    Opponent,  // 対戦相手
}

[System.Serializable]  // インスペクターにクラス内部が表示される
public class Character
{
    public ReactiveCollection<CardData> CopyCardDataList = new();  // 攻撃力などの変更をキャラごとに設定。バトル終了後、破棄する

    public ReactiveProperty<int> Hp = new();
    public ReactiveProperty<int> Shield = new();
    public ReactiveProperty<int> AttackPower = new();

    public ReactiveProperty<SimpleStateData> Buff = new();
    public ReactiveProperty<SimpleStateData> Debuff = new();

    private int maxHp;

    private OwnerStatus owner;

    private bool replaceBuff = false;  // バフを置き換えるかどうか
    public bool ReplaceBuff => replaceBuff;
    private bool replaceDebuff = false;
    public bool ReplaceDebuff => replaceDebuff;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="defaultHp"></param>
    /// <param name="owner"></param>
    public Character(int defaultHp, OwnerStatus owner)
    {
        Hp.Value = defaultHp;
        maxHp = Hp.Value;
        this.owner = owner;

        CopyCardDataList = new ReactiveCollection<CardData>(DataBaseManager.instance.cardDataSO.cardDataList);  // ReactiveCollectionのコンストラクタを使用して、CopyDataListを置き換え
    }

    /// <summary>
    /// HPの更新
    /// </summary>
    /// <param name="amount"></param>
    public virtual void UpdateHp(int amount)
    {
        Hp.Value = Mathf.Clamp(Hp.Value += amount, 0, maxHp);

        Debug.Log($"{owner}のHP : {Hp.Value}");
    }

    /// <summary>
    /// シールド値の更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateShield(int amount)
    {
        Shield.Value = Mathf.Clamp(Shield.Value += amount, 0, int.MaxValue);

        Debug.Log($"{owner}のシールド値 : {Shield.Value}");
    }

    /// <summary>
    /// バフの継続時間の更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateBuff(SimpleStateData newStateData)
    {
        replaceBuff = Buff.Value != newStateData;

        Buff.Value = newStateData;

        Debug.Log($"{owner}のバフ : {Buff.Value}");
    }

    /// <summary>
    /// デバフの継続時間の更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateDebuff(SimpleStateData newStateData)
    {
        replaceDebuff = Debuff.Value != newStateData;

        Debuff.Value = newStateData;

        Debug.Log($"{owner}のデバフ : {Debuff.Value}");
    }
}
