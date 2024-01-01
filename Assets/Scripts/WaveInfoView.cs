using UnityEngine;
using UnityEngine.UI;

public class WaveInfoView : MonoBehaviour
{
    [SerializeField] private Text txtWaveNo;


    /// <summary>
    /// ウェーブの表示を更新
    /// </summary>
    /// <param name="no"></param>
    public void UpdateWaveNo(int no)
    {
        txtWaveNo.text = $"現在 {no}";
    }
}
