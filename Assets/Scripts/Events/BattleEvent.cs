using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleEvent : EventBase
{
    /// <summary>
    /// イベント処理
    /// </summary>
    /// <returns></returns>
    public async override UniTask ExecuteEvent()
    {
        PopupBase pop = PopupManager.instance.Show<BattleEventPop>(false);

        // TODO イベント終了(Canvasが閉じる)を待つ
        await UniTask.WaitUntil(() => pop.canvasGroup.alpha == 0);  // TODO きちんと動くか確認する。PopupBase修正する

        Debug.Log($"{this}終了");
    }
}
