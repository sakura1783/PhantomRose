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
        if (owner == OwnerStatus.Player)
        {
            GameData.instance.GetOpponent().CalculateDamage(-cardData.AttackPower.Value, false);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, OwnerStatus.Opponent);

            // 相手にデバフを付与
            GameData.instance.GetOpponent().UpdateDebuff(cardData.stateList[0]);
            FloatingMessageManager.instance.GenerateFloatingMessage(cardData.stateList[0].duration, DataBaseManager.instance.stateDataSO.stateDataList[cardData.stateList[0].stateId].spriteId, OwnerStatus.Opponent);
        }
        else
        {
            GameData.instance.GetPlayer().CalculateDamage(-cardData.AttackPower.Value, false);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.AttackPower.Value, -1, OwnerStatus.Player);

            GameData.instance.GetPlayer().UpdateDebuff(cardData.stateList[0]);
            FloatingMessageManager.instance.GenerateFloatingMessage(cardData.stateList[0].duration, DataBaseManager.instance.stateDataSO.stateDataList[cardData.stateList[0].stateId].spriteId, OwnerStatus.Player);

        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
