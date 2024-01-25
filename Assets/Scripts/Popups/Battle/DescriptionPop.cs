using UnityEngine;

public class DescriptionPop : PopupBase
{
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
    /// <param name="data"></param>
    public override void ShowPopUp(CardData data)
    {
        var cardDescPop = Instantiate(cardDescriptionPopPrefab, transform);
        //cardDescPop.SetUp(data);

        //for (int i = 0; i < 

        base.ShowPopUp();
    }
}
