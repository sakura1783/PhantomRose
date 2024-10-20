using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

/// <summary>
/// 宝物の種類
/// </summary>
public enum TreasureType
{
    None,  // ハズレ。何も起きない
    GetItem,
    MiniGame,
}

public class SearchEventPop : PopupBase
{
    [SerializeField] private TreasureButtonController btnTreasurePrefab;

    [SerializeField] private Transform[] treasureButtonTrans;

    [SerializeField] private CanvasGroup notFoundPopGroup;

    [SerializeField] private TreasureDiscoveryPop treasureDiscoveryPop;

    [SerializeField] private MiniGamePop miniGamePop;

    [SerializeField] private Button btnLeave;

    private List<TreasureButtonController> treasureButtonList = new();

    private TreasureType[] events = new TreasureType[4] { TreasureType.MiniGame, TreasureType.MiniGame, TreasureType.MiniGame, TreasureType.MiniGame};  // 必要であれば変更する


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        notFoundPopGroup.alpha = 0;
        notFoundPopGroup.blocksRaycasts = false;

        btnLeave.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ => PopupManager.instance.GoBack())
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public override void ShowPopUp(CardData cardData = null)
    {
        notFoundPopGroup.alpha = 0;
        notFoundPopGroup.blocksRaycasts = false;

        treasureDiscoveryPop.CanvasGroup.alpha = 0;
        treasureDiscoveryPop.CanvasGroup.blocksRaycasts = false;

        miniGamePop.CanvasGroup.alpha = 0;
        miniGamePop.CanvasGroup.blocksRaycasts = false;

        // ボタンの生成と各イベントの設定
        GenerateTreasureButtons();

        base.ShowPopUp(cardData);
    }

    /// <summary>
    /// ボタンの生成と各イベントの設定
    /// </summary>
    private void GenerateTreasureButtons()
    {
        events = events.OrderBy(treasure => Random.value).ToArray();

        for (int i = 0; i < treasureButtonTrans.Length; i++)
        {
            TreasureButtonController button = Instantiate(btnTreasurePrefab, treasureButtonTrans[i], false);
            button.SetUp(OnClickTreasureButton, events[i]);

            treasureButtonList.Add(button);
        }
    }

    /// <summary>
    /// btnTreasureを押した際の処理
    /// </summary>
    /// <param name="treasureType"></param>
    private void OnClickTreasureButton(TreasureType treasureType)
    {
        // 各イベントへ分岐
        switch (treasureType)
        {
            case TreasureType.None:
                notFoundPopGroup.DOFade(1, 0.5f)
                    .SetEase(ease)
                    .OnComplete(() => notFoundPopGroup.blocksRaycasts = true);
                break;

            case TreasureType.GetItem:
                treasureDiscoveryPop.ShowTreasureDiscoveryPop(GetRandomItem());
                break;

            case TreasureType.MiniGame:
                miniGamePop.ShowPopUp();
                break;

            default:
                Debug.Log("該当の探索イベントがありません");
                break;
        }
    }

    /// <summary>
    /// どのアイテムを獲得するかランダムに決定
    /// </summary>
    /// <returns></returns>
    private ItemDataSO.ItemData GetRandomItem()
    {
        int randomNo = Random.Range(0, DataBaseManager.instance.itemDataSO.itemDataList.Count);

        return DataBaseManager.instance.itemDataSO.itemDataList[randomNo];
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public override void HidePopUp()
    {
        base.HidePopUp();

        for (int i = 0; i < treasureButtonList.Count; i++)
        {
            Destroy(treasureButtonList[i].gameObject);
        }

        treasureButtonList.Clear();
    }
}
