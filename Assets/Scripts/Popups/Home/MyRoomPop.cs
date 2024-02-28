using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MyRoomPop : PopupBase
{
    [SerializeField] private Button btnToUpgradePop;
    [SerializeField] private Button btnToCardGalleryPop;
    [SerializeField] private Button btnToChallengeTaskPop;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        //TODO 各ボタンの購読処理
        btnToUpgradePop.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ => PopupManager.instance.Show<UpgradePop>())
            .AddTo(this);

        btnToChallengeTaskPop.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ => PopupManager.instance.Show<ChallengeTaskPop>())
            .AddTo(this);
    }
}
