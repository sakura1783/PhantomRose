using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

// このポップアップはShow<>では表示しない(currentPopが置き換わり、GoBack()した時にこのポップアップだけが閉じてSearchEventPopが閉じない)。
public class TreasureDiscoveryPop : PopupBase
{
    [SerializeField] private Transform treasurePlace;

    [SerializeField] private Image imgTreasure;

    [SerializeField] private Text txtTreasureName;

    [SerializeField] private Button btnGet;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        btnGet.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // TODO インベントリにアイテムを追加(下記メソッド)
                PopupManager.instance.GoBack();
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップを表示
    /// </summary>
    /// <param name="itemData"></param>
    public void ShowTreasureDiscoveryPop(ItemData itemData)
    {
        // 各値の設定
        imgTreasure.sprite = IconManager.instance.GetItemIcon(itemData.id);
        txtTreasureName.text = itemData.itemName;

        // ポップアップ表示
        canvasGroup.DOFade(1, 0.5f)
            .SetEase(ease)
            .OnComplete(() => canvasGroup.blocksRaycasts = true);
    }

    // TODO インベントリにアイテムを追加するメソッド。インベントリを確認して、アイテムがいっぱいでなければアイテムを追加、いっぱいであればどれかを捨ててインベントリに追加する。
}
