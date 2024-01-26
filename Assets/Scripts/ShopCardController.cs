using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShopCardController : MonoBehaviour
{
    [SerializeField] private Button btnShopCard;

    [SerializeField] private Text txtCardName;
    [SerializeField] private Text txtPrice;

    [SerializeField] private Transform cardTran;

    [SerializeField] private CardController cardPrefab;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="data"></param>
    public void SetUp(CardData data, ShopEventPop shopEventPop)
    {
        txtCardName.text = data.name;
        txtPrice.text = data.price.ToString();

        var card = Instantiate(cardPrefab, cardTran);

        // 生成したカードの各設定
        card.SetInactive();
        card.transform.rotation = Quaternion.Euler(0, 0, 0);
        card.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        card.SetUp(data);

        btnShopCard.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => shopEventPop.SwitchCardPurchasePopVisibility(true, data))
            .AddTo(this);
    }
}
