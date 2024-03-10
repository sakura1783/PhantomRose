using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BattleItemController : MonoBehaviour
{
    [SerializeField] private Image imgItem;

    [SerializeField] private Text txtItemName;
    [SerializeField] private Text txtDescription;

    [SerializeField] private Button btnUse;
    [SerializeField] private Button btnExchange;

    [SerializeField] private CanvasGroup btnExchangeGroup;
    public CanvasGroup BtnExchangeGroup
    {
        get => btnExchangeGroup;
        set => btnExchangeGroup = value;
    }

    private InventoryPop inventoryPop;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="data"></param>
    public void SetUp(ItemDataSO.ItemData data, InventoryPop inventoryPop)
    {
        this.inventoryPop = inventoryPop;

        // 各値の設定
        imgItem.sprite = IconManager.instance.GetItemIcon(data.spriteId);
        txtItemName.text = data.itemName;
        txtDescription.text = data.description;

        // ボタンの購読処理
        btnUse.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // TODO アイテムの効果を発動し、アイテムを破棄

                Debug.Log($"アイテム：{data.itemName}を使用しました");
            })
            .AddTo(this);

        btnExchange.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // アイテムを交換
                ExchangeBattleItem(data, inventoryPop.NewItemData);

                inventoryPop.HidePopUp();
            })
            .AddTo(this);
    }

    /// <summary>
    /// アイテムを交換する
    /// </summary>
    /// <param name="thisData">このゲームオブジェクトのアイテムデータ</param>
    /// <param name="newItemData"></param>
    public void ExchangeBattleItem(ItemDataSO.ItemData thisItemData, ItemDataSO.ItemData newItemData)
    {
        // ポップアップをもとに戻す
        inventoryPop.SwitchDisplayExchangePop(false);

        // GameDataのListからこのアイテムを削除(ここではListに新しい要素は追加しない。追加してしまうと、AddBattleItemメソッドのreturnにより処理が行われない)
        GameDataManager.instance.gameData.myItemList.Remove(thisItemData);

        // 親スロットのHasBattleItemを変更する
        if (transform.parent.TryGetComponent(out BattleItemSlot slot))
        {
            slot.HasBattleItem = false;
        }

        // 新しいアイテムをインベントリに生成
        inventoryPop.AddBattleItem(newItemData);

        // このゲームオブジェクトを破棄
        Destroy(gameObject);
    }
}
