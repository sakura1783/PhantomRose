using UnityEngine;
using UnityEngine.UI;

public class StateDescriptionPopController : MonoBehaviour
{
    [SerializeField] private Image imgState;

    [SerializeField] private Text txtStateName;
    [SerializeField] private Text txtBuffOrDebuff;
    [SerializeField] private Text txtDescription;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="data"></param>
    public void SetUp(StateData data)
    {
        // 各値の設定
        imgState.sprite = IconManager.instance.GetStateIcon(data.spriteId);
        txtStateName.text = data.stateName + " 状態";
        txtBuffOrDebuff.text = data.stateType.ToString();
        txtBuffOrDebuff.color = data.stateType == StateType.バフ ? ColorManager.instance.GetColor(ColorType.Cyan) : ColorManager.instance.GetColor(ColorType.Magenta);
        txtDescription.text = data.description;
    }
}
