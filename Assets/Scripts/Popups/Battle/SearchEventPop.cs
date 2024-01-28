using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

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

    private List<TreasureButtonController> treasureButtonList = new();

    // TODO 必要であれば変更する
    private TreasureType[] events = new TreasureType[4] { TreasureType.None, TreasureType.GetItem, TreasureType.GetItem, TreasureType.GetItem};


    //TODO 消す
    //void Start()
    //{
    //    this.UpdateAsObservable()
    //        .Where(_ => Input.GetKeyDown(KeyCode.Return))
    //        .Subscribe(_ => ShowPopUp());

    //    this.UpdateAsObservable()
    //        .Where(_ => Input.GetKeyDown(KeyCode.Backspace))
    //        .Subscribe(_ => HidePopUp());
    //}

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
    public override void ShowPopUp(CardData cardData = null)
    {
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
        Debug.Log(treasureType);

        //TODO switch文
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
