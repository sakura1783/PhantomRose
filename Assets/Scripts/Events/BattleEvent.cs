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
        // イベント用のポップアップを開く
        PopupBase pop = PopupManager.instance.Show<BattleEventPop>(false);

        // イベント終了(Canvasが閉じる)を待つ
        await UniTask.WaitUntil(() => pop.CanvasGroup.alpha <= 0);

        Debug.Log($"{this}終了");
    }
}
