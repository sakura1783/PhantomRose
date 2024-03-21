using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class DiscardSubmitPop : PopupBase
{
    [SerializeField] private Text txtSubmit;

    [SerializeField] private Button btnCancel;
    [SerializeField] private Button btnSubmit;

    private Channel<bool> confirmChannel = default;  // 型に対しての初期値が入る


    public override void SetUp()
    {
        btnCancel.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                confirmChannel.Writer.TryWrite(false);  // Writerで一番先頭に追加
                HidePopUp();
            })
            .AddTo(this);

        btnSubmit.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                confirmChannel.Writer.TryWrite(true);
                HidePopUp();
            })
            .AddTo(this);
    }

    public override void ShowPopUp(CardData cardData = null)
    {
        base.ShowPopUp();

        txtSubmit.text = $"{cardData.name} を捨てますか？";
    }

    public async UniTask<bool> WaitConfirm()
    {
        // 今回bool型でチャネルを初期化
        confirmChannel = Channel.CreateSingleConsumerUnbounded<bool>();

        // ReadAsyncでボタンを押すまで処理を待機
        bool result = await confirmChannel.Reader.ReadAsync();

        // 追加の書き込みをしないようにする
        confirmChannel.Writer.TryComplete();

        return result;
    }
}
