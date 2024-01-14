using UnityEngine;
using UnityEngine.UI;

public class StateDescriptionPop : PopupBase
{
    [SerializeField] private Image imgState;

    [SerializeField] private Text txtStateName;
    [SerializeField] private Text txtStateBuffOrDebuff;
    [SerializeField] private Text txtStateDescription;


    //TODO デバフの場合は赤、バフの場合は青に文字の色を設定
}
