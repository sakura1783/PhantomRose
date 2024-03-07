using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Slash : CardEffectBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public Slash(CardData cardData) : base(cardData) { }

    /// <summary>
    /// カード処理
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        // 攻撃
        AllCardEffectManager.OneAttack(owner, -cardData.AttackPower.Value);
        FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, GameData.instance.GetTarget(owner));

        // 相手にデバフ付与
        AllCardEffectManager.AddDebuff(owner, cardData.stateList[0]);  // TODO 数字を使わないように、リファクタリング。debuffとbuffの情報を分けて保持した方がいいかも
        FloatingMessageManager.instance.GenerateFloatingMessage(cardData.stateList[0].duration, DataBaseManager.instance.stateDataSO.stateDataList[cardData.stateList[0].stateId].spriteId, GameData.instance.GetTarget(owner));

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
