using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 宝石の種類
/// </summary>
public enum GemType
{
    Purple,
    Gold,
    Diamond,
    Ruby,
}

[CreateAssetMenu(fileName = "UpgradeDataSO", menuName = "Create UpgradeDataSO")]
public class UpgradeDataSO : ScriptableObject
{
    public List<UpgradeData> upgradeDataList = new();


    [System.Serializable]
    public class UpgradeData
    {
        public int id;

        [Multiline] public string detail;
        public int cost;

        public GemType gemType;
    }
}
