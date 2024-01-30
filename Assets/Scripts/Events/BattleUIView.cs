using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUIView : MonoBehaviour
{
    [SerializeField] private Text txtPlayerHp;
    [SerializeField] private Text txtOpponentHp;
    // TODO 最大HPのText追加

    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider opponentSlider;

    [SerializeField] private float animDuration;

    [SerializeField] private Ease animEase;


    /// <summary>
    /// プレイヤーのHP設定
    /// </summary>
    /// <param name="maxHp"></param>
    public void SetUpPlayerHp(int maxHp)
    {
        playerSlider.value = maxHp;
        UpdatePlayerHp(maxHp);  // 第1引数を0にすると、0 → maxHpへのアニメができる
    }

    /// <summary>
    /// 対戦相手のHP設定
    /// </summary>
    /// <param name="maxHp"></param>
    public void SetUpOpponentHp(int maxHp)
    {
        opponentSlider.value = maxHp;
        UpdateOpponentHp(maxHp);
    }

    /// <summary>
    /// プレイヤーのHPの更新
    /// </summary>
    /// <param name="currentHp"></param>
    public void UpdatePlayerHp(int currentHp)
    {
        txtPlayerHp.text = currentHp.ToString();
        playerSlider.DOValue(currentHp, animDuration).SetEase(animEase);
    }

    /// <summary>
    /// 対戦相手のHPの更新
    /// </summary>
    /// <param name="currentHp"></param>
    public void UpdateOpponentHp(int currentHp)
    {
        txtOpponentHp.text = currentHp.ToString();
        opponentSlider.DOValue(currentHp, animDuration).SetEase(animEase);
    }
}
