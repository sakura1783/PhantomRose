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

    private List<CardController> handCardList = new();


    /// <summary>
    /// 初期設定。最初に1回だけ行う
    /// </summary>
    public void SetUp()
    {
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
        GameData.instance.InitCharacter(OwnerStatus.Player, 10);

        // プレイヤーのHP購読処理
        battleUIPresenter.SubscribePlayerHp();

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
    /// <param name="popCloseAction"></param>
    /// <returns></returns>
    public async UniTask Initialize(CancellationToken token) //UnityAction popCloseAction)
    {
        // デリゲートに登録
        //battleEndAction = popCloseAction;

        for (int i = 0; i < handCardList.Count; i++)
        {
            Destroy(handCardList[i].gameObject);
        }

        handCardList.Clear();

        // プレイヤーのカードは毎回初期化(前バトルで変更された攻撃力など)
        GameData.instance.GetPlayer().CopyCardDataList = new ReactiveCollection<CardData>(GameData.instance.myCardList);

        for (int i = 0; i < GameData.instance.GetPlayer().CopyCardDataList.Count; i++)
        {
            CardController card = Instantiate(cardPrefab, playerHandCardTran);
            card.SetUp(GameData.instance.GetCardData(i), descriptionPop);

            handCardList.Add(card);
        }

        Debug.Log($"handCardList : {handCardList.Count}");

        // TODO GameDataへ移行予定
        playerHandCardManager = new(handCardList, SelectCard);
        opponentHandCardManager = new(handCardList, cardSlotManager);

        // カードの効果が全て終了したら購読する
        //cardHandler.CommandSubject
        //    .Subscribe(_ => PrepareNextTurn());

        // TODO デバッグ用にプレイヤーと対戦相手の生成(対戦相手はバトルのたびにインスタンスする)
        GameData.instance.InitCharacter(OwnerStatus.Opponent, 5);

        // キャラのHP、Shield、バフデバフなどの購読処理
        battleUIPresenter.SubscribeEveryBattle();

        await UniTask.Delay(500, cancellationToken: token);  // 第一引数はミリ秒。1000ミリ秒で1秒

        PrepareNextTurn();
    }

    /// <summary>
    /// 次のターンに向けた準備処理
    /// </summary>
    private void PrepareNextTurn()
    {
        // TODO クールタイムの減少など

        // 盤面のリセット
        ResetBattleField();

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
        // TODO カードのクールタイムを設定

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

            // ルート番号を初期化
            mainGameManager.CurrentRouteIndex.Value = 0;

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

            mainGameManager.CurrentRouteIndex.Value = 0;

            // ゲームオーバーのポップアップを開く
            gameUpPop.ShowPopUp(false);

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
        playerHandCardManager.ActivateSelectedCards(cardSlotManager.setPlayerCardList);

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
}
