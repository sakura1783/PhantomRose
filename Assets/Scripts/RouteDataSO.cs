using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RouteDataSO", menuName = "Create RouteDataSO")]
public class RouteDataSO : ScriptableObject
{
    public List<RouteData> routeList = new();

    [System.Serializable]
    public class RouteData
    {
        public List<EventBase> eventList = new();  //バトル、探索などのイベントを登録するリスト 
    }
}
