using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;

public class GameData : AbstractSingleton<GameData>
{
    public List<CardData> myCardList = new();

    public List<CardData> attackCardList = new();
    public List<CardData> magicCardList = new();

    public int inventoryCapacity;
    public List<ItemData> myItemList = new();

    public List<int> achievedChallengeTaskList = new();

    // TODO バトルのデータ。敵の種類、フェードカウント、置いたカード情報、キャラのHP、バフデバフなど

    public ReactiveProperty<int> RubyCount = new();

    public ReactiveProperty<int> PurpleGemCount = new();
    public ReactiveProperty<int> GoldGemCount = new();
    public ReactiveProperty<int> DiamondGemCount = new();
    public ReactiveProperty<int> RubyGemCount = new();

    public ReactiveProperty<bool> HasSaveData = new();  // セーブデータがあるかどうか。ある場合、その情報を使って中断した部分から再開する。

    private Character player;
    private Character opponent;


    /// <summary>
    /// キャラクターの生成
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="hp"></param>
    /// <param name="handCards"></param>
    public void InitCharacter(OwnerStatus owner, int hp, List<CardData> handCards)
    {
        // キャラクターを生成し、HPの設定を行う
        if (owner == OwnerStatus.Player)
        {
            player = new(owner, hp);
        }
        else
        {
            opponent = new(owner, hp);
        }

        // キャラの手札をセット
        SetCardData(owner, handCards);
    }

    /// <summary>
    /// プレイヤーの取得
    /// </summary>
    /// <returns></returns>
    public Character GetPlayer() => player;  // 戻り値があり、メソッド内の処理が一文で済む場合に使える省略記法。

    /// <summary>
    /// 対戦相手(敵)の取得
    /// </summary>
    /// <returns></returns>
    public Character GetOpponent() => opponent;

    //TODO カード管理用の処理を追加する

    /// <summary>
    /// キャラクターの手札をセット
    /// </summary>
    private void SetCardData(OwnerStatus owner, List<CardData> cards)
    {
        if (owner == OwnerStatus.Player)
        {
            // プレイヤーの初期カードを設定
            myCardList.AddRange(cards);
            player.CopyCardDataList = new(myCardList);
        }
        else
        {
            // 敵の手札のカードを設定
            opponent.CopyCardDataList = new(cards);
        }
    }

    /// <summary>
    /// 攻撃カードと魔法カードに分ける
    /// </summary>
    public void SortBattleCardList()
    {
        attackCardList = myCardList.Where(card => card.cardType == CardType.攻撃).ToList();

        magicCardList = myCardList.Where(card => card.cardType == CardType.魔法).ToList();
    }

    /// <summary>
    /// カード取得
    /// </summary>
    /// <param name="cardId"></param>
    /// <returns></returns>
    public CardData GetCardData(int cardId)
    {
        return myCardList.Find(card => card.id == cardId);
    }

    /// <summary>
    /// ジェムの更新
    /// </summary>
    /// <param name="data"></param>
    public void UpdateGemCount(GemType gemType, int value)
    {
        switch (gemType)
        {
            case GemType.Purple:
                PurpleGemCount.Value += value;
                break;

            case GemType.Gold:
                GoldGemCount.Value += value;
                break;

            case GemType.Diamond:
                DiamondGemCount.Value += value;
                break;

            case GemType.Ruby:
                RubyGemCount.Value += value;
                break;

            default:
                Debug.Log("該当のGemTypeがありません");
                break;
        }
    }
}
