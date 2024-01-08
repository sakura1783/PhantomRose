using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Magic : CardBase
{
    public override async UniTask ExecuteAsync(CancellationToken token)
    {
        Debug.Log("Magic");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public Magic(CardData cardData) : base(cardData) { }
}
