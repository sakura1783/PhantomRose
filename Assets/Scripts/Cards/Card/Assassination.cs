using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Assassination : CardEffectBase
{
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
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        if (owner == OwnerStatus.Player)
        {
            for (int i = 0; i < 2; i++)
            {
                GameData.instance.GetOpponent().UpdateHp(-cardData.attackPower);
                FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.attackPower, -1, OwnerStatus.Opponent);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                GameData.instance.GetPlayer().UpdateHp(-cardData.attackPower);
                FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.attackPower, -1, OwnerStatus.Player);
            }
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
