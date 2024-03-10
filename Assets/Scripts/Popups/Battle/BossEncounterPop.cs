using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BossEncounterPop : PopupBase
{
    [SerializeField] private Button btnAssault;
    [SerializeField] private Button btnFight;

    [SerializeField] private Text txtAssaultCost;

    [SerializeField] private int assaultCost;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        txtAssaultCost.color = (GameDataManager.instance.gameData.RubyCount.Value >= assaultCost) ? ColorManager.instance.GetColor(ColorType.LightRed) : ColorManager.instance.GetColor(ColorType.Red);

        // TODO 監視処理
        btnAssault.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // ルビー支払い
                GameDataManager.instance.gameData.RubyCount.Value -= assaultCost;

                // HPを満タン回復
                GameDataManager.instance.gameData.GetPlayer().Hp.Value = GameDataManager.instance.gameData.GetPlayer().MaxHp;

                // 敵のHPを10減らす

                PopupManager.instance.Show<BattleEventPop>(false);
            })
            .AddTo(this);

        btnFight.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // HPを20回復
                GameDataManager.instance.gameData.GetPlayer().Hp.Value += 20;

                PopupManager.instance.Show<BattleEventPop>(false);
            })
            .AddTo(this);
    }
}
