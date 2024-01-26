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

        cardList.Clear();
    }
}
