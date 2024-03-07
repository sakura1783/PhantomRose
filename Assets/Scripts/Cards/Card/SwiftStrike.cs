using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SwiftStrike : CardEffectBase
{
    public SwiftStrike(CardData cardData) : base(cardData) { }

    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        // TODO 攻撃。フィールド最初の2枚のいずれかである場合、攻撃力9で攻撃
        AllCardEffectManager.OneAttack(owner, -cardData.AttackPower.Value);
        FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, GameData.instance.GetTarget(owner));

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
