using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

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

    private string[] hidePopNames = new[] { "Store(Home)", "Inventory", "CardDeck", "PlayerState" };

    //TODO 変数追加


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        //TODO 各ボタンの設定
        btnMenu.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(1f))
            .Subscribe(_ => PopupManager.instance.Show<ExitPop>(false, false))
            .AddTo(this);

        btnReturn.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => PopupManager.instance.currentViewPop.Value.HidePopUp())
            .AddTo(this);

        btnToStore.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                // 現在開いているポップアップがストア、インベントリ、カード、状態のいずれかである場合、開かれている最新のポップアップを閉じる
                if (hidePopNames.Contains(PopupManager.instance.currentViewPop.Value.name))
                {
                    PopupManager.instance.Show<StorePop>(false);
                }
                // それ以外の(バトル用のポップアップが開かれている)場合、それらは閉じない
                else
                {
                    PopupManager.instance.Show<StorePop>(false, false);
                }
            })
            .AddTo(this);

        btnToInventory.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                if (hidePopNames.Contains(PopupManager.instance.currentViewPop.Value.name))
                {
                    PopupManager.instance.Show<InventoryPop>(false);
                }
                else
                {
                    PopupManager.instance.Show<InventoryPop>(false, false);
                }
            })
            .AddTo(this);

        btnToCards.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ =>
            {
                if (hidePopNames.Contains(PopupManager.instance.currentViewPop.Value.name))
                {
                    PopupManager.instance.Show<CardDeckPop>(false);
                }
                else
                {
                    PopupManager.instance.Show<CardDeckPop>(false, false);
                }
            })
            .AddTo(this);

        //btnToPlayerState
    }
}
