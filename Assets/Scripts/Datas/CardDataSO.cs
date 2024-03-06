using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// カードの種類
/// </summary>
public enum CardType
{
    攻撃,
    魔法,
}

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Create CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public List<CardData> cardDataList = new();


    /// <summary>
    /// CardDataSOの情報をディープコピー
    /// </summary>
    /// <returns></returns>
    public List<CardData> GetCopyCardDataList()
    {
        // 深いコピーを行い、新しいリストを返す
        return cardDataList.Select(cardData => cardData.Clone()).ToList();
    }
}
