using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClearRewardController : MonoBehaviour
{
    [SerializeField] private Image imgGem;

    [SerializeField] private Text txtGemCount;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="gemType"></param>
    /// <param name="gemCount"></param>
    public void SetUp(GemType gemType, int gemCount)
    {
        imgGem.sprite = IconManager.instance.GetGemSprite(gemType);
        txtGemCount.text = gemCount.ToString();
    }
}
