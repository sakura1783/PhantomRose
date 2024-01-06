using UnityEngine;

[System.Serializable]
public class CardData
{
    public int id;

    public string name;
    public CardType cardType;

    public int attackPower;
    public int shieldPower;
    public int recoveryPower;

    public int coolTime;

    [Multiline] public string description;

    public int spriteId;

    // バフ、デバフ用のクラスの変数
}
