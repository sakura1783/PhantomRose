using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private Transform routeBasePrefab;
    [SerializeField] private Transform routeBaseSetTran;
    [SerializeField] private Transform playerIcon;
    [SerializeField] private Transform eventButtonTran;

    [SerializeField] private RouteDetail routeDetailPrefab;

    [SerializeField] private RouteDataSO routeDataSO;

    [SerializeField] private WaveInfoView waveInfoView;

    private List<Transform> routeList = new();

    private List<EventBase> currentEventList = new();

    //private int currentRouteIndex = 0; 
    public ReactiveProperty<int> CurrentRouteIndex = new(0);  // ReactivePropertyで監視できるようになる。プロパティは参照型なので最初に初期値を代入する。


    void Start()
    {
        LoadRouteDatas();

        GenerateEventButtons();

        //監視。Startに1回書けば良い
        CurrentRouteIndex.Subscribe(value => waveInfoView.UpdateWaveNo(value)).AddTo(this);
    }

    /// <summary>
    /// ルート作成
    /// </summary>
    private void LoadRouteDatas()
    {
        for (int i = 0; i < routeDataSO.routeList.Count; i++)
        {
            // ルート配置用のベース作成
            Transform routeBase = Instantiate(routeBasePrefab, routeBaseSetTran, false);

            // デバッグ。ボタン配置
            //for (int eventCount = 0; eventCount < routeDataSO.routeList[i].eventList.Count; eventCount++)
            //{
            //    EventBase eventData = Instantiate(routeDataSO.routeList[i].eventList[eventCount], eventSet.transform, false);
            //}

            routeList.Add(routeBase);

            //int eventCount = routeDataSO.routeList[i].eventList.Count;

            //// ルート内のイベント分だけマスを配置
            //for (int count = 0; count < eventCount; count++)
            //{
            //    GameObject routeBrunch = Instantiate(routeDetailPrefab, routeBase, false);

            //    // イベントが1つしかない場合には、マスの縦方向を広げる
            //    if (eventCount == 1)
            //    {
            //        routeBrunch.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 240);
            //    }
            //}

            // 上の処理をリファクタリング
            int index = 0;
            RouteDetail routeBrunch = null;

            // ルート内のイベント分だけマスを配置
            for (int eventCount = 0; eventCount < routeDataSO.routeList[i].eventList.Count; eventCount++)
            {
                index = eventCount;

                routeBrunch = Instantiate(routeDetailPrefab, routeBase, false);

                IconType iconType = (IconType)Enum.Parse(typeof(IconType), routeDataSO.routeList[i].eventList[index].name);  // Enum.Parse(列挙型の型情報, 変換したい文字列)

                routeBrunch.SetUp(IconManager.instance.GetIcon(iconType));
            }

            // イベントが1つしかない場合には、マスの縦方向を広げる
            if (index == 0)
            {
                // アイコンとフレームを表示する場合には利用する。(アイコンだけ設定する場合には不要)
                routeBrunch.GetComponent<RectTransform>().sizeDelta = new(85, 200);
                routeBrunch.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new(75, 190);

                // 代わりにアイコンを中央に寄せる
                //routeBrunch.transform.parent.GetComponent<VerticalLayoutGroup>().padding.top = 50;
            }
        }
    }

    /// <summary>
    /// ゲーム進行用のルート分岐ボタンの作成
    /// </summary>
    private void GenerateEventButtons()
    {
        // 次のイベント用のボタン生成
        for (int i = 0; i < routeDataSO.routeList[CurrentRouteIndex.Value].eventList.Count; i++)
        {
            int index = i;
            EventBase eventButton = Instantiate(routeDataSO.routeList[CurrentRouteIndex.Value].eventList[i], eventButtonTran, false);

            // ボタンのイベントを購読
            eventButton.OnClickEventButtonObserbable
                .ThrottleFirst(TimeSpan.FromSeconds(2f))  // ThrottleFirstで、指定された時間内に最初の要素のみを通過させ、それ以後の要素は無視する(ボタン連打防止を実現できる)
                .Subscribe(async _ =>  // Subscribeは、Observableに対して、何らかのイベントが発生した時の処理を指定するためのメソッド。
                {
                    // プレイヤーのアイコンの位置設定(子オブジェクトにする)
                    SetPlayerLocation(routeList[CurrentRouteIndex.Value].GetChild(index));

                    await eventButton.ExecuteEvent();

                    HandleEventCompletion(index);
                })
                .AddTo(this);  // AddToで監視処理を止める条件を引数で指定する

            currentEventList.Add(eventButton);
        }
    }

    /// <summary>
    /// プレイヤーアイコンの配置位置の更新
    /// </summary>
    /// <param name="nextParentObj"></param>
    private void SetPlayerLocation(Transform nextParentObj)
    {
        playerIcon.SetParent(nextParentObj);
        playerIcon.localPosition = new(0, 50, 0);

        //TODO マスの色を変える(UIごと差し替えなどの対応も可能)
        nextParentObj.GetComponent<RouteDetail>().ChangeRouteColor(Color.red);
    }

    /// <summary>
    /// イベント終了後の処理
    /// </summary>
    /// <param name="index"></param>
    private void HandleEventCompletion(int index)
    {
        DestroyEndEvents();

        CurrentRouteIndex.Value++;

        CheckRoute();
    }

    /// <summary>
    /// 分岐用ボタンの削除
    /// </summary>
    private void DestroyEndEvents()
    {
        // イベント用のボタン削除
        for (int i = 0; i < currentEventList.Count; i++)
        {
            Destroy(currentEventList[i].gameObject);
        }

        // 前のイベントを削除
        currentEventList.Clear();
    }

    /// <summary>
    /// ルートが残っているかチェックする
    /// </summary>
    private void CheckRoute()
    {
        // ルートが残っていない場合、ルートクリア
        if (routeDataSO.routeList.Count <= CurrentRouteIndex.Value)
        {
            Debug.Log("ルート終了");

            return;
        }

        // ルートが残っている場合、次の分岐用ボタンを作成
        GenerateEventButtons();
    }
}
