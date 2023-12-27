using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Create CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public List<CardData> cardDataList = new();

    [System.Serializable]
    public class CardData
    {
        public string name;
        public CardType cardType;

        public int id;

        public int recoveryPower;
        public int attackPower;
        public int shieldPower;

        public int coolTime;

        [Multiline] public string description;

        //バフ、デバフ用のクラスの変数
    }
}
