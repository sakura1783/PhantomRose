using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WishDataSO", menuName = "Create WishDataSO")]
public class WishDataSO : ScriptableObject
{
    public List<WishData> wishDataList = new();


    [System.Serializable]
    public class WishData
    {
        public int id;

        [Multiline] public string wishDetail;
    }
}
