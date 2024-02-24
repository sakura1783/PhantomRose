using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UpgradeElementController : MonoBehaviour
{
    [SerializeField] private Button btnUse;
    [SerializeField] private Button btnRevert;

    [SerializeField] private Text txtUpgradeDetail;
    [SerializeField] private Text txtCost;

    [SerializeField] private Image imgGem;

    [SerializeField] private CanvasGroup revertButtonGroup;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="data"></param>
    public void SetUp(UpgradeDataSO.UpgradeData data)
    {
        // 各値の設定
        txtUpgradeDetail.text = data.detail;
        txtCost.text = data.cost.ToString();
        imgGem.sprite = IconManager.instance.GetGemSprite(data.gemType);

        // TODO ゲーム進行中はボタンを押せない
        GameData.instance.HasSaveData
            .Subscribe(value =>
            {
                btnUse.interactable = !value;
                btnRevert.interactable = !value;
            })
            .AddTo(this);

        // 購読処理
        btnUse.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
            .Where(_ => CheckEnoughGems(data))  // CheckEnoughGemsの戻り値がtrue(つまり、ジェムが足りる)時
            .Subscribe(_ =>
            {
                // ジェム消費
                GameData.instance.UpdateGemCount(data.gemType, -data.cost);

                revertButtonGroup.alpha = 1;
                revertButtonGroup.blocksRaycasts = true;
            })
            .AddTo(this);

        btnRevert.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
            .Subscribe(_ =>
            {
                // ジェムの払い戻し
                GameData.instance.UpdateGemCount(data.gemType, data.cost);

                revertButtonGroup.alpha = 0;
                revertButtonGroup.blocksRaycasts = false;
            })
            .AddTo(this);
    }

    /// <summary>
    /// ジェムが支払えるか(足りるか)どうかを確認
    /// </summary>
    /// <returns>trueで足りる、falseで足りない</returns>
    public bool CheckEnoughGems(UpgradeDataSO.UpgradeData data)
    {
        switch (data.gemType)
        {
            case GemType.Purple:
                return GameData.instance.PurpleGemCount.Value >= data.cost;

            case GemType.Gold:
                return GameData.instance.GoldGemCount.Value >= data.cost;

            case GemType.Diamond:
                return GameData.instance.DiamondGemCount.Value >= data.cost;

            case GemType.Ruby:
                return GameData.instance.RubyGemCount.Value >= data.cost;

            default:
                return true;
        }
    }
}
