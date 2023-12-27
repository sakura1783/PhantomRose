using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ルート分岐用のマス
/// </summary>
public class RouteDetail : MonoBehaviour
{
    [SerializeField] private Image imgRoute;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="iconSprite"></param>
    public void SetUp(Sprite iconSprite)
    {
        imgRoute.sprite = iconSprite;
    }

    /// <summary>
    /// マスの色を変更
    /// </summary>
    /// <param name="newColor"></param>
    public void ChangeRouteColor(Color newColor)
    {
        imgRoute.color = newColor;
    }
}
