using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleEvent : EventBase
{
    /// <summary>
    /// 初期設定
    /// </summary>
    //public override void SetUp()
    //{

    //}

    public async override UniTask ExecuteEvent()
    {
        // TODO バトルイベント用のポップアップを開く

        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));

        Debug.Log($"{this} : 終了");
    }
}
