using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Shield : CardEffectBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public Shield(CardData cardData) : base(cardData) { }

    /// <summary>
    /// カード処理
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        // シールド追加
        AllCardEffectManager.AddShield(owner, cardData.shieldPower);
        FloatingMessageManager.instance.GenerateFloatingMessage(cardData.shieldPower, 102, owner);

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
