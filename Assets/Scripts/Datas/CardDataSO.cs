using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードの種類
/// </summary>
public enum CardType
{
    Attack,
    Magic,
}

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Create CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public List<CardData> cardDataList = new();
}
