using System;
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
    [SerializeField] private Transform playerHandCardTran;

    [SerializeField] private CardSlot cardSlotPrefab;

    [SerializeField] private CardController cardPrefab;

    [SerializeField] private List<CardSlot> cardSlotList = new();

    [SerializeField] private BattleUIPresenter battleUIPresenter;

    [SerializeField] private TurnStartDialog turnStartDialog;

    [SerializeField] private GameUpPop gameUpPop;

    [SerializeField] private DescriptionPop descriptionPop;

    [SerializeField] private MainGameManager mainGameManager;

    private CardSlotManager cardSlotManager;

    private readonly int slotCount = 4;

    private PlayerHandCardManager playerHandCardManager;
    private OpponentHandCardManager opponentHandCardManager;

    private CardHandler cardHandler;

    //private UnityAction battleEndAction = null;

    private List<CardController> playerHandCardList = new();
    // TODO private List<CardData> opponentHandCardList = new();


    /// <summary>
    /// 初期設定。最初に1回だけ行う
    /// </summary>
    public void SetUp()
    {
        // クールタイムのセーブデータをクリア
        PlayerPrefsHelper.ClearSaveData("CoolTime_Key");

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
        GameData.instance.InitCharacter(OwnerStatus.Player, 30);

        // プレイヤーの各ステータス購読処理
        battleUIPresenter.SubscribePlayerHp();
        battleUIPresenter.SubscribePlayerShieldValue();
        battleUIPresenter.SubscribePlayerBuff();
        battleUIPresenter.SubscribePlayerDebuff();

        // TODO プレイヤーのCardDataListの購読処理。変更されたら、そのデータを持っているカードの表示を更新する
        //subscription = GameData.instance.GetPlayer().CopyCardDataList.ObserveReplace()
        //    .Subscribe(data =>
        //    {
        //        handCardList.Where(card => card.CardData == data.NewValue).FirstOrDefault().SetUp(data.NewValue);

        //        Debug.Log("表示を更新しました");
        //    });
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

        // カードのゲームオブジェクトを削除せず、下でカードの攻撃力などの情報だけ初期化することで、クールタイムを引き継ぐ
        foreach (var card in playerHandCardList)
        {
            Destroy(card.gameObject);
        }
        playerHandCardList.Clear();
        // TODO opponentHandCardList.Clear();

        // カードの情報を初期化 (前回バトルで変更された攻撃力など)
        GameData.instance.GetPlayer().CopyCardDataList = new ReactiveCollection<CardData>(GameData.instance.myCardList);

        for (int i = 0; i < GameData.instance.GetPlayer().CopyCardDataList.Count; i++)
        {
            CardController card = Instantiate(cardPrefab, playerHandCardTran);
            card.SetUp(GameData.instance.GetCardData(i), descriptionPop);

            playerHandCardList.Add(card);
        }
        // TODO
        //foreach (var card in GameData.instance.GetOpponent().CopyCardDataList)
        //{
        //    opponentHandCardList.Add(card);
        //}

        // クールタイムのセーブデータがある場合
        if (PlayerPrefsHelper.ExistsData("CoolTime_Key"))
        {
            Debug.Log("通りました");

            var coolTimeDataDic = PlayerPrefsHelper.Load<Dictionary<int, int>>("CoolTime_Key");
            foreach (var data in coolTimeDataDic)
            {
                Debug.Log($"{data.Key}, {data.Value}");  // TODO ここのデバッグが出ない。=>coolTimeDataDicの中身が空？
            }

            Debug.Log($"coolTimeDicの値：{coolTimeDataDic}");

            foreach (var card in playerHandCardList)
            {
                // セーブデータに該当のカードが含まれていたら
                if (coolTimeDataDic.ContainsKey(card.CardData.id))
                {
                    // クールタイムを引き継ぎ
                    card.SetCoolTime(coolTimeDataDic[card.CardData.id]);

                    Debug.Log("クールタイムを引き継ぎました");
                }
            }
        }

        // TODO GameDataへ移行予定
        playerHandCardManager = new(playerHandCardList, SelectCard);
        opponentHandCardManager = new(playerHandCardList, cardSlotManager);  // TODO 同じカードリストを使っているため、敵の手札のカードにもプレイヤーのクールタイムが反映されてしまう

        // カードの効果が全て終了したら購読する
        //cardHandler.CommandSubject
        //    .Subscribe(_ => PrepareNextTurn());

        // プレイヤーの状態異常とシールド値の初期化
        InitPlayerForNewBattle();

        // TODO デバッグ用にプレイヤーと対戦相手の生成(対戦相手はバトルのたびにインスタンスする)
        GameData.instance.InitCharacter(OwnerStatus.Opponent, 10);

        // 敵のステータスの購読処理
        battleUIPresenter.SubscribeEveryBattle();

        await UniTask.Delay(500, cancellationToken: token);  // 第一引数はミリ秒。1000ミリ秒で1秒

        PrepareNextTurn();
    }

    /// <summary>
    /// 次のターンに向けた準備処理
    /// </summary>
    private void PrepareNextTurn()
    {
        // スロットに配置したカードのクールタイムを設定
        playerHandCardManager.SetCoolTimeCards(cardSlotManager.setPlayerCardList);

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
            .ExecuteCommandsAsync(cardSlotList.Select(slot => slot.cardController.CardEffect).ToList(), cardSlotList.Select(slot => slot.owner).ToList(), this.GetCancellationTokenOnDestroy());

        if (currentBattleStateResult == BattleState.Win)
        {
            // バトル上にある双方の手札のゲームオブジェクトを削除
            //playerHandCardManager.DestroyHandCards();
            //opponentHandCardManager.DestroyHandCards();

            // 購読の停止
            battleUIPresenter.EndBattle();

            // プレイヤーアイコンの位置と親子関係を初期化
            mainGameManager.ResetPlayerIconTran();

            // TODO 最後に使ったカードのクールタイム設定。2枚のうち、1枚使わずに勝った場合も考慮して実装する

            // 全カードのクールタイムを記憶
            var dic = new Dictionary<int, int>();  // カードのid番号とクールタイムの情報を持つDictionaryを作成

            foreach (var card in playerHandCardList)
            {
                dic.Add(card.CardData.id, card.CurrentCoolTime);
            }

            foreach (var data in dic)
            {
                Debug.Log($"{data.Key}, {data.Value}");
            }

            PlayerPrefsHelper.Save("CoolTime_Key", dic);  // セーブ

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
            GameData.instance.GetPlayer().Hp.Value = GameData.instance.GetPlayer().MaxHp;

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
    /// 毎バトル行う、プレイヤーのステータス初期化処理
    /// </summary>
    private void InitPlayerForNewBattle()
    {
        // 前回バトルで付随したシールドや状態異常をリセット
        GameData.instance.GetPlayer().Shield.Value = 0;
        GameData.instance.GetPlayer().BuffDuration.Value = 0;
        GameData.instance.GetPlayer().DebuffDuration.Value = 0;

        Debug.Log($"DebuffDurationの値：{GameData.instance.GetPlayer().DebuffDuration.Value}");
    }
}
