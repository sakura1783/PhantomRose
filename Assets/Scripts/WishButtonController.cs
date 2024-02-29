using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class WishButtonController : MonoBehaviour
{
    [SerializeField] private Button btnWish;

    [SerializeField] private Text txtWishDetail;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="data"></param>
    public void SetUp(WishDataSO.WishData data)
    {
        txtWishDetail.text = data.wishDetail;

        // ボタンの購読処理
        btnWish.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // TODO 願いの効果を適用する

                PopupManager.instance.GoBack();
            })
            .AddTo(this);
    }
}
