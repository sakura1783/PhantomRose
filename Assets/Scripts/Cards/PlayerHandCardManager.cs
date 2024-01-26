using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.Events;

public class PlayerHandCardManager : HandCardManagerBase
{
    /// <summary>
    /// コンストラクタ。baseでCardListを初期化済み
    /// </summary>
    /// <param name="newCardList"></param>
    /// <param name="selectCardAction"></param>
    public PlayerHandCardManager(List<CardController> newCardList, UnityAction<CardController> selectCardAction) : base(newCardList)  // HandCardManagerBaseのコンストラクタにnewCardListを渡し、初期化
    {
        foreach (var card in cardList)
        {
            card.OnClickAsObservable
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.3f))
                .Where(_ => !card.IsSelectable.Value)
                .Subscribe(_ => selectCardAction?.Invoke(card))  // (変数?)でnullチェック、(.メソッド名など)でnullでない時の処理
                .AddTo(card);
        }
    }

    /// <summary>
    /// スロットに配置したカードを再度選択可能にする
    /// </summary>
    /// <param name="setCardList"></param>
    public void ActivateSelectedCards(List<CardController> setCardList)
    {
        //foreach (var card in cardList)
        //{
        //    for (int i = 0; i < setCardList.Count; i++)
        //    {
        //        if (card == setCardList[i])
        //        {
        //            card.IsSelectable.Value = false;
        //        }
        //    }
        //}

        // 上記をLinqで

    }
}
