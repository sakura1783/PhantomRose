using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Assassination : CardEffectBase
{
    private int attackCount = 2;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="cardData"></param>
    public Assassination(CardData cardData) : base(cardData) { }

    /// <summary>
    /// カード処理
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token, int slotNo)
    {
        // 指定回数(attackCount)だけ攻撃
        for (int i = 0; i < attackCount; i++)
        {
            AllCardEffectManager.OneAttack(owner, -cardData.AttackPower.Value);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, GameDataManager.instance.gameData.GetTarget(owner));
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
