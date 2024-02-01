using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BossEncounterPop : PopupBase
{
    [SerializeField] private Button btnAssault;
    [SerializeField] private Button btnFight;

    [SerializeField] private int assaultCost;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        // TODO 監視処理
        btnAssault.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // HPを満タン回復

                // 敵のHPを10減らす

                PopupManager.instance.Show<BattleEventPop>(false);
            })
            .AddTo(this);

        btnFight.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // HPを20回復

                PopupManager.instance.Show<BattleEventPop>(false);
            })
            .AddTo(this);
    }
}
