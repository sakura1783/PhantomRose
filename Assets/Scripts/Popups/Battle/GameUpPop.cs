using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class GameUpPop : PopupBase
{
    [SerializeField] private Button btnToHome;

    [SerializeField] private Text txtStageNo;
    [SerializeField] private Text txtWinOrLose;

    [SerializeField] private Image imgGameUp;

    [SerializeField] private Sprite victoryImage;
    [SerializeField] private Sprite defeatImage;

    [SerializeField] private MainGameManager mainGameManager;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        btnToHome.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                PopupManager.instance.SwitchToBattleOrHomeScene("Home");
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="isVictorious"></param>
    public void ShowPopUp(bool isVictorious)
    {
        // 各値の設定
        txtStageNo.text = mainGameManager.CurrentRouteIndex.ToString();

        txtWinOrLose.text = isVictorious ? "勝利" : "敗北";
        txtWinOrLose.color = isVictorious ? ColorManager.instance.GetColor(ColorType.Yellow) : ColorManager.instance.GetColor(ColorType.DarkRed);

        imgGameUp.sprite = isVictorious ? victoryImage : defeatImage;

        // Canvasの表示
        canvasGroup.DOFade(1, 0.5f).SetEase(ease).OnComplete(() => canvasGroup.blocksRaycasts = true);
    }
}
