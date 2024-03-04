using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public List<ItemData> itemDataList = new();


    [System.Serializable]
    public class ItemData
    {
        public int id;
        public string itemName;

        public int spriteId;

        [Multiline] public string description;
    }
}
