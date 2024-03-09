using System.Collections.Generic;

[System.Serializable]
public class HandCardManagerBase
{
    public List<CardController> cardList;  // 手札のリスト


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="newCardList"></param>
    public HandCardManagerBase(List<CardController> newCardList)
    {
        cardList = new(newCardList);
    }

    /// <summary>
    /// 手札を全て破棄
    /// </summary>
    public virtual void DestroyHandCards()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            UnityEngine.MonoBehaviour.Destroy(cardList[i].gameObject);
        }

        //cardList.Clear();
    }

    /// <summary>
    /// 今回利用したカードにクールタイムを設定
    /// </summary>
    /// <param name="setCardList"></param>
    public void SetCoolTimeCard(CardController card)
    {
        card.SetCoolTime(card.CardData.coolTime);
    }

    /// <summary>
    /// カードのクールタイムを更新
    /// </summary>
    /// <param name="setCardList"></param>
    public void UpdateCoolTimeCard(CardController card)
    {
        card.UpdateCoolTime();
    }
}
