using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class BattleAlwaysPop : PopupBase
{
    [SerializeField] private Button btnMenu;
    [SerializeField] private Button btnReturn;

    [SerializeField] private Button btnToStore;
    [SerializeField] private Button btnToInventory;
    [SerializeField] private Button btnToCards;
    [SerializeField] private Button btnToPlayerState;

    [SerializeField] private Text txtRubyCount;
    [SerializeField] private Text txtDiamondCount;

    [SerializeField] private CanvasGroup returnButtonGroup;

    private readonly string[] hidePopNames = new[] { "Store(Home)", "Inventory", "CardDeck", "PlayerState" };  // readonlyで、変数が一度初期化されたらその値を変更できなくする


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        txtRubyCount.text = GameDataManager.instance.gameData.RubyCount.ToString();

        // 各ボタンの設定
        btnMenu.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => PopupManager.instance.Show<ExitPop>(false, false))
            .AddTo(this);

        btnReturn.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                PopupManager.instance.GoBack();
                SetReturnButtonActivation(false);
            })
            .AddTo(this);

        btnToStore.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => EvaluatePopupsVisibility<StorePop>())
            .AddTo(this);

        btnToInventory.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => EvaluatePopupsVisibility<InventoryPop>())
            .AddTo(this);

        btnToCards.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => EvaluatePopupsVisibility<CardDeckPop>())
            .AddTo(this);

        // TODO btnToPlayerState

        GameDataManager.instance.gameData.RubyCount
            .Subscribe(value => txtRubyCount.text = Mathf.Clamp(value, 0, int.MaxValue).ToString())
            .AddTo(this);
    }

    /// <summary>
    /// 複数のポップアップの表示、非表示の切り替えを判断する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void EvaluatePopupsVisibility<T>() where T : PopupBase  // ジェネリックメソッド。呼び出し時<>内に書いたクラス(データ型)をメソッドに渡すことができる。
    {
        // 現在開いているポップアップがストア、インベントリ、カード、状態のいずれかである場合、開かれている最新のポップアップを閉じる
        if (hidePopNames.Contains(PopupManager.instance.CurrentViewPop.name))
        {
            PopupManager.instance.Show<T>(false);
        }
        // それ以外の(バトル用のポップアップが開かれている)場合、それらは閉じない
        else
        {
            PopupManager.instance.Show<T>(false, false);
        }

        // btnReturnをアクティブにする
        SetReturnButtonActivation(true);
    }

    /// <summary>
    /// btnReturnのアクティブ状態を切り替える
    /// </summary>
    /// <param name="isActive"></param>
    public void SetReturnButtonActivation(bool isActive)
    {
        int alphaValue = isActive == true ? 1 : 0;

        returnButtonGroup.DOFade(alphaValue, 0.3f)
            .SetEase(ease)
            .OnComplete(() => returnButtonGroup.blocksRaycasts = isActive);
    }
}
