using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDetailDataSO", menuName = "Create StageDetailDataSO")]
public class StageDetailDataSO : ScriptableObject
{
    public List<StageDetailData> stageDetailDataList = new();
}
