using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GameData
{
    //public List<int> myCardList = new();  // 持っているカードのid番号のList。この情報を使って、毎バトル時、手札を生成する
    public Dictionary<int, int> myCardList = new();  // Dictionary<serialNo, cardId>

    public List<CardData> attackCardList = new();
    public List<CardData> magicCardList = new();

    public int handCardCapacity;  // TODO とりあえず、固定値

    public int inventoryCapacity;
    public List<ItemDataSO.ItemData> myItemList = new();

    public List<int> achievedChallengeTaskList = new();

    public ReactiveProperty<int> DiamondCount = new();
    public ReactiveProperty<int> RubyCount = new();

    public ReactiveProperty<int> PurpleGemCount = new();
    public ReactiveProperty<int> GoldGemCount = new();
    public ReactiveProperty<int> DiamondGemCount = new();
    public ReactiveProperty<int> RubyGemCount = new();

    public List<int> levelupCardSerialNumbers = new();  // レベルアップしたカードたちの通し番号を持つList

    // TODO いらなくなるかも
    public ReactiveProperty<bool> HasSaveData = new();  // セーブデータがあるかどうか。ある場合、その情報を使って中断した部分から再開する。

    private Character player;
    private Character opponent;

    private Dictionary<GemType, ReactiveProperty<int>> gemCounts = new();

    private int currentSerialNo;  // カードの通し番号の最後の値。カードを生成する際に、この値をつけ、通し番号は常に一意なものとする
    public int CurrentSerialNo
    {
        get => currentSerialNo;
        set => currentSerialNo = value;
    }

    /// <summary>
    /// レベルアップしたカードの情報
    /// </summary>
    [System.Serializable]  // Serializable属性を付けないと、levelUpCardDataList変数をpublicにしても、インスペクターに表示されない
    public class LevelUpCardData
    {
        public int serialNo;

        public int attackPower;
        public int recoveryPower;
        public int shieldPower;

        public int buffDuration;
        public int debuffDuration;
    }

    public List<LevelUpCardData> levelUpCardDataList = new();


    /// <summary>
    /// キャラクターの生成
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="hp"></param>
    /// <param name="handCards"></param>
    public void InitCharacter(OwnerStatus owner, int hp)
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

    /// <summary>
    /// 攻撃カードと魔法カードに分ける
    /// </summary>
    public void SortBattleCardList()
    {
        // レベルが低い順に並び替え
        GetPlayer().HandCardList = GetPlayer().HandCardList.OrderBy(card => card.level).ToList();

        attackCardList = GetPlayer().HandCardList.Where(card => card.cardType == CardType.攻撃).ToList();
        magicCardList = GetPlayer().HandCardList.Where(card => card.cardType == CardType.魔法).ToList();
    }

    /// <summary>
    /// GemCounts変数の初期化
    /// </summary>
    public void InitGemCounts()
    {
        gemCounts.Add(GemType.Purple, new ReactiveProperty<int>());
        gemCounts.Add(GemType.Gold, new ReactiveProperty<int>());
        gemCounts.Add(GemType.Diamond, new ReactiveProperty<int>());
        gemCounts.Add(GemType.Ruby, new ReactiveProperty<int>());
    }

    /// <summary>
    /// ジェムの更新
    /// </summary>
    /// <param name="gemType"></param>
    /// <param name="value"></param>
    public void UpdateGemCount(GemType gemType, int value)
    {
        //switch (gemType)
        //{
        //    case GemType.Purple:
        //        PurpleGemCount.Value += value;
        //        break;

        //    case GemType.Gold:
        //        GoldGemCount.Value += value;
        //        break;

        //    case GemType.Diamond:
        //        DiamondGemCount.Value += value;
        //        break;

        //    case GemType.Ruby:
        //        RubyGemCount.Value += value;
        //        break;

        //    default:
        //        Debug.Log("該当のGemTypeがありません");
        //        break;
        //}

        // Dictionaryの変数を利用して、上記をリファクタリング
        if (gemCounts.ContainsKey(gemType))
        {
            gemCounts[gemType].Value += value;
        }
        else
        {
            Debug.Log("該当のGemTypeがありません");
        }
    }

    /// <summary>
    /// 複数のジェムの更新
    /// </summary>
    /// <param name="gemTypes"></param>
    /// <param name="values"></param>
    public void UpdateGemCounts(GemType[] gemTypes, int[] values)
    {
        if (gemTypes.Length != values.Length)
        {
            Debug.Log("GemTypesとValuesの配列の長さが一致しません");
            return;
        }

        for (int i = 0; i < gemTypes.Length; i++)
        {
            if (gemCounts.ContainsKey(gemTypes[i]))
            {
                gemCounts[gemTypes[i]].Value += values[i];
            }
            else
            {
                Debug.Log("該当のGemTypeがありません");
            }
        }
    }

    /// <summary>
    /// 自分からみて、ターゲットを見つける
    /// </summary>
    /// <param name="myStatus"></param>
    /// <returns></returns>
    public OwnerStatus GetTarget(OwnerStatus myStatus)
    {
        return myStatus == OwnerStatus.Player ? OwnerStatus.Opponent : OwnerStatus.Player;
    }

    /// <summary>
    /// 引数で指定したカードをmyCardList(所持カード)に追加
    /// 通し番号を付けて管理
    /// </summary>
    /// <param name="cardId"></param>
    //public void AddMyCard(int cardId)
    //{
    //    myCardList.Add(currentSerialNo, cardId);
    //}

    /// <summary>
    /// 引数に指定した通し番号を持つカードをmyCardListから削除
    /// </summary>
    /// <param name="serialNo"></param>
    public void RemoveMyCard(int serialNo)
    {
        if (myCardList.ContainsKey(serialNo))
        {
            myCardList.Remove(serialNo);
        }
    }
}
