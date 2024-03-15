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
    private List<CardData> handCardList = new();  // CardDataSOのcardDataListの情報をディープコピーした、DataBaseManagerのcopyCardDataListの情報を使って値を代入する
    public List<CardData> HandCardList
    {
        get => handCardList;
        set => handCardList = value;
    }

    public ReactiveProperty<int> Hp = new();
    public ReactiveProperty<int> Shield = new();
    public ReactiveProperty<int> AttackPower = new();

    public ReactiveProperty<SimpleStateData> Buff = new();
    public ReactiveProperty<SimpleStateData> Debuff = new();
    public ReactiveProperty<int> BuffDuration = new();
    public ReactiveProperty<int> DebuffDuration = new();

    private int maxHp;
    public int MaxHp => maxHp;

    private OwnerStatus owner;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="defaultHp"></param>
    /// <param name="owner"></param>
    /// <param name="handCards">キャラの手札のカード</param>
    public Character(OwnerStatus owner, int maxHp) //List<CardData> handCards
    {
        this.owner = owner;
        this.maxHp = maxHp;
        Hp.Value = maxHp;

        //copyCardDataList = new List<CardData>(handCards);
    }

    /// <summary>
    /// HPの更新
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isDirect">シールドを無視して、直接HPに影響を与えるかどうか</param>
    public virtual void UpdateHp(int value, bool isDirect)
    {
        // 最終的にHP追加する量
        int amount;

        // TODO 「集中」バフを持っている場合、isDirectをtrueにする

        if (isDirect)
        {
            amount = value;
        }
        else
        {
            // ダメージがシールド値よりも大きい場合
            if (-value > Shield.Value)
            {
                amount = -(-value - Shield.Value);
                Shield.Value = 0;
            }
            else
            {
                amount = 0;
                UpdateShield(value);
            }
        }

        Hp.Value = Mathf.Clamp(Hp.Value += amount, 0, maxHp);
    }

    /// <summary>
    /// シールド値の更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateShield(int amount)
    {
        Shield.Value = Mathf.Clamp(Shield.Value += amount, 0, int.MaxValue);
    }

    /// <summary>
    /// バフ更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateBuff(SimpleStateData newStateData)
    {
        // バフを置き換えるかどうか
        var replaceBuff = Buff.Value != newStateData;

        //置き換える場合
        if (replaceBuff)
        {
            Buff.Value = newStateData;

            //継続時間を置き換え
            BuffDuration.Value = newStateData.duration;
        }
        else
        {
            // 継続時間を追加
            BuffDuration.Value += newStateData.duration;
        }
    }

    /// <summary>
    /// デバフ更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateDebuff(SimpleStateData newStateData)
    {
        var replaceDebuff = Debuff.Value != newStateData;

        if (replaceDebuff)
        {
            Debuff.Value = newStateData;
            DebuffDuration.Value = newStateData.duration;
        }
        else
        {
            DebuffDuration.Value += newStateData.duration;
        }
    }
}
