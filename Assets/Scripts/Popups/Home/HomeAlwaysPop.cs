using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HomeAlwaysPop : PopupBase
{
    [SerializeField] private Button btnToMyRoom;
    [SerializeField] private Button btnToBattle;
    [SerializeField] private Button btnToStore;

    private bool isFirstSetUp = true;

    //TODO 変数追加


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        btnToMyRoom.image.color = new Color32(51, 42, 42, 0);
        btnToBattle.image.color = new Color32(51, 42, 42, 255);
        btnToStore.image.color = new Color32(51, 42, 42, 255);

        // 初回だけ以下の処理を行う
        if (isFirstSetUp)
        {
            // 各ボタンの設定
            btnToMyRoom.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ =>
                {
                    btnToMyRoom.image.color = new Color32(51, 42, 42, 0);
                    btnToBattle.image.color = new Color32(51, 42, 42, 255);
                    btnToStore.image.color = new Color32(51, 42, 42, 255);
                    PopupManager.instance.Show<MyRoomPop>(false);
                })
                .AddTo(this);

            btnToBattle.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ =>
                {
                    btnToMyRoom.image.color = new Color32(51, 42, 42, 255);
                    btnToBattle.image.color = new Color32(51, 42, 42, 0);
                    btnToStore.image.color = new Color32(51, 42, 42, 255);
                    PopupManager.instance.SwitchToBattleOrHomeScene("Battle");
                })
                .AddTo(this);

            btnToStore.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ =>
                {
                    btnToMyRoom.image.color = new Color32(51, 42, 42, 255);
                    btnToBattle.image.color = new Color32(51, 42, 42, 255);
                    btnToStore.image.color = new Color32(51, 42, 42, 0);
                    PopupManager.instance.Show<StorePop>(false);
                })
                .AddTo(this);

            isFirstSetUp = false;
        }
    }
}
