using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class VictoryPop : PopupBase
{
    [SerializeField] private Button btnGet;

    [SerializeField] private CardController cardPrefab;

    [SerializeField] private Transform rewardTran;

    [SerializeField] private DescriptionPop descriptionPop;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        btnGet.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // TODO インベントリにカードを追加

                PopupManager.instance.GoBack();
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        GenerateRandomCard();

        base.ShowPopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public override void HidePopUp()
    {
        descriptionPop.HidePopUp();
        base.HidePopUp();
    }

    /// <summary>
    /// ランダムなカードを生成する
    /// </summary>
    private void GenerateRandomCard()
    {
        // 前回生成したカードを削除
        foreach (Transform child in rewardTran)
        {
            Destroy(child.gameObject);
        }

        // ランダムにカード報酬を生成
        int randomNo = Random.Range(0, DataBaseManager.instance.cardDataSO.cardDataList.Count);
        var cardData = DataBaseManager.instance.cardDataSO.cardDataList[randomNo];
        var card = Instantiate(cardPrefab, rewardTran);
        card.SetUp(cardData, descriptionPop);
        card.transform.localScale = new Vector2(1.3f, 1.3f);
    }
}
