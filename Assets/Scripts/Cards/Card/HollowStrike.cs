using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HollowStrike : CardEffectBase
{
    /// <summary>
    /// コンストラクタ。呼び出された際に、引数でCardDataを受け取り、親のCardEffectBaseのコンストラクタを呼び出し、受け取ったCardDataを渡す
    /// </summary>
    /// <param name="cardData"></param>
    public HollowStrike(CardData cardData) : base(cardData) { }

    /// <summary>
    /// カードの効果
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetOpponent().UpdateHp(-cardData.attackPower);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.attackPower, -1, OwnerStatus.Opponent);
        }
        else
        {
            GameData.instance.GetPlayer().UpdateHp(-cardData.attackPower);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.attackPower, -1, OwnerStatus.Player);
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
