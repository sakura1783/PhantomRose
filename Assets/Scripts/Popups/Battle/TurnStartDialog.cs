using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class TurnStartDialog : PopupBase
{
    [SerializeField] private Button btnScrollLeft;
    [SerializeField] private Button btnScrollRight;

    [SerializeField] private Button btnTurnStart;
    [SerializeField] private Button btnFieldReset;

    [SerializeField] private Text txtFieldResetCost;

    [SerializeField] private CanvasGroup turnStartGroup;
    [SerializeField] private CanvasGroup fieldResetGroup;

    [SerializeField] private int fieldResetCost;

    [SerializeField] private BattleEventManager battleEventManager;

    [SerializeField] private DescriptionPop descriptionPop;

    private CanvasGroup currentGroup;  // 現在表示されているCanvas


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        fieldResetGroup.alpha = 0;
        fieldResetGroup.blocksRaycasts = false;

        currentGroup = turnStartGroup;

        txtFieldResetCost.text = fieldResetCost.ToString();
        txtFieldResetCost.color = ColorManager.instance.GetColor(ColorType.LightRed);

        // 各ボタンの購読処理
        btnScrollLeft.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
            .Subscribe(_ => SwitchDialog())
            .AddTo(this);

        btnScrollRight.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
            .Subscribe(_ => SwitchDialog())
            .AddTo(this);

        btnTurnStart.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                HidePopUp();

                // ターン開始
                battleEventManager.SubmitCards();
            })
            .AddTo(this);

        btnFieldReset.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                HidePopUp();

                // ルビー支払い
                GameData.instance.RubyCount.Value -= fieldResetCost;

                // プレイヤーのカードをキャンセル
                battleEventManager.CancelCards();
            })
            .AddTo(this);
    }

    /// <summary>
    /// ダイアログの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        if (currentGroup == fieldResetGroup)
        {
            // 表示をもとに戻す
            SwitchDialog();
        }

        base.ShowPopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public override void HidePopUp()
    {
        descriptionPop.HidePopUp();
        base.HidePopUp();
    }

    /// <summary>
    /// 現在のCanvasを非表示にして、それ以外のCanvasを表示
    /// </summary>
    /// <param name="showCanvas"></param>
    private void SwitchDialog()
    {
        currentGroup.alpha = 0;
        currentGroup.blocksRaycasts = false;

        //if (currentGroup == turnStartGroup)
        //{
        //    fieldResetGroup.DOFade(1, 0.5f).SetEase(ease).OnComplete(() => fieldResetGroup.blocksRaycasts = true);
        //}
        //else
        //{
        //    turnStartGroup.DOFade(1, 0.5f).SetEase(ease).OnComplete(() => turnStartGroup.blocksRaycasts = true);
        //}

        //上記をリファクタリング
        CanvasGroup showGroup = (currentGroup == turnStartGroup) ? fieldResetGroup : turnStartGroup;
        showGroup.DOFade(1, 0.5f).SetEase(ease).OnComplete(() => showGroup.blocksRaycasts = true);

        currentGroup = showGroup;
    }
}
