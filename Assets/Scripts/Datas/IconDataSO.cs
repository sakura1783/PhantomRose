using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconDataSO", menuName = "Create IconDataSO")]
public class IconDataSO : ScriptableObject
{
    public List<EventIconData> eventIconDataList = new();
    public List<CardIconData> cardIconDataList = new();

    [System.Serializable]
    public class EventIconData
    {
        public EventIconType iconType;
        public Sprite iconSprite;
    }

    [System.Serializable]
    public class CardIconData
    {
        public int iconId;
        public Sprite iconSprite;
    }
}