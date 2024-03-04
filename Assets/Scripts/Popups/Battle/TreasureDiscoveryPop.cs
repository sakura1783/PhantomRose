using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

// このポップアップはShow<>では表示しない(currentPopが置き換わり、GoBack()した時にこのポップアップだけが閉じてSearchEventPopが閉じない)。
public class TreasureDiscoveryPop : PopupBase
{
    [SerializeField] private Image imgTreasure;

    [SerializeField] private Text txtTreasureName;

    [SerializeField] private Button btnGet;

    [SerializeField] private InventoryPop inventoryPop;

    private ItemDataSO.ItemData getItemData;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        btnGet.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // インベントリにアイテムを追加
                inventoryPop.AddBattleItem(getItemData);

                PopupManager.instance.GoBack();
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップを表示
    /// </summary>
    /// <param name="itemData"></param>
    public void ShowTreasureDiscoveryPop(ItemDataSO.ItemData itemData)
    {
        getItemData = itemData;

        // 各値の設定
        imgTreasure.sprite = IconManager.instance.GetItemIcon(itemData.spriteId);
        txtTreasureName.text = itemData.itemName;

        // ポップアップ表示
        canvasGroup.DOFade(1, 0.5f)
            .SetEase(ease)
            .OnComplete(() => canvasGroup.blocksRaycasts = true);
    }
}
