using System.Threading;
using Cysharp.Threading.Tasks;

[System.Serializable]
public abstract class CardBase : ICommand
{
    protected CardData cardData;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public CardBase(CardData cardData)
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
