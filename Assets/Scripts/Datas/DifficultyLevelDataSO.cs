using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevelDataSO", menuName = "Create DifficultyLevelDataSO")]
public class DifficultyLevelDataSO : ScriptableObject
{
    public List<DifficultyLevelData> difficultyLevelDataList = new();


    [System.Serializable]
    public class DifficultyLevelData
    {
        public DifficultyType difficultyType;
        public List<StageDataSO> stageDataList = new();
    }
}
