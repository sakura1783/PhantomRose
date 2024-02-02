using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BreakEventPop : PopupBase
{
    [SerializeField] private Button btnHeal;
    [SerializeField] private Button btnResetCoolDown;
    [SerializeField] private Button btnCardDismantling;

    [SerializeField] private Text txtHealCost;
    [SerializeField] private Text txtResetCoolDownCost;
    [SerializeField] private Text txtCardCount;

    [SerializeField] private CanvasGroup cardDismantlingPromptGroup;

    [SerializeField] private int healCost;
    [SerializeField] private int resetCoolDownCost;

    // TODO テスト
    [SerializeField] private int maxCardCount;

    private bool isCardCountBeyondMax = false;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        txtHealCost.text = healCost.ToString();
        txtResetCoolDownCost.text = resetCoolDownCost.ToString();

        // ボタンの監視処理
        btnHeal.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // ルビーの支払い
                GameData.instance.RubyCount.Value -= healCost;

                // 他のボタンの制御
                SwitchActivateButtons();

                // ボタンを非アクティブ化
                btnHeal.interactable = false;  // 順番に注意。上に書いてしまうと、falseにした後、ルビーが足りていればまたtrueになってしまう
            })
            .AddTo(this);

        btnResetCoolDown.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                GameData.instance.RubyCount.Value -= resetCoolDownCost;

                SwitchActivateButtons();

                btnResetCoolDown.interactable = false;
            })
            .AddTo(this);

        btnCardDismantling.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // TODO カードデッキを開く

                Debug.Log("カード分解ポップアップを開きます");
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        SwitchActivateButtons();

        txtCardCount.text = $"{GameData.instance.myCardList.Count}/{maxCardCount}";

        isCardCountBeyondMax = GameData.instance.myCardList.Count > maxCardCount;
        ProcessByCardCountStatus();

        base.ShowPopUp();
    }

    /// <summary>
    /// ボタンのアクティブ状態の切り替え
    /// </summary>
    private void SwitchActivateButtons()
    {
        btnHeal.interactable = GameData.instance.RubyCount.Value >= healCost;
        txtHealCost.color = GameData.instance.RubyCount.Value >= healCost ? Color.white : Color.red;

        btnResetCoolDown.interactable = GameData.instance.RubyCount.Value >= resetCoolDownCost;
        txtResetCoolDownCost.color = GameData.instance.RubyCount.Value >= resetCoolDownCost ? Color.white : Color.red;
    }

    /// <summary>
    /// isCardCountBeyondMax(カードの数)に基づいて処理を行う
    /// </summary>
    private void ProcessByCardCountStatus()
    {
        // カードの数が最大値に達している場合
        if (isCardCountBeyondMax)
        {
            // カード分解を勧める表示を出す
            cardDismantlingPromptGroup.alpha = 1;

            // 文字色を赤にする
            txtCardCount.color = Color.red;

            // 「立ち去る」ボタンを押せなくする
            btnClose.interactable = false;
        }
        // 最大値に達していない場合
        else
        {
            cardDismantlingPromptGroup.alpha = 0;

            txtCardCount.color = Color.white;

            btnClose.interactable = true;
        }
    }
}
