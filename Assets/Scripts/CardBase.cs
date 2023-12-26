using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

[System.Serializable]
public abstract class CardBase : ICommand
{
    protected CardDataSO.CardData cardData;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public CardBase(CardDataSO.CardData cardData)
    {
        this.cardData = cardData;
    }

    public abstract UniTask ExecuteAsync(CancellationToken token);

    //public int GetId()
    //{
    //    return cardData.id;
    //}
    public int GetId() => cardData.id;  //上記を省略
}
