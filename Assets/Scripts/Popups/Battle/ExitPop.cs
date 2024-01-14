using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ExitPop : PopupBase
{
    [SerializeField] private Button btnSaveAndExit;
    [SerializeField] private Button btnRetire;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        //TODO 各ボタンの設定
        //btnSaveAndExit

        btnRetire.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                PopupManager.instance.SwitchToBattleOrHomeScene("Home");
            })
            .AddTo(this);
    }
}
