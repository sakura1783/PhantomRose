using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUIView : MonoBehaviour
{
    // HP
    [SerializeField] private Text txtPlayerHp;
    [SerializeField] private Text txtPlayerMaxHp;
    [SerializeField] private Text txtOpponentHp;
    [SerializeField] private Text txtOpponentMaxHp;

    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider opponentSlider;

    // シールド値
    [SerializeField] private Text txtPlayerShieldValue;
    [SerializeField] private Text txtOpponentShieldValue;

    [SerializeField] private CanvasGroup playerShieldGroup;
    [SerializeField] private CanvasGroup opponentShieldGroup;

    // バフ、デバフ
    [SerializeField] private Text txtPlayerBuffDuration;
    [SerializeField] private Text txtPlayerDebuffDuration;
    [SerializeField] private Text txtOpponentBuffDuration;
    [SerializeField] private Text txtOpponentDebuffDuration;

    [SerializeField] private Image imgPlayerBuff;
    [SerializeField] private Image imgPlayerDebuff;
    [SerializeField] private Image imgOpponentBuff;
    [SerializeField] private Image imgOpponentDebuff;

    [SerializeField] private CanvasGroup playerBuffGroup;
    [SerializeField] private CanvasGroup playerDebuffGroup;
    [SerializeField] private CanvasGroup opponentBuffGroup;
    [SerializeField] private CanvasGroup opponentDebuffGroup;

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

    /// <summary>
    /// プレイヤーのシールド値の初期設定
    /// </summary>
    /// <param name="defaultValue"></param>
    public void SetUpPlayerShieldValue(int defaultValue)
    {
        // Canvasの表示設定
        SetCanvasAlpha(playerShieldGroup, defaultValue);

        txtPlayerShieldValue.text = defaultValue.ToString();
    }

    /// <summary>
    /// プレイヤーのシールド値の更新
    /// </summary>
    /// <param name="currentValue"></param>
    public void UpdatePlayerShieldValue(int currentValue)
    {
        SetCanvasAlpha(playerShieldGroup, currentValue);

        txtPlayerShieldValue.text = currentValue.ToString();
    }

    /// <summary>
    /// 対戦相手のシールド値の初期設定
    /// </summary>
    /// <param name="defaultValue"></param>
    public void SetUpOpponentShieldValue(int defaultValue)
    {
        SetCanvasAlpha(opponentShieldGroup, defaultValue);

        txtOpponentShieldValue.text = defaultValue.ToString();
    }

    /// <summary>
    /// 対戦相手のシールド値の更新
    /// </summary>
    /// <param name="currentValue"></param>
    public void UpdateOpponentShieldValue(int currentValue)
    {
        SetCanvasAlpha(opponentShieldGroup, currentValue);

        txtOpponentShieldValue.text = currentValue.ToString();
    }

    /// <summary>
    /// プレイヤーのバフの初期設定
    /// </summary>
    public void SetUpPlayerBuff()
    {
        SetCanvasAlpha(playerBuffGroup, 0);

        txtPlayerBuffDuration.text = "0";
        imgPlayerBuff.sprite = null;
    }

    /// <summary>
    /// プレイヤーのバフの更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdatePlayerBuff(SimpleStateData newStateData)
    {
        SetCanvasAlpha(playerBuffGroup, newStateData.duration);

        txtPlayerBuffDuration.text = newStateData.duration.ToString();
        imgPlayerBuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// プレイヤーのデバフの初期設定
    /// </summary>
    public void SetUpPlayerDebuff()
    {
        SetCanvasAlpha(playerDebuffGroup, 0);

        txtPlayerDebuffDuration.text = "0";
        imgPlayerDebuff.sprite = null;
    }

    /// <summary>
    /// プレイヤーのデバフ継続時間の更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdatePlayerDebuff(SimpleStateData newStateData)
    {
        SetCanvasAlpha(playerDebuffGroup, newStateData.duration);

        txtPlayerDebuffDuration.text = newStateData.duration.ToString();
        imgPlayerDebuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// 対戦相手のバフ継続時間の初期設定
    /// </summary>
    /// <param name="defaultDuration"></param>
    public void SetUpOpponentBuff()
    {
        SetCanvasAlpha(opponentBuffGroup, 0);

        txtOpponentBuffDuration.text = "0";
        imgOpponentBuff.sprite = null;
    }

    /// <summary>
    /// 対戦相手のバフ継続時間の更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdateOpponentBuff(SimpleStateData newStateData)
    {
        SetCanvasAlpha(opponentBuffGroup, newStateData.duration);

        txtOpponentBuffDuration.text = newStateData.duration.ToString();
        imgOpponentBuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// 対戦相手のデバフ継続時間の初期設定
    /// </summary>
    /// <param name="defaultDuration"></param>
    public void SetUpOpponentDebuff()
    {
        SetCanvasAlpha(opponentDebuffGroup, 0);

        txtOpponentDebuffDuration.text = "0";
        imgOpponentDebuff.sprite = null;
    }

    /// <summary>
    /// 対戦相手のデバフ継続時間の更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdateOpponentDebuff(SimpleStateData newStateData)
    {
        SetCanvasAlpha(opponentDebuffGroup, newStateData.duration);

        txtOpponentDebuffDuration.text = newStateData.duration.ToString();
        imgOpponentDebuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// CanvasGroupのAlpha値を設定
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="value"></param>
    private void SetCanvasAlpha(CanvasGroup canvasGroup, int value)
    {
        // valueの値が0以下(シールドやバフなどがない)場合、Canvasを非表示にする。シールドやバフがある場合、Canvasを表示する
        canvasGroup.alpha = (value <= 0) ? 0 : 1;
    }
}
