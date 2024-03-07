using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ThornShield : CardEffectBase
{
    public ThornShield(CardData cardData) : base(cardData) { }

    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        // シールド追加
        AllCardEffectManager.AddShield(owner, cardData.shieldPower);
        FloatingMessageManager.instance.GenerateFloatingMessage(cardData.shieldPower, 102, owner);

        // 自身にバフを付与
        AllCardEffectManager.AddBuff(owner, cardData.stateList[0]);
        FloatingMessageManager.instance.GenerateFloatingMessage(cardData.stateList[0].duration, DataBaseManager.instance.stateDataSO.stateDataList[cardData.stateList[0].stateId].spriteId, owner);

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
