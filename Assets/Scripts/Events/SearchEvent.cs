using UnityEngine;
using Cysharp.Threading.Tasks;

public class SearchEvent : EventBase
{
    /// <summary>
    /// イベント処理
    /// </summary>
    /// <returns></returns>
    public async override UniTask ExecuteEvent()
    {
        // イベント用のポップアップを開く
        PopupBase pop = PopupManager.instance.Show<SearchEventPop>(false, false);

        // イベント終了を待つ
        await UniTask.WaitUntil(() => pop.CanvasGroup.alpha <= 0);

        Debug.Log($"{this} : 終了");
    }
}
