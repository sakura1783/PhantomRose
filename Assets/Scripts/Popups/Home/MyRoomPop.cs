using UnityEngine;
using UnityEngine.UI;

public class MyRoomPop : PopupBase
{
    [SerializeField] private Button btnUpgrade;
    [SerializeField] private Button btnCard;
    [SerializeField] private Button btnTask;

    //TODO デイリーミッション用の変数を追加する


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
