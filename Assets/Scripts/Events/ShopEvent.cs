using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShopEvent : EventBase
{
    /// <summary>
    /// イベント処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask ExecuteEvent()
    {
        // イベント用のポップアップを開く
        PopupManager.instance.Show<ShopEventPop>(true);

        await UniTask.DelayFrame(1);

        Debug.Log($"{this} 終了");
    }
}
