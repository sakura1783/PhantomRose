using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class HomeAlwaysPop : PopupBase
{
    [SerializeField] private Button btnToMyRoom;
    [SerializeField] private Button btnToBattle;
    [SerializeField] private Button btnToStore;

    [SerializeField] private Button[] buttons = new Button[3];

    private Color32 brightColor = new (51, 42, 42, 0);
    private Color32 darkColor = new (51, 42, 42, 255);

    //TODO 変数追加


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        // 各ボタンの設定
        btnToMyRoom.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                SetButtonsColor(btnToMyRoom);
                PopupManager.instance.Show<MyRoomPop>(false);
            })
            .AddTo(this);

        btnToBattle.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                SetButtonsColor(btnToBattle);
                PopupManager.instance.SwitchToBattleOrHomeScene("Battle");
            })
            .AddTo(this);

        btnToStore.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                SetButtonsColor(btnToStore);
                PopupManager.instance.Show<StorePop>(false);
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public override void ShowPopUp(CardData cardData = null)
    {
        base.ShowPopUp();

        SetButtonsColor(btnToMyRoom);
    }

    /// <summary>
    /// ボタンの色を変更
    /// </summary>
    /// <param name="nextColor"></param>
    private void SetButtonsColor(Button pressedButton)
    {
        foreach (var button in buttons)
        {
            if (button == pressedButton)
            {
                button.image.color = brightColor;
            }
            else
            {
                button.image.color = darkColor;
            }
        }
    }
}
