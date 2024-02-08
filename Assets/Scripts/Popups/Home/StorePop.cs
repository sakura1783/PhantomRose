using UnityEngine;
using UnityEngine.UI;

public class StorePop : PopupBase
{
    [SerializeField] private Button btnToCardProduct;
    [SerializeField] private Button btnToSpecialProduct;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        //TODO 各ボタンの設定
    }
}
