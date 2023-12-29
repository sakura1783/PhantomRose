using UnityEngine;
using UnityEngine.UI;

public class WaveInfoView : MonoBehaviour
{
    [SerializeField] private Text txtWaveNo;


    public void UpdateWaveNo(int no)
    {
        txtWaveNo.text = $"現在 {no}";
    }
}
