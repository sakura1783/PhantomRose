using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class MiniGamePop : PopupBase
{
    [SerializeField] private WishDataSO wishDataSO;

    [SerializeField] private Text txtChanceCount;

    [SerializeField] private CanvasGroup judgementDialogGroup;
    [SerializeField] private Image imgJudgementDialog;

    [SerializeField] private CanvasGroup firstGroup;
    [SerializeField] private CanvasGroup secondGroup;

    [SerializeField] private Sprite checkSprite;
    [SerializeField] private Sprite crossSprite;

    [SerializeField] private Transform tarotTran;

    [SerializeField] private Transform wishTran;

    [SerializeField] private TarotController tarotPrefab;

    [SerializeField] private WishButtonController wishPrefab;

    [SerializeField] private int tarotCount;  // 生成するタロットの数

    private int chanceCount;

    private List<TarotType> tarotTypeList = new();

    private List<TarotController> generatedTarotList = new();

    public ReactiveCollection<TarotType> selectedTarotList;  // めくったタロットの情報


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        selectedTarotList.ObserveCountChanged()
            .Where(_ => selectedTarotList.Count >= 2)
            .Subscribe(_ =>
            {
                // チャンスカウントを減少
                chanceCount--;
                txtChanceCount.text = chanceCount.ToString();

                if (ShowJudgementDialog())
                {
                    // ポップアップの切り替え
                    ShowSecondGroup();

                    // タロットの模様が一致している場合、以下は処理しない
                    return;
                }

                // めくったタロットの情報をリセット
                selectedTarotList.Clear();

                // すべてのタロットを裏に戻す
                foreach (var tarot in generatedTarotList)
                {
                    tarot.FrontGroup.DOFade(0, 0.3f).SetEase(ease).OnComplete(() => tarot.FrontGroup.blocksRaycasts = false);
                }

                // チャンスカウントが0になったら
                if (chanceCount <= 0)
                {
                    PopupManager.instance.GoBack();
                }
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        Initialize();

        // 生成するタロットの模様を決定
        CreateTarotList();

        // タロットの生成
        for (int i = 0; i < tarotTypeList.Count; i++)
        {
            var tarot = Instantiate(tarotPrefab, tarotTran);
            tarot.SetUp(this, tarotTypeList[i]);

            generatedTarotList.Add(tarot);
        }

        base.ShowPopUp();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        // 切り替えたポップアップをもとに戻す
        firstGroup.alpha = 1;
        firstGroup.blocksRaycasts = true;
        secondGroup.alpha = 0;
        secondGroup.blocksRaycasts = false;

        // 生成したタロットを破棄
        foreach (var tarot in generatedTarotList)
        {
            Destroy(tarot.gameObject);
        }

        // 生成したWishButtonを破棄
        foreach (Transform child in wishTran)
        {
            Destroy(child.gameObject);
        }

        // 各リストの中身をクリア
        tarotTypeList.Clear();
        generatedTarotList.Clear();
        selectedTarotList.Clear();

        // チャンスカウントの設定
        chanceCount = 2;
        txtChanceCount.text = chanceCount.ToString();
    }

    /// <summary>
    /// 生成するタロットの模様をランダムに決定
    /// </summary>
    private void CreateTarotList()
    {
        // 重複は1度だけ許し、生成するタロットの数(5)だけループ(つまり、リスト内の要素の数は2,2,1になる)
        for (int i = 0; i < tarotCount; i++)
        {
            TarotType randomTarot;

            do
            {
                randomTarot = (TarotType)Random.Range(0, (int)TarotType.count);
            }
            while (tarotTypeList.Count(t => t == randomTarot) >= 2);  // リスト内に同じ要素が2つ以上含まれている場合、繰り返す

            tarotTypeList.Add(randomTarot);
        }
    }

    /// <summary>
    /// 選んだタロットの模様が一致しているかどうか確認して、ダイアログを開く
    /// </summary>
    private bool ShowJudgementDialog()
    {
        bool isMatch = selectedTarotList[0] == selectedTarotList[1];

        // リストの中の要素が一致している場合
        if (isMatch)
        {
            imgJudgementDialog.sprite = checkSprite;
            judgementDialogGroup.DOFade(1, 0.1f).SetEase(ease).OnComplete(() => DOVirtual.DelayedCall(0.5f, () => judgementDialogGroup.DOFade(0, 0.1f).SetEase(ease)));
        }
        // 一致していない場合
        else
        {
            imgJudgementDialog.sprite = crossSprite;
            judgementDialogGroup.DOFade(1, 0.1f).SetEase(ease).OnComplete(() => DOVirtual.DelayedCall(0.5f, () => judgementDialogGroup.DOFade(0, 0.1f).SetEase(ease)));
        }

        return isMatch;
    }

    /// <summary>
    /// ポップアップの一部の表示を切り替え
    /// </summary>
    private void ShowSecondGroup()
    {
        // wishDataListの要素の数だけ、連番のリストを作成
        List<int> idList = Enumerable.Range(0, wishDataSO.wishDataList.Count).ToList();

        // 上のリストをランダムに並び替え
        List<int> generateWishList = idList.OrderBy(_ => Random.value).Take(2).ToList();

        foreach (var wishId in generateWishList)
        {
            // WishButtonの生成
            var wish = Instantiate(wishPrefab, wishTran);
            wish.SetUp(wishDataSO.wishDataList[wishId]);
        }

        // 表示切り替え
        firstGroup.alpha = 0;
        firstGroup.blocksRaycasts = false;
        secondGroup.DOFade(1, 0.3f).SetEase(ease).OnComplete(() => secondGroup.blocksRaycasts = true);
    }
}
