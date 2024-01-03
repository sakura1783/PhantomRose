using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UniRx;

/// <summary>
/// バトル、探索、ショップ、レベルアップなどのイベント全般に利用する抽象クラス
/// </summary>
public abstract class EventBase : MonoBehaviour, IEvent
{
    public abstract UniTask ExecuteEvent();

    [SerializeField] private Button btnEvent;
    public IObservable<Unit> OnClickEventButtonObserbable => btnEvent.OnClickAsObservable();  //ボタンがクリックされた時に通知を受け取るプロパティ。OnClickAsObservableでボタンのクリックイベントをIObservable<Unit>に変換する。

    /// <summary>
    /// 初期設定
    /// </summary>
    //public abstract void SetUp();
}
