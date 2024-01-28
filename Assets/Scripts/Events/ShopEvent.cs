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
        PopupBase pop = PopupManager.instance.Show<ShopEventPop>(true);

        // イベント終了を待つ
        await UniTask.WaitUntil(() => pop.CanvasGroup.alpha <= 0);

        Debug.Log($"{this} 終了");
    }
}
