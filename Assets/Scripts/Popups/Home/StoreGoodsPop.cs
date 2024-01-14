using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class StoreGoodsPop : PopupBase
{
    [SerializeField] private Button btnBuy;

    [SerializeField] private Text txtGoodsType;
    [SerializeField] private Text txtGoodsName;
    [SerializeField] private Text txtGoodsDescription;
    [SerializeField] private Text txtSpendDiamonds;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        btnBuy.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2))
            .Subscribe(_ => BuyStoreGoods())
            .AddTo(this);
    }

    //TODO ダイヤが足りていない場合、btnBuyを非アクティブにする

    /// <summary>
    /// ストアの商品を買う
    /// </summary>
    private void BuyStoreGoods()
    {
        // ダイヤを消費

        // カードデッキに追加
    }
}
