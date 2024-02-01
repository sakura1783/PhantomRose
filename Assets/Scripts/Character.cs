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
    private int maxHp;

    private OwnerStatus owner;

    public ReactiveProperty<int> Hp = new();
    public ReactiveProperty<int> Shield = new();
    public ReactiveProperty<int> AttackPower = new();

    public ReactiveProperty<int> BuffDuration = new();
    public ReactiveProperty<int> DebuffDuration = new();


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
    }

    /// <summary>
    /// HP取得用プロパティ
    /// </summary>
    /// <returns></returns>
    public int GetHp
    {
        get => Hp.Value;
        set => Hp.Value = value;
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
    public void UpdateBuffDuration(int amount)
    {
        BuffDuration.Value = Mathf.Clamp(BuffDuration.Value += amount, 0, int.MaxValue);
        Debug.Log($"{owner}のバフ値 : {Shield.Value}");
    }

    /// <summary>
    /// デバフの継続時間の更新
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateDebuffDuration(int amount)
    {
        DebuffDuration.Value = Mathf.Clamp(DebuffDuration.Value, 0, int.MaxValue);
        Debug.Log($"{owner}のデバフ値 : {Shield.Value}");
    }
}
