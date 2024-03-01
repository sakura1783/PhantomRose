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
    public Character(int defaultHp, OwnerStatus owner)
    {
        Hp.Value = defaultHp;
        maxHp = Hp.Value;
        this.owner = owner;

        CopyCardDataList = new ReactiveCollection<CardData>(GameData.instance.myCardList);  // ReactiveCollectionのコンストラクタを使用して、CopyDataListを置き換え
    }

    /// <summary>
    /// HPの更新
    /// </summary>
    /// <param name="amount"></param>
    public virtual void UpdateHp(int amount)
    {
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
