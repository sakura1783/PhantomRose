using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// カード効果用の抽象化された親クラス
/// </summary>
[System.Serializable]
public abstract class CardEffectBase : ICommand
{
    protected CardData cardData;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public CardEffectBase(CardData cardData)
    {
        this.cardData = cardData;
    }

    /// <summary>
    /// カードの効果。子クラスで実装して振る舞いを変える
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public abstract UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token);

    public int GetId() => cardData.id;  // 戻り値があるメソッドの省略記法
}
