using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class ShopEventPop : PopupBase
{
    [SerializeField] private CanvasGroup cardPurchaseGroup;

    [SerializeField] private Button btnRedraw;
    [SerializeField] private Button btnBuy;

    [SerializeField] private Button btnHideCardDescription;

    [SerializeField] private Text txtClerk;

    [SerializeField] private Transform productPlace;

    [SerializeField] private ShopCardController shopCardPrefab;

    //TODO [SerializeField] private CardDescriptionPop cardDescriptionPop;  // isDisplayCardDescription取得用

    //TODO
    //カード引き直しの回数更新
    //cardPurchaseポップアップが開いたあと、最初のクリックでポップアップを非表示にする


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        // 各ボタンの設定
        btnBuy.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ => PurchaseCard())
            .AddTo(this);

        btnRedraw.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => GenerateCardProduct())
            .AddTo(this);

        btnHideCardDescription.OnClickAsObservable()
            //TODO .Where(_ => cardDescriptionPop.IsDisplayCardDescriptionPop)
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                SwitchCardPurchasePopVisibility(false);
                Debug.Log("タップされました");
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public override void ShowPopUp(CardData data = null)
    {
        txtClerk.text = "いらっしゃいませ！\nゆっくり選んでいってくださいね。";

        // カードの商品をランダムに生成し、初期設定
        GenerateCardProduct();

        base.ShowPopUp();
    }

    /// <summary>
    /// カードの商品を生成し、初期設定
    /// </summary>
    private void GenerateCardProduct()
    {
        // 生成してある商品(productPlaceの子要素)を全て削除する
        foreach (Transform child in productPlace)
        {
            Destroy(child.gameObject);
        }

        var generateCardList = GetRandom();

        foreach (var data in generateCardList)
        {
            var product = Instantiate(shopCardPrefab, productPlace);
            product.SetUp(data, this);
        }
    }

    /// <summary>
    /// リストの中からランダムに複数のCardDataを重複なしで取得する
    /// </summary>
    /// <returns></returns>
    private List<CardData> GetRandom()
    {
        List<CardData> selectedDataList = new();

        while (selectedDataList.Count < 4)
        {
            int selectNo = Random.Range(0, DataBaseManager.instance.cardDataSO.cardDataList.Count);

            if (!selectedDataList.Contains(DataBaseManager.instance.cardDataSO.cardDataList[selectNo]))
            {
                selectedDataList.Add(DataBaseManager.instance.cardDataSO.cardDataList[selectNo]);
            }
        }

        return selectedDataList;
    }

    /// <summary>
    /// TODO カードを買う際の処理
    /// </summary>
    private void PurchaseCard()
    {
        // ルビーを減らす

        // カードデッキに買ったカードを追加 

        // CardPurchaseポップアップを非表示にする
        SwitchCardPurchasePopVisibility(false);

        txtClerk.text = "ご購入ありがとうございます！";
    }

    /// <summary>
    /// CardPurchaseポップアップの表示、非表示の切り替え
    /// </summary>
    /// <param name="alphaValue">trueでポップアップを表示</param>
    public void SwitchCardPurchasePopVisibility(bool showPopup)
    {
        if (showPopup)
        {
            cardPurchaseGroup.DOFade(1, 0.2f)
                .SetEase(ease)
                .OnComplete(() => cardPurchaseGroup.blocksRaycasts = true);

            //TODO ポップアップの初期設定
        }
        else
        {
            cardPurchaseGroup.DOFade(0, 0.2f)
                .SetEase(ease)
                .OnComplete(() => cardPurchaseGroup.blocksRaycasts = false);
        }
    }
}
