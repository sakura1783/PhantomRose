using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Create CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public List<CardData> cardDataList = new();
}
