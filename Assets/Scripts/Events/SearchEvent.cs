using UnityEngine;
using Cysharp.Threading.Tasks;

public class SearchEvent : EventBase
{
    public async override UniTask ExecuteEvent()
    {
        //TODO サーチイベント用のポップアップを開く

        await UniTask.DelayFrame(1);

        Debug.Log($"{this} : 終了");
    }
}
