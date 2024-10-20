using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

/// <summary>
/// バトルの状態
/// </summary>
public enum BattleState
{
    Continue,  // バトル継続
    Win,  // プレイヤー勝利
    Lose,  // プレイヤー敗北
}

public class BattleEventManager : MonoBehaviour
{
    public CardController selectedCard;

    [SerializeField] private Transform cardSlotTran;

    [SerializeField] private Transform attackCardTran;
    [SerializeField] private Transform magicCardTran;

    [SerializeField] private Transform opponentCardTran;

    [SerializeField] private CardSlot cardSlotPrefab;

    [SerializeField] private CardController cardPrefab;

    [SerializeField] private List<CardSlot> cardSlotList = new();

    [SerializeField] private BattleUIPresenter battleUIPresenter;

    [SerializeField] private TurnStartDialog turnStartDialog;

    [SerializeField] private GameUpPop gameUpPop;

    [SerializeField] private DescriptionPop descriptionPop;

    [SerializeField] private MainGameManager mainGameManager;

    [SerializeField] private CardDeckPop cardDeckPop;

    private CardSlotManager cardSlotManager;

    private readonly int slotCount = 4;

    private PlayerHandCardManager playerHandCardManager;

    private OpponentHandCardManager opponentHandCardManager;

    private CardHandler cardHandler;

    //private UnityAction battleEndAction = null;

    private List<CardController> playerHandCardObjs = new();  // 生成した手札カードのプレハブを入れておく場所
    private List<CardController> opponentHandCardObjs = new();

    [System.Serializable]
    public class CoolTimeData
    {
        public int cardId;
        public int coolTime;
    }

    /// <summary>
    /// Json化する際、そのままListを渡すとJson化しても無視される(できない)ので、Listをラップするためのクラスを用意
    /// </summary>
    [System.Serializable]
    public class Wrapper
    {
        public List<CoolTimeData> coolTimeDataList = new();
    }

    private const string CoolTime_Key = "CoolTime_Key";  // クールタイムのセーブ用Key


    /// <summary>
    /// 初期設定。最初に1回だけ行う
    /// </summary>
    public void SetUp()
    {
        // クールタイムのセーブデータをクリア
        PlayerPrefsHelper.ClearSaveData(CoolTime_Key);

        // CardDataSO.cardDataListをディープコピー
        DataBaseManager.instance.copyCardDataList = DataBaseManager.instance.cardDataSO.GetCopyCardDataList();  

        // カードスロットの作成
        for (int i = 0; i < slotCount; i++)
        {
            CardSlot slot = Instantiate(cardSlotPrefab, cardSlotTran);
            slot.SetUp(i);

            cardSlotList.Add(slot);
        }

        cardSlotManager = new(this, cardSlotList);

        // ハンドラの生成と購読
        cardHandler = new();  // これによって、CardHandlerクラスが利用できるようになる

        // プレイヤー情報を生成
        GameDataManager.instance.gameData.InitCharacter(OwnerStatus.Player, 30);

        // プレイヤーの初期手札カードを作成
        for (int i = 0; i < 7; i++)
        {
            cardDeckPop.AddMyCard(DataBaseManager.instance.copyCardDataList[i]);
        }

        // プレイヤーの各ステータス購読処理
        battleUIPresenter.SubscribePlayerHp();
        battleUIPresenter.SubscribePlayerShieldValue();
        battleUIPresenter.SubscribePlayerBuff();
        battleUIPresenter.SubscribePlayerDebuff();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask Initialize(CancellationToken token) //UnityAction popCloseAction)
    {
        // デリゲートに登録
        //battleEndAction = popCloseAction;

        // カードの元情報を初期化
        DataBaseManager.instance.copyCardDataList = DataBaseManager.instance.cardDataSO.GetCopyCardDataList();

        // 前回バトルで生成したカードのゲームオブジェクトを削除
        foreach (var card in playerHandCardObjs)
        {
            Destroy(card.gameObject);
        }
        playerHandCardObjs.Clear();
        foreach (var card in opponentHandCardObjs)
        {
            Destroy(card.gameObject);
        }
        opponentHandCardObjs.Clear();

        InitPlayerForNewBattle();

        SetOpponentForNewBattle();

        playerHandCardManager = new(playerHandCardObjs, SelectCard);
        opponentHandCardManager = new(opponentHandCardObjs, cardSlotManager);

        // カードの効果が全て終了したら購読する
        //cardHandler.CommandSubject
        //    .Subscribe(_ => PrepareNextTurn());

        await UniTask.Delay(500, cancellationToken: token);  // 第一引数はミリ秒。1000ミリ秒で1秒

        PrepareNextTurn();
    }

    /// <summary>
    /// 次のターンに向けた準備処理
    /// </summary>
    private void PrepareNextTurn()
    {
        //スロットに配置したカードのクールタイムを設定
        //playerHandCardManager.SetCoolTimeCards(cardSlotManager.setPlayerCardList);

        // それ以外のクールタイムがあるカードのクールタイムを減少
        playerHandCardManager.UpdateCoolTimeCards(cardSlotManager.setPlayerCardList);

        // 盤面のリセット
        ResetBattleField();

        // TODO バフとデバフの管理

        // スロットのOwnerStatusをランダムで2箇所、対戦相手のスロットとして設定
        cardSlotManager.SetOpponentCardSlotsRandomly();

        // 対戦相手のスロットにランダムなカードを配置
        opponentHandCardManager.SetSlotsRandomCard();

        Debug.Log("ターン開始の準備完了");
    }

    /// <summary>
    /// カード選択時の処理
    /// </summary>
    /// <param name="chooseCard"></param>
    public void SelectCard(CardController chooseCard)
    {
        // 選択中のカードがすでにある場合
        if (selectedCard)
        {
            // 選択中のカードをクリック可能にする
            selectedCard.SetSelectable(false);
        }

        selectedCard = chooseCard;

        // 新しく選択したカードをクリック不可にする => そのままセットした場合には、クリック不可のままになる
        selectedCard.SetSelectable(true);
    }

    /// <summary>
    /// 選択しているカードを削除
    /// </summary>
    public void RemoveCard()
    {
        selectedCard = null;
    }

    /// <summary>
    /// 全てのスロットにカードがセットされた際に実行される処理
    /// </summary>
    public void OnCardSet()
    {
        Debug.Log("全てのスロットにカードがセットされました");

        // ターン開始ダイアログ表示
        turnStartDialog.ShowPopUp();  // PopupManager.Showだと、currentViewPopに引っかかって2回目以降、表示されない
    }

    /// <summary>
    /// カード実行
    /// </summary>
    /// <returns></returns>
    private async UniTask PrepareExecuteAsync()
    {
        // セットされたカードを実行し、結果を取得する
        BattleState currentBattleStateResult = await cardHandler
            .ExecuteCommandsAsync(cardSlotList.Select(slot => slot.cardController).ToList(),  
                                  cardSlotList.Select(slot => slot.owner).ToList(),
                                  this.GetCancellationTokenOnDestroy(),
                                  playerHandCardManager);

        if (currentBattleStateResult == BattleState.Win)
        {
            // バトル上にある双方の手札のゲームオブジェクトを削除
            //playerHandCardManager.DestroyHandCards();
            //opponentHandCardManager.DestroyHandCards();

            // 購読の停止
            battleUIPresenter.EndBattle();

            // プレイヤーアイコンの位置と親子関係を初期化
            mainGameManager.ResetPlayerIconTran();

            // 全カードのクールタイムを記憶
            SaveCoolTime();

            // 勝利ポップアップを開く
            PopupManager.instance.Show<VictoryPop>(false);

            // このポップアップを閉じる
            //battleEndAction?.Invoke();

            return;
        }

        if (currentBattleStateResult == BattleState.Lose)
        {
            battleUIPresenter.EndBattle();

            mainGameManager.ResetPlayerIconTran();

            // ゲームオーバーのポップアップを開く
            gameUpPop.ShowPopUp(false);

            // プレイヤーのHPを最大値に戻す
            GameDataManager.instance.gameData.GetPlayer().Hp.Value = GameDataManager.instance.gameData.GetPlayer().MaxHp;

            return;
        }

        Debug.Log("全てのカードを実行しました");

        PrepareNextTurn();  // (カード終了のタイミングを購読してもいいが、ここにある方が処理が読みやすい)
    }

    /// <summary>
    /// 盤面のリセット
    /// </summary>
    public void ResetBattleField()
    {
        // スロットに配置したプレイヤーのカードだけ、再度利用できる状態にする
        //playerHandCardManager.ActivateSelectedCards(cardSlotManager.setPlayerCardList);

        // 全てのカードを破棄
        cardSlotManager.DeleteAllCardsFromSlots();

        Debug.Log("盤面をリセットしました");
    }

    /// <summary>
    /// ターン開始
    /// </summary>
    public void SubmitCards()
    {
        // カード実行
        PrepareExecuteAsync().Forget();
    }

    /// <summary>
    /// プレイヤーのカードをキャンセル
    /// </summary>
    public void CancelCards()
    {
        // スロットに配置したプレイヤーのカードを再度利用できる状態にする
        playerHandCardManager.ActivateSelectedCards(cardSlotManager.setPlayerCardList);

        //プレイヤーのカードだけ破棄
        cardSlotManager.DeleteCardsFromSlots(OwnerStatus.Player);
    }

    /// <summary>
    /// 毎バトル行う、プレイヤーのカードやステータスの初期化処理
    /// </summary>
    private void InitPlayerForNewBattle()
    {
        // 前回バトルで付随したシールドや状態異常をリセット
        GameDataManager.instance.gameData.GetPlayer().Shield.Value = 0;
        GameDataManager.instance.gameData.GetPlayer().BuffDuration.Value = 0;
        GameDataManager.instance.gameData.GetPlayer().DebuffDuration.Value = 0;

        // 手札の情報を作成
        GameDataManager.instance.gameData.GetPlayer().HandCardList.Clear();
        foreach (var card in GameDataManager.instance.gameData.myCardList)
        {
            GameDataManager.instance.gameData.GetPlayer().HandCardList.Add(DataBaseManager.instance.copyCardDataList[card.Value]);
        }

        // 手札のカードのプレハブを生成
        GameDataManager.instance.gameData.SortBattleCardList();

        int battleSerialNo = 0;  // バトル専用の通し番号
        foreach (var cardData in GameDataManager.instance.gameData.attackCardList)
        {
            var cardObj = Instantiate(cardPrefab, attackCardTran);
            cardObj.SetUp(cardData, descriptionPop, battleSerialNo);

            playerHandCardObjs.Add(cardObj);

            battleSerialNo++;
        }
        foreach (var cardData in GameDataManager.instance.gameData.magicCardList)
        {
            var cardObj = Instantiate(cardPrefab, magicCardTran);
            cardObj.SetUp(cardData, descriptionPop, battleSerialNo);

            playerHandCardObjs.Add(cardObj);

            battleSerialNo++;
        }

        // クールタイム引き継ぎ処理
        if (PlayerPrefsHelper.ExistsData(CoolTime_Key))
        {
            var wrapper = PlayerPrefsHelper.Load<Wrapper>(CoolTime_Key);

            foreach (var data in wrapper.coolTimeDataList)
            {
                //foreach (var card in playerHandCardList)
                //{
                //    
                //    if (data.cardId == card.CardData.id && data.coolTime > 0)
                //    {
                //    }
                //}

                // 上記をリファクタリング
                // セーブデータに該当のカードが含まれていて、かつ、クールタイムがあるなら
                var card = playerHandCardObjs.FirstOrDefault(card => card.CardData.id == data.cardId && data.coolTime > 0);

                if (card)
                {
                    // クールタイムを引き継ぎ
                    card.SetCoolTime(data.coolTime);
                }
            }
        }
    }

    /// <summary>
    /// 対戦相手の設置
    /// </summary>
    private void SetOpponentForNewBattle()
    {
        // 対戦相手の生成
        GameDataManager.instance.gameData.InitCharacter(OwnerStatus.Opponent, 10);

        // TODO 対戦相手のHandCardListの作成
        GameDataManager.instance.gameData.GetOpponent().HandCardList.Clear();
        GameDataManager.instance.gameData.GetOpponent().HandCardList = GameDataManager.instance.gameData.GetPlayer().HandCardList;

        // 対戦相手の手札のカードを見えない位置に生成
        foreach (var cardData in GameDataManager.instance.gameData.GetOpponent().HandCardList)
        {
            var cardObj = Instantiate(cardPrefab, opponentCardTran);
            cardObj.SetUp(cardData, descriptionPop);

            opponentHandCardObjs.Add(cardObj);
        }

        // 敵のステータスの購読処理
        battleUIPresenter.SubscribeEveryBattle();
    }

    /// <summary>
    /// 全カードのクールタイムをセーブ
    /// </summary>
    private void SaveCoolTime()
    {
        Wrapper wrapper = new();

        foreach (var card in playerHandCardObjs)
        {
            var coolTimeData = new CoolTimeData
            {
                // 各値を設定
                cardId = card.CardData.id,
                coolTime = card.CurrentCoolTime,
            };

            wrapper.coolTimeDataList.Add(coolTimeData);
        }

        PlayerPrefsHelper.Save(CoolTime_Key, wrapper);
    }
}
