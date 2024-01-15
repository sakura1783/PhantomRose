using UnityEngine;

/// <summary>
/// アイコンの種類
/// </summary>
public enum EventIconType
{
    BattleEvent,
    SearchEvent,
    Player,
}

public class IconManager : AbstractSingleton<IconManager>
{
    [SerializeField] private IconDataSO iconDataSO;


    /// <summary>
    /// イベント用アイコン画像の取得
    /// </summary>
    /// <param name="searchIconType"></param>
    /// <returns></returns>
    public Sprite GetEventIcon(EventIconType searchIconType)
    {
        return iconDataSO.eventIconDataList.Find(data => data.iconType == searchIconType).iconSprite;
    }

    /// <summary>
    /// カード用アイコン画像の取得
    /// </summary>
    /// <param name="searchIconNo"></param>
    /// <returns></returns>
    public Sprite GetCardIcon(int searchIconNo)
    {
        return iconDataSO.cardIconDataList.Find(data => data.iconId == searchIconNo).iconSprite;
    }
}
