using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Create StageDataSO")]
public class StageDataSO : ScriptableObject
{
    // 各難易度ごとのSilver-1、Silver-2といった枝部分のデータ
    public List<StageData> stageDataList = new();


    [System.Serializable]
    public class StageData
    {
        // ステージ内のルートデータ (1-1、1-2といった、枝部分のデータ)
        public List<RouteDataSO> routeDataSOList = new();
    }
}
