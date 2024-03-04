using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpponentDataSO", menuName = "Create OpponentDataSO")]
public class OpponentDataSO : ScriptableObject
{
    public List<OpponentData> opponentDataList = new();
}
