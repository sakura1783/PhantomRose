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
        //TODO PopupManager.instance.Show<BattleEventPop>(false);

        await UniTask.DelayFrame(1);

        Debug.Log($"{this}終了");
    }
}
