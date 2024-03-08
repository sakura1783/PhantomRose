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
        public string difficultyName;
        public List<StageDataSO> stageDataList = new();

        // 難易度クリア後にもらえる報酬
        public int diamond;
        public int purpleGem;
        public int goldGem;
        public int diamondGem;
        public int rubyGem;
    }
}
