using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeTaskDataSO", menuName = "Create ChallengeTaskDataSO")]
public class ChallengeTaskDataSO : ScriptableObject
{
    public List<ChallengeTaskData> challengeTaskDataList = new();


    [System.Serializable]
    public class ChallengeTaskData
    {
        public int id;

        public int rewardDiamondCount;
        public string name;
        [Multiline] public string detail;
        public Sprite taskSprite;
    }
}
