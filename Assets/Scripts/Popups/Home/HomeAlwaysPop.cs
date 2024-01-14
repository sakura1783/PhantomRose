using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HomeAlwaysPop : PopupBase
{
    [SerializeField] private Button btnToMyRoom;
    [SerializeField] private Button btnToBattle;
    [SerializeField] private Button btnToStore;

    //TODO 変数追加


    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        // 各ボタンの設定
        btnToMyRoom.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => PopupManager.instance.Show<MyRoomPop>(false))
            .AddTo(this);

        btnToBattle.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                PopupManager.instance.SwitchToBattleOrHomeScene("Battle");
            })
            .AddTo(this);

        btnToStore.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => PopupManager.instance.Show<StorePop>(false))
            .AddTo(this);
    }
}
