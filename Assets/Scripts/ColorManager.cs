using UnityEngine;

/// <summary>
/// 色の種類
/// </summary>
public enum ColorType
{
    VeryLightRed,
    LightRed,
    Red,
    DarkRed,
    Magenta,
    Cyan,
}

public class ColorManager : AbstractSingleton<ColorManager>
{
    [SerializeField] private ColorDataSO colorDataSO;


    /// <summary>
    /// 16進数のColorの取得
    /// </summary>
    /// <param name="searchColorType"></param>
    /// <returns></returns>
    public Color GetColor(ColorType searchColorType)
    {
        var colorString = colorDataSO.colorDataList.Find(data => data.colorType == searchColorType).hexadecimalColor;
        ColorUtility.TryParseHtmlString(colorString, out Color color);

        return color;
    }
}
