using System.Collections.Generic;
using System.Linq;
using UniRx;

public class CardSlotManager
{
    public List<CardSlot> CardSlotList { get; private set; }  // newで初期化もできる

    private CompositeDisposable disposables = new();  // CompositeDisposableを初期化

    // デバッグ用
    private OwnerStatus[] slotStatus = new OwnerStatus[4]
    {
        OwnerStatus.Player, OwnerStatus.Player, OwnerStatus.Opponent, OwnerStatus.Opponent
    };

    public List<CardController> setPlayerCardList = new();  // スロットにセットしたプレイヤーのカード群


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="battleEventManager"></param>
    /// <param name="newCardSlotList"></param>
    public CardSlotManager(BattleEventManager battleEventManager, List<CardSlot> newCardSlotList)
    {
        CardSlotList = new(newCardSlotList);

        foreach (var cardSlot in CardSlotList)
        {
            cardSlot.OnClickAsObservable
                .Where(_ => battleEventManager.selectedCard != null)
                .Where(_ => cardSlot.owner == OwnerStatus.Player)
                .Where(_ => cardSlot.cardController == null)
                .Subscribe(_ =>
                {
                    // スロットにカードを配置
                    OnClickCardSlot(cardSlot, battleEventManager.selectedCard);

                    setPlayerCardList.Add(battleEventManager.selectedCard);

                    // 選択していたカードを解除
                    battleEventManager.RemoveCard();

                    // 全てのスロットにカードがセットされているか確認
                    if (CardSlotList.All(slot => slot.cardController != null))
                    {
                        battleEventManager.OnCardSet();
                    }
                })
                .AddTo(disposables);
        }
    }

    /// <summary>
    /// スロットにカードを配置
    /// </summary>
    /// <param name="cardSlot"></param>
    /// <param name="cardController"></param>
    private void OnClickCardSlot(CardSlot cardSlot, CardController selectedCard)
    {
        cardSlot.SetCard(selectedCard);
    }

    /// <summary>
    /// 不必要なリソースをまとめて解放
    /// </summary>
    public void Dispose()
    {
        disposables.Dispose();
    }

    /// <summary>
    /// カードスロットのランダムな2箇所を対戦相手用のスロットに設定
    /// </summary>
    public void SetOpponentCardSlotsRandomly()
    {
        slotStatus = slotStatus.OrderBy(_ => UnityEngine.Random.value).ToArray();

        for (int i = 0; i < slotStatus.Length; i++)
        {
            CardSlotList[i].ChangeStatus(slotStatus[i]);
        }
    }

    /// <summary>
    /// スロットにセットされているカードのうち、指定したOwnerStatusのカードを破棄
    /// </summary>
    /// <param name="target"></param>
    public void DeleteCardsFromSlots(OwnerStatus target)
    {
        foreach (var slot in CardSlotList)
        {
            if (slot.owner == target && slot.cardController != null)
            {
                slot.DeleteCard();
            }
        }

        // 上記をLinqで
        //CardSlotList
        //    .Where(slot => slot.owner == target && slot.cardController != null)
        //    .ToList()
        //    .ForEach(slot => slot.DeleteCard());

        if (target == OwnerStatus.Player)
        {
            setPlayerCardList.Clear();
        }
    }

    /// <summary>
    /// スロットにセットされている全てのカードを破棄
    /// </summary>
    public void DeleteAllCardsFromSlots()
    {
        foreach (var slot in CardSlotList)
        {
            if (slot.cardController != null)
            {
                slot.DeleteCard();
            }
        }

        // 上記をLinqで
        //CardSlotList
        //    .Where(slot => slot.cardController != null)
        //    .ToList()
        //    .ForEach(slot => slot.DeleteCard());

        setPlayerCardList.Clear();
    }
}
