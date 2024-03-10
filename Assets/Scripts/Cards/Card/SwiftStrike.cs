using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SwiftStrike : CardEffectBase
{
    public SwiftStrike(CardData cardData) : base(cardData) { }

    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token, int slotNo)
    {
        // 攻撃。フィールド最初の2枚のいずれかである場合、攻撃力9で攻撃
        if (slotNo <= 1)
        {
            AllCardEffectManager.OneAttack(owner, -9);
            FloatingMessageManager.instance.GenerateFloatingMessage(-9, -1, GameData.instance.GetTarget(owner));
        }
        else
        {
            AllCardEffectManager.OneAttack(owner, -cardData.AttackPower.Value);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, GameData.instance.GetTarget(owner));
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
