using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryPop : PopupBase
{
    [SerializeField] private BattleItemController battleItemPrefab;

    [SerializeField] private BattleItemSlot BattleItemSlotPrefab;

    [SerializeField] private Transform battleItemPlace;

    [SerializeField] private GameObject exchangeItemPromptObj;

    [SerializeField] private Button btnSkip;

    private List<BattleItemSlot> slots = new();
    private List<BattleItemController> battleItems = new();

    private List<GameObject> temporaryObjects = new();

    private bool isDisplayExchangePop = false;  // 重複してポップアップの切り替えが行われるのを防ぐ

    private ItemDataSO.ItemData newItemData;
    public ItemDataSO.ItemData NewItemData => newItemData;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        // アイテムスロットの生成
        for (int i = 0; i < GameDataManager.instance.gameData.inventoryCapacity; i++)
        {
            var slot = Instantiate(BattleItemSlotPrefab, battleItemPlace);

            slots.Add(slot);
        }

        // ExchangePopを非表示にする
        SwitchDisplayExchangePop(false);
    }

    /// <summary>
    /// インベントリにアイテムを追加
    /// </summary>
    /// <param name="data"></param>
    public void AddBattleItem(ItemDataSO.ItemData data)
    {
        newItemData = data;

        // インベントリのスペースがない場合
        if (GameDataManager.instance.gameData.myItemList.Count >= GameDataManager.instance.gameData.inventoryCapacity)
        {
            // ExchangePopを表示
            SwitchDisplayExchangePop(true);
            
            return;
        }

        // インベントリにアイテムを追加する
        GameDataManager.instance.gameData.myItemList.Add(data);

        var emptySlot = slots.Where(slot => !slot.HasBattleItem).Take(1).FirstOrDefault();
        var item = Instantiate(battleItemPrefab, emptySlot.transform);
        item.SetUp(data, this);
        item.transform.localPosition = new Vector2(-5, 0);
        emptySlot.HasBattleItem = true;

        battleItems.Add(item);
    }

    /// <summary>
    /// アイテム交換用にポップアップを一部変更、または変更したものをもとに戻す
    /// </summary>
    public void SwitchDisplayExchangePop(bool isActive)
    {
        // 表示する
        if (isActive)
        {
            // ボタンを変更
            foreach (var item in battleItems)
            {
                item.BtnExchangeGroup.alpha = 1;
                item.BtnExchangeGroup.blocksRaycasts = true;
            }

            // ScrollViewのContentに要素を追加し、並び替え
            var txtObj = Instantiate(exchangeItemPromptObj, battleItemPlace);
            txtObj.transform.SetAsFirstSibling();  // SetAsFirstSiblingで、対象のTransformを子要素のうち先頭にする
            temporaryObjects.Add(txtObj);

            btnClose = Instantiate(btnSkip, battleItemPlace);
            btnClose.transform.SetAsLastSibling();  // 最後尾にする
            temporaryObjects.Add(btnClose.gameObject);

            // Canvasの表示
            canvasGroup.DOFade(1, 0.5f)
                .SetEase(ease)
                .OnComplete(() => canvasGroup.blocksRaycasts = true);
        }
        // 非表示にする
        else
        {
            // すでにExchangePopが閉じられている場合は処理しない
            if (!isDisplayExchangePop)
            {
                return;
            }

            // ボタンをもとに戻す
            foreach (var item in battleItems)
            {
                item.BtnExchangeGroup.alpha = 0;
                item.BtnExchangeGroup.blocksRaycasts = false;
            }

            // 一時的に生成したゲームオブジェクトを破棄
            foreach(var obj in temporaryObjects)
            {
                Destroy(obj);
            }
        }

        isDisplayExchangePop = isActive;
    }
}
