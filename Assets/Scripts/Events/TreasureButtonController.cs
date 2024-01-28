using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;

public class TreasureButtonController : MonoBehaviour
{
    [SerializeField] private Button btnTreasure;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="action"></param>
    public void SetUp(UnityAction<TreasureType> buttonAction, TreasureType treasureType)
    {
        btnTreasure.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ => buttonAction?.Invoke(treasureType))
            .AddTo(this);
    }
}
