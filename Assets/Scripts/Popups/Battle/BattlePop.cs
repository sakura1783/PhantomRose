using UnityEngine;

public class BattlePop : PopupBase
{
    [SerializeField] private CardController cardPrefab;

    //TODO 変数追加


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
