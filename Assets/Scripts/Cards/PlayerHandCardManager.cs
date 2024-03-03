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
        foreach (var card in cardList.Intersect(setCardList))  // Intersectメソッドは2つのリストの共通の要素を返す。そのため、cardListとsetCardListの共通の要素だけが処理の対象となる。
        {
            card.IsSelectable.Value = false;
        }

        // 更にLinqで書いた場合だが、これは通常のbool(値型)なら動くが、今回はReactiveProperty(参照型)なので、正常に動かない
        //cardList.Intersect(setCardList).Select(card => card.IsSelectable.Value = false);
    }

    /// <summary>
    /// カードを選択不可にする
    /// </summary>
    /// <param name="card"></param>
    public void InactivateCard(CardController card)
    {
        cardList.Find(x => card).SetSelectable(true);
    }

    /// <summary>
    /// 今回利用したカードにクールタイムを設定
    /// </summary>
    /// <param name="setCardList"></param>
    public void SetCoolTimeCards(List<CardController> setCardList)
    {
        foreach (CardController card in cardList.Intersect(setCardList))
        {
            card.SetCoolTime(card.CardData.coolTime);
        }
    }

    /// <summary>
    /// カードのクールタイムを更新
    /// </summary>
    /// <param name="setCardList"></param>
    public void UpdateCoolTimeCards(List<CardController> setCardList)
    {
        foreach (CardController card in cardList.Except(setCardList))  // 共通していないものを取り出す
        {
            card.UpdateCoolTime();
        }
    }
}
