using UnityEngine;
using UnityEngine.UI;

public class ChallengeTaskController : MonoBehaviour
{
    [SerializeField] private Image imgTask;

    [SerializeField] private Image imgBackground;

    [SerializeField] private Text txtReward;
    [SerializeField] private Text txtTaskName;
    [SerializeField] private Text txtTaskDetail;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="data"></param>
    public void SetUp(ChallengeTaskDataSO.ChallengeTaskData data)
    {
        // 各値の設定
        imgTask.sprite = data.taskSprite;
        txtReward.text = data.rewardDiamondCount.ToString();
        txtTaskName.text = data.name;
        txtTaskDetail.text = data.detail;

        // 達成しているタスクであれば
        if (GameData.instance.achievedChallengeTaskList.Contains(data.id))
        {
            // 背景色の設定
            imgBackground.color = ColorManager.instance.GetColor(ColorType.Brown);
        }
    }
}
