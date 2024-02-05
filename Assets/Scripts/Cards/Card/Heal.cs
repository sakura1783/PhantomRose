using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Heal : CardEffectBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public Heal(CardData cardData) : base(cardData) { }

    /// <summary>
    /// カード処理
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetPlayer().UpdateHp(cardData.recoveryPower);
        }
        else
        {
            GameData.instance.GetOpponent().UpdateHp(cardData.recoveryPower);
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
