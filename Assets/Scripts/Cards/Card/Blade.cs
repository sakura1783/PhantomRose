using System.Threading;
using System.Linq;
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
    public override async UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token)
    {
        if (owner == OwnerStatus.Player)
        {
            // 対戦相手のHPを減らす
            GameData.instance.GetOpponent().CalculateDamage(-cardData.attackPower, false);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.attackPower, -1, OwnerStatus.Opponent);

            // このカードの攻撃力を1増加
            GameData.instance.GetPlayer().CopyCardDataList.Where(data => data == cardData).FirstOrDefault().attackPower++;
            FloatingMessageManager.instance.GenerateFloatingMessage(1, 3, OwnerStatus.Player);  // TODO 第二引数、数を使わない
        }
        else
        {
            GameData.instance.GetPlayer().CalculateDamage(-cardData.attackPower, false);
            FloatingMessageManager.instance.GenerateFloatingMessage(-cardData.attackPower, -1, OwnerStatus.Player);

            GameData.instance.GetOpponent().CopyCardDataList.Where(data => data == cardData).FirstOrDefault().attackPower++;
            FloatingMessageManager.instance.GenerateFloatingMessage(1, 3, OwnerStatus.Opponent);
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
