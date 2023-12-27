using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconDataSO", menuName = "Create IconDataSO")]
public class IconDataSO : ScriptableObject
{
    public List<IconData> iconDataList = new();

    [System.Serializable]
    public class IconData
    {
        public IconType iconType;
        public Sprite iconSprite;
    }
}
