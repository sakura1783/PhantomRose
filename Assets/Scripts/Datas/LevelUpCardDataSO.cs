using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpCardDataSO", menuName = "Create LevelUpCardDataSO")]
public class LevelUpCardDataSO : ScriptableObject
{
    public List<LevelUpCardData> levelUpCardDataList = new();
}
