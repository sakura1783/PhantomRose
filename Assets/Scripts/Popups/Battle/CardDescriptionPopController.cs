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
    public void SetUp(CardData data)
    {
        // 各値を設定
        imgCard.sprite = IconManager.instance.GetCardIcon(data.spriteId);
        txtCardType.text = data.cardType.ToString();
        txtCardLevel.text = $"LV {data.level}";
        txtCardName.text = data.name;
        txtCardDescription.text = data.description;
    }
}
