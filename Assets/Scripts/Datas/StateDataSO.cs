using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateDataSO", menuName = "Create StateDataSO")]
public class StateDataSO : ScriptableObject
{
    public List<StateData> stateDataList = new();
}
