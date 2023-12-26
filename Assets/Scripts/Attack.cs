using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Attack : CardBase
{
    public override async UniTask ExecuteAsync(CancellationToken token)
    {
        //仮の待機処理
        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));

        Debug.Log("Attack");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public Attack(CardDataSO.CardData cardData) : base(cardData)
    {
        //中身はなしでいい。その場合、親クラスでの処理が行われる
    }
}
