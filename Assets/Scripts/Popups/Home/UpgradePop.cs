using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UpgradePop : PopupBase
{
    [SerializeField] private UpgradeDataSO upgradeDataSO;

    [SerializeField] private Text txtPurpleGemCount;
    [SerializeField] private Text txtGoldGemCount;
    [SerializeField] private Text txtDiamondGemCount;
    [SerializeField] private Text txtRubyGemCount;

    [SerializeField] private Transform elementsTran;  // ScrollViewのContentのTransform

    [SerializeField] private Transform purpleElementTran;
    [SerializeField] private Transform goldElementTran;
    [SerializeField] private Transform diamondElementTran;
    [SerializeField] private Transform rubyElementTran;

    [SerializeField] private UpgradeElementController upgradeElementPrefab;

    [SerializeField] private GameObject upgradeDisabledDialog;

    private GameObject dialog;  // 生成したダイアログ


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        txtPurpleGemCount.text = GameData.instance.PurpleGemCount.ToString();
        txtGoldGemCount.text = GameData.instance.GoldGemCount.ToString();
        txtDiamondGemCount.text = GameData.instance.DiamondGemCount.ToString();
        txtRubyGemCount.text = GameData.instance.RubyGemCount.ToString();

        // 各ジェムの購読処理
        GameData.instance.PurpleGemCount
            .Subscribe(value => txtPurpleGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        GameData.instance.GoldGemCount
            .Subscribe(value => txtGoldGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        GameData.instance.DiamondGemCount
            .Subscribe(value => txtDiamondGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        GameData.instance.RubyGemCount
            .Subscribe(value => txtRubyGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        // アップグレード要素の生成。種類ごとに適切な場所に生成する
        foreach (var data in upgradeDataSO.upgradeDataList)
        {
            switch (data.gemType)
            {
                case GemType.Purple:
                    var purpleElement = Instantiate(upgradeElementPrefab, purpleElementTran);
                    purpleElement.SetUp(data);
                    break;

                case GemType.Gold:
                    var goldElement = Instantiate(upgradeElementPrefab, goldElementTran);
                    goldElement.SetUp(data);
                    break;

                case GemType.Diamond:
                    var diamondElement = Instantiate(upgradeElementPrefab, diamondElementTran);
                    diamondElement.SetUp(data);
                    break;

                case GemType.Ruby:
                    var rubyElement = Instantiate(upgradeElementPrefab, rubyElementTran);
                    rubyElement.SetUp(data);
                    break;

                default:
                    Debug.Log("該当のアップグレード要素が見つかりません");
                    break;
            }
        }
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        // ゲーム進行中の場合は機能が使えないことを示すダイアログを生成
        if (GameData.instance.HasSaveData.Value)
        {
            dialog = Instantiate(upgradeDisabledDialog, elementsTran);
            dialog.transform.SetAsFirstSibling();  // 子要素のうち先頭に配置
        }

        base.ShowPopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public override void HidePopUp()
    {
        base.HidePopUp();

        // 生成したダイアログを破棄
        if (dialog)
        {
            Destroy(dialog);
        }
    }
}
