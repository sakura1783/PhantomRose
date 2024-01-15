using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CardController : MonoBehaviour
{
    [SerializeField] private Button cardPrefab;

    [SerializeField] private Image imgCard;
    [SerializeField] private Image imgBase;  //カードの色変更用

    [SerializeField] private Text txtAttackPointOrInterval;


    //TODO カードの情報を取得して、何のポップアップを何個用意すればいいか確認する。

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUp(CardData data)
    {
        SetCardDetail(data);

        cardPrefab.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2))
            .Subscribe(_ => PopupManager.instance.Show<CardDescriptionPop>(false, false))  //TODO 修正する
            .AddTo(this);
    }

    /// <summary>
    /// カードに情報を設定する
    /// </summary>
    private void SetCardDetail(CardData data)
    {
        // 各値を設定
        imgCard.sprite = IconManager.instance.GetCardIcon(data.spriteId);
        imgBase.color = data.cardColor;
        txtAttackPointOrInterval.text = data.attackPower <= 0 ? "" : data.attackPower.ToString();  //TODO 回復値、攻撃値、シールド値が別々に設定されているため、どうするか検討する。
    }
}
