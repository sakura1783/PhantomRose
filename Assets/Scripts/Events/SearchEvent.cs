using UnityEngine;
using Cysharp.Threading.Tasks;

public class SearchEvent : EventBase
{
    public async override UniTask ExecuteEvent()
    {
        // TODO サーチイベント用のポップアップウィンドウを開く

        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));

        Debug.Log($"{this} : 終了");
    }
}
