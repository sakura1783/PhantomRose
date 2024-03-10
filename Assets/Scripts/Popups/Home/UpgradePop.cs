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

    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;  // ScrollViewのContentのLayoutGroup

    [SerializeField] private UpgradeElementController upgradeElementPrefab;

    [SerializeField] private GameObject upgradeDisabledDialog;

    private GameObject dialog;  // 生成したダイアログ


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        txtPurpleGemCount.text = GameDataManager.instance.gameData.PurpleGemCount.ToString();
        txtGoldGemCount.text = GameDataManager.instance.gameData.GoldGemCount.ToString();
        txtDiamondGemCount.text = GameDataManager.instance.gameData.DiamondGemCount.ToString();
        txtRubyGemCount.text = GameDataManager.instance.gameData.RubyGemCount.ToString();

        // 各ジェムの購読処理
        GameDataManager.instance.gameData.PurpleGemCount
            .Subscribe(value => txtPurpleGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        GameDataManager.instance.gameData.GoldGemCount
            .Subscribe(value => txtGoldGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        GameDataManager.instance.gameData.DiamondGemCount
            .Subscribe(value => txtDiamondGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        GameDataManager.instance.gameData.RubyGemCount
            .Subscribe(value => txtRubyGemCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);

        // ゲーム実行中にスクリプトからLayoutGroup内に子オブジェクトを生成してScrollView内のサイズが可変すると正しく表示されないので、以下の1、2、3で修正 (これはCanvasの画面表示更新のタイミングとLayoutGroup内のサイズ変更のタイミングがずれているために生じる)
        //Canvas.ForceUpdateCanvases();  // 1.Canvas内の表示更新

        // アップグレード要素の生成。種類ごとに適切な場所に生成する
        foreach (var data in upgradeDataSO.upgradeDataList)
        {
            Canvas.ForceUpdateCanvases();  // 1

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

            verticalLayoutGroup.CalculateLayoutInputVertical();  // 2
            verticalLayoutGroup.SetLayoutVertical();  // 3
        }

        // 2.LayoutGroup内の入力値を再計算
        //verticalLayoutGroup.CalculateLayoutInputVertical();

        // 3.レイアウト再設定(表示更新)
        //verticalLayoutGroup.SetLayoutVertical();
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        // ゲーム進行中の場合は機能が使えないことを示すダイアログを生成
        if (GameDataManager.instance.gameData.HasSaveData.Value)
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
