using UnityEngine;

/// <summary>
/// アイコンの種類
/// </summary>
public enum IconType
{
    BattleEvent,
    SearchEvent,
    Player,
}

public class IconManager : MonoBehaviour
{
    public static IconManager instance;

    [SerializeField] private IconDataSO iconDataSO;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// アイコン用画像の取得
    /// </summary>
    /// <param name="searchIconType"></param>
    /// <returns></returns>
    public Sprite GetIcon(IconType searchIconType)
    {
        return iconDataSO.iconDataList.Find(data => data.iconType == searchIconType).iconSprite;
    }
}
