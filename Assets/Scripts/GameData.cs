using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public List<CardData> myCardList = new();

    public List<CardData> attackCardList = new();
    public List<CardData> magicCardList = new();

    private Character player;
    private Character opponent;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// キャラクターの生成
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="hp"></param>
    public void InitCharacter(OwnerStatus owner, int hp)
    {
        // キャラクターを生成し、HPの設定を行う
        if (owner == OwnerStatus.Player)
        {
            player = new(hp, owner);
        }
        else
        {
            opponent = new(hp, owner);
        }
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
    /// 攻撃カードと魔法カードに分ける
    /// </summary>
    public void SortBattleCardList()
    {
        attackCardList = myCardList.Where(card => card.cardType == CardType.Attack).ToList();

        magicCardList = myCardList.Where(card => card.cardType == CardType.Magic).ToList();
    }
}
