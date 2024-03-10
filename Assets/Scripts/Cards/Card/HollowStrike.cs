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
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token, int slotNo)
    {
        //if (owner == OwnerStatus.Player)
        //{
        //    GameData.instance.GetOpponent().UpdateHp(-cardData.AttackPower.Value, false);
        //    FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, OwnerStatus.Opponent);
        //}
        //else
        //{
        //    GameData.instance.GetPlayer().UpdateHp(-cardData.AttackPower.Value, false);
        //    FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, OwnerStatus.Player);
        //}

        // AllCardEffectManagerを利用して、上記をリファクタリング
        // 攻撃
        AllCardEffectManager.OneAttack(owner, -cardData.AttackPower.Value);
        FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, GameDataManager.instance.gameData.GetTarget(owner));

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
