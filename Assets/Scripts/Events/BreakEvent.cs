using UnityEngine;
using Cysharp.Threading.Tasks;

public class BreakEvent : EventBase
{
    /// <summary>
    /// イベント処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask ExecuteEvent()
    {
        var pop = PopupManager.instance.Show<BreakEventPop>(false, false);

        await UniTask.WaitUntil(() => pop.CanvasGroup.alpha <= 0);

        Debug.Log($"{this} : 終了");
    }
}
