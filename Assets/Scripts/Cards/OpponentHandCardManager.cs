using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OpponentHandCardManager : HandCardManagerBase
{
    private CardSlotManager cardSlotManager;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="newCardList"></param>
    /// <param name="cardSlotManager"></param>
    public OpponentHandCardManager(List<CardController> newCardList, CardSlotManager cardSlotManager) : base(newCardList)
    {
        Debug.Log($"mewCardList : {newCardList.Count}");

        if (cardSlotManager != null)
        {
            this.cardSlotManager = cardSlotManager;
        }
    }

    //TODO 対戦相手のカードをセットする処理

    /// <summary>
    /// 対戦相手用のスロットにランダムなカードを配置
    /// </summary>
    public void SetSlotsRandomCard()
    {
        // カードスロットの中から対戦相手用のスロットのみを抽出(2つ取得できる)
        List<CardSlot> slots = cardSlotManager.CardSlotList.Where(slot => slot.owner == OwnerStatus.Opponent).ToList();

        // 対戦相手が所持しているカードの中からランダムなカードを2枚抽出
        List<CardController> tempCardList = cardList.OrderBy(_ => Random.value).Take(slots.Count).ToList();

        // カードスロットにカードを配置
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetCard(tempCardList[i]);
        }
    }
}
