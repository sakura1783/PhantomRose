using UnityEngine;

public class ChallengeTaskPop : PopupBase
{
    [SerializeField] private ChallengeTaskDataSO challengeTaskDataSO;

    [SerializeField] private Transform taskTran;

    [SerializeField] private ChallengeTaskController challengeTaskPrefab;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();

        // タスクのゲームオブジェクトを生成
        foreach (var data in challengeTaskDataSO.challengeTaskDataList)
        {
            var task = Instantiate(challengeTaskPrefab, taskTran);
            task.SetUp(data);
        }
    }
}
