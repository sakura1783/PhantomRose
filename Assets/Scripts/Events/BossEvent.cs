using UnityEngine;
using Cysharp.Threading.Tasks;

public class BossEvent : EventBase
{
    /// <summary>
    /// イベント処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask ExecuteEvent()
    {
        var pop = PopupManager.instance.Show<BossEncounterPop>(false, false);

        // TODO 変更。BattleEventPopが閉じるまで待つ
        await UniTask.WaitUntil(() => pop.CanvasGroup.alpha <= 0);

        Debug.Log($"{this} : 終了");
    }
}
