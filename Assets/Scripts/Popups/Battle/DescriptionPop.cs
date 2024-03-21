using DG.Tweening;
using UnityEngine;

public class DescriptionPop : PopupBase
{
    private bool isDisplayDescriptionPop = false;
    public bool IsDisplayDescriptionPop => isDisplayDescriptionPop;

    private CardData tapCardData;  // タップしたカードの情報。外部クラスで使用する
    public CardData TapCardData => tapCardData;

    [SerializeField] private CardDescriptionPopController cardDescriptionPopPrefab;

    [SerializeField] private StateDescriptionPopController stateDescriptionPopPrefab;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData)
    {
        tapCardData = cardData;

        // 生成した子要素のポップアップを全て破棄
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // CardDescポップアップの生成と初期設定
        var cardDescPop = Instantiate(cardDescriptionPopPrefab, transform);
        cardDescPop.SetUp(cardData, false);

        // StateDescポップアップの生成と初期設定
        for (int i = 0; i < cardData.stateList.Count; i++)
        {
            var stateDescPop = Instantiate(stateDescriptionPopPrefab, transform);
            stateDescPop.SetUp(DataBaseManager.instance.stateDataSO.stateDataList[cardData.stateList[i].stateId]);
        }

        canvasGroup.DOFade(1, 0.5f)
            .SetEase(ease)
            .OnComplete(() => isDisplayDescriptionPop = true);
    }
}
