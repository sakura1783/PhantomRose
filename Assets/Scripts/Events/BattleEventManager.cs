using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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

    private CardSlotManager cardSlotManager;

    private readonly int slotCount = 4;

    private PlayerHandCardManager playerHandCardManager;
    private OpponentHandCardManager opponentHandCardManager;

    private CardHandler cardHandler;

    private UnityAction battleEndAction = null;


    void Start()
    {
        // デバッグ用
        //Initialize();
        SetUp();
    }

    void Update()
    {
        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrepareNextTurn();
        }
    }

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
        cardHandler = new();

        // プレイヤー情報を生成
        GameData.instance.InitCharacter(OwnerStatus.Player, 15);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="token"></param>
    /// <param name="popCloseAction"></param>
    /// <returns></returns>
    public async UniTask Initialize(CancellationToken token, UnityAction popCloseAction)
    {
        // デリゲートに登録
        battleEndAction = popCloseAction;

        // 手札のカード情報から、手札のオブジェクトの作成
        List<CardController> handCardList = new();

        for (int i = 0; i < GameData.instance.myCardList.Count; i++)
        {
            CardController card = Instantiate(cardPrefab, playerHandCardTran);
            card.SetUp(GameData.instance.GetCardData(i));  // TODO 適切なidで生成されるか、確認

            handCardList.Add(card);
        }

        // TODO GameDataへ移行予定
        playerHandCardManager = new(handCardList, SelectCard);
        opponentHandCardManager = new(handCardList, cardSlotManager);

        // TODO ハンドラの生成と購読
        //cardHandler = new();

        // カードの効果が全て終了したら購読する
        // TODO cardHandler.CommandSubject.Subscribe(_ => PrepareNextTurn());

        // TODO デバッグ用にプレイヤーと対戦相手の生成(対戦相手はバトルのたびにインスタンスする)
        GameData.instance.InitCharacter(OwnerStatus.Opponent, 5);

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
    /// カード選択  // TODO 何のためのメソッド？
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

        // TODO ターン開始ダイアログ生成

        // カード実行
        PrepareExecuteAsync().Forget();
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
            // バトル終了
            ResetBattleField();

            // バトル上にある双方の手札のゲームオブジェクトを削除
            playerHandCardManager.DestroyHandCards();
            opponentHandCardManager.DestroyHandCards();

            // このポップアップを閉じる
            battleEndAction?.Invoke();

            return;
        }

        if (currentBattleStateResult == BattleState.Lose)
        {
            // TODO ゲームオーバーのポップアップを開く

            return;
        }

        Debug.Log("全てのカードを実行しました");

        //TODO カード終了タイミングを購読してもいいが、ここにある方が処理が読みやすい
        PrepareNextTurn();
    }

    /// <summary>
    /// 盤面のリセット
    /// </summary>
    private void ResetBattleField()
    {
        // スロットに配置したプレイヤーのカードだけ、再度利用できる状態にする
        playerHandCardManager.ActivateSelectedCards(cardSlotManager.setPlayerCardList);

        // Characterのカードだけ破棄
        cardSlotManager.DeleteAllCardsFromSlots();

        Debug.Log("盤面をリセットしました");
    }

    // TODO ダイアログから実行されるメソッド2つ(決定とキャンセル)用意

    public void SubmitCards()
    {

    }

    public void CancelCards()
    {

    }

    // TODO カードの実行準備用のメソッドを用意
}
