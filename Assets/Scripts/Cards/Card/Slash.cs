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
            GameData.instance.GetOpponent().UpdateHp(-cardData.attackPower);

            // 相手にデバフを付与
            GameData.instance.GetOpponent().UpdateDebuff(cardData.stateList[0]);
        }
        else
        {
            GameData.instance.GetPlayer().UpdateHp(-cardData.attackPower);

            GameData.instance.GetPlayer().UpdateDebuff(cardData.stateList[0]);
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
