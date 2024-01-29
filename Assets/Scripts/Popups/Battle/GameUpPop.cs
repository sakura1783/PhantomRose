using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameUpPop : PopupBase
{
    [SerializeField] private Button btnToHome;

    [SerializeField] private Text txtStageNo;
    [SerializeField] private Text txtWinOrLose;


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
                PopupManager.instance.Show<HomeAlwaysPop>(false);
                PopupManager.instance.Show<MyRoomPop>(false, false);
            })
            .AddTo(this);
    }

    public override void ShowPopUp(CardData cardData = null)
    {
        // TODO 各値の設定

        base.ShowPopUp(cardData);
    }
}
