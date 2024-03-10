using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Blade : CardEffectBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public Blade(CardData cardData) : base(cardData) { }

    /// <summary>
    /// カードの効果
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token, int slotNo)
    {
        // 攻撃
        AllCardEffectManager.OneAttack(owner, -cardData.AttackPower.Value);
        FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, GameDataManager.instance.gameData.GetTarget(owner));

        // 攻撃力アップ
        AllCardEffectManager.UpdateAttackPower(owner, cardData);
        FloatingMessageManager.instance.GenerateFloatingMessage(1, 3, owner);

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
