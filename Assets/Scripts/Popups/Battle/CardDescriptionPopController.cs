using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardDescriptionPopController : MonoBehaviour
{
    [SerializeField] private Image imgCard;

    [SerializeField] private Text txtCardType;
    [SerializeField] private Text txtCardLevel;
    [SerializeField] private Text txtCardName;
    [SerializeField] private Text txtCardDescription;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="data"></param>
    public void SetUp(CardData data, bool isEnhanced)
    {
        // 各値を設定
        imgCard.sprite = IconManager.instance.GetCardIcon(data.spriteId);
        txtCardType.text = data.cardType.ToString();
        txtCardLevel.text = $"LV {data.level}";
        txtCardName.text = isEnhanced ? $"{data.name}+" : data.name;
        txtCardDescription.text = isEnhanced ? DataBaseManager.instance.levelUpCardDataSO.levelUpCardDataList.Where(levelUpData => levelUpData.cardId == data.id).FirstOrDefault().description : data.description;
    }
}
