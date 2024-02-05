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
            GameData.instance.GetOpponent().UpdateHp(-cardData.attackPower);

            // このカードの攻撃力を1増加
            GameData.instance.GetPlayer().CopyCardDataList.Where(data => data == cardData).FirstOrDefault().attackPower++;
        }
        else
        {
            GameData.instance.GetPlayer().UpdateHp(-cardData.attackPower);

            GameData.instance.GetOpponent().CopyCardDataList.Where(data => data == cardData).FirstOrDefault().attackPower++;
        }

        await UniTask.DelayFrame(1);

        Debug.Log(this);
    }
}
