using System.Linq;

public static class AllCardEffectManager
{
    /// <summary>
    /// 1回攻撃
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="attackPower"></param>
    public static void OneAttack(OwnerStatus owner, int attackPower)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetOpponent().UpdateHp(attackPower, false);
        }
        else
        {
            GameData.instance.GetPlayer().UpdateHp(attackPower, false);
        }
    }

    /// <summary>
    /// カードの攻撃力を増加
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="cardData"></param>
    public static void UpdateAttackPower(OwnerStatus owner, CardData cardData)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetPlayer().HandCardList.Where(data => data == cardData).FirstOrDefault().AttackPower.Value++;
        }
        else
        {
            GameData.instance.GetOpponent().HandCardList.Where(data => data == cardData).FirstOrDefault().AttackPower.Value++;
        }
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="recoveryPower"></param>
    public static void HealHp(OwnerStatus owner, int recoveryPower)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetPlayer().UpdateHp(recoveryPower, true);
        }
        else
        {
            GameData.instance.GetOpponent().UpdateHp(recoveryPower, true);
        }
    }

    /// <summary>
    /// シールド加算
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="shieldPower"></param>
    public static void AddShield(OwnerStatus owner, int shieldPower)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetPlayer().UpdateShield(shieldPower);
        }
        else
        {
            GameData.instance.GetOpponent().UpdateShield(shieldPower);
        }
    }

    /// <summary>
    /// バフ追加
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="buff"></param>
    public static void AddBuff(OwnerStatus owner, SimpleStateData buff)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetPlayer().UpdateBuff(buff);
        }
        else
        {
            GameData.instance.GetOpponent().UpdateBuff(buff);
        }
    }

    /// <summary>
    /// デバフ付与
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="debuff"></param>
    public static void AddDebuff(OwnerStatus owner, SimpleStateData debuff)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetOpponent().UpdateDebuff(debuff);
        }
        else
        {
            GameData.instance.GetPlayer().UpdateDebuff(debuff);
        }
    }
}
