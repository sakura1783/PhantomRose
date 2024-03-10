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
        txtPlayerMaxHp.text = "/" + maxHp;
        playerSlider.value = maxHp;

        UpdatePlayerHp(maxHp);  // 第1引数を0にすると、0 → maxHpへのアニメができる
    }

    /// <summary>
    /// 対戦相手のHP設定
    /// </summary>
    /// <param name="maxHp"></param>
    public void SetUpOpponentHp(int maxHp)
    {
        txtOpponentMaxHp.text = "/" + maxHp;
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
        playerSlider.DOValue((float)currentHp / GameDataManager.instance.gameData.GetPlayer().MaxHp, animDuration).SetEase(animEase);
    }

    /// <summary>
    /// プレイヤーのシールド値の更新
    /// </summary>
    /// <param name="currentValue"></param>
    public void UpdatePlayerShieldValue(int currentValue)
    {
        // Canvasの表示設定
        SetCanvasAlpha(playerShieldGroup, currentValue);

        txtPlayerShieldValue.text = currentValue.ToString();
    }

    /// <summary>
    /// プレイヤーのバフの初期設定
    /// </summary>
    public void SetUpPlayerBuff()
    {
        SetCanvasAlpha(playerBuffGroup, 0);

        // バフの継続時間と画像の初期化
        imgPlayerBuff.sprite = null;
        UpdatePlayerBuffDuration(0);
    }

    /// <summary>
    /// プレイヤーのバフ更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdatePlayerBuff(SimpleStateData newStateData)
    {
        // カードに状態異常効果がない場合、リストの中身が空になる(データがない)ので、newStateDataがない場合は処理を行わない
        if (newStateData == null)
        {
            return;
        }

        SetCanvasAlpha(playerBuffGroup, newStateData.duration);

        UpdatePlayerBuffDuration(newStateData.duration);
        imgPlayerBuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// プレイヤーのバフ継続時間の更新
    /// </summary>
    /// <param name="duration"></param>
    public void UpdatePlayerBuffDuration(int duration)
    {
        txtPlayerBuffDuration.text = duration.ToString();
    }

    /// <summary>
    /// プレイヤーのデバフの初期設定
    /// </summary>
    public void SetUpPlayerDebuff()
    {
        SetCanvasAlpha(playerDebuffGroup, 0);

        UpdatePlayerDebuffDuration(0);
        imgPlayerDebuff.sprite = null;
    }

    /// <summary>
    /// プレイヤーのデバフ更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdatePlayerDebuff(SimpleStateData newStateData)
    {
        if (newStateData == null)
        {
            return;
        }

        SetCanvasAlpha(playerDebuffGroup, newStateData.duration);

        UpdatePlayerDebuffDuration(newStateData.duration);
        imgPlayerDebuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// プレイヤーのデバフ継続時間の更新
    /// </summary>
    /// <param name="duration"></param>
    public void UpdatePlayerDebuffDuration(int duration)
    {
        txtPlayerDebuffDuration.text = duration.ToString();
    }

    /// <summary>
    /// 対戦相手のHPの更新
    /// </summary>
    /// <param name="currentHp"></param>
    public void UpdateOpponentHp(int currentHp)
    {
        txtOpponentHp.text = currentHp.ToString();
        opponentSlider.DOValue((float)currentHp / GameDataManager.instance.gameData.GetOpponent().MaxHp, animDuration).SetEase(animEase);
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
    /// 対戦相手のバフの初期設定
    /// </summary>
    /// <param name="defaultDuration"></param>
    public void SetUpOpponentBuff()
    {
        SetCanvasAlpha(opponentBuffGroup, 0);

        UpdateOpponentBuffDuration(0);
        imgOpponentBuff.sprite = null;
    }

    /// <summary>
    /// 対戦相手のバフ更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdateOpponentBuff(SimpleStateData newStateData)
    {
        if (newStateData == null)
        {
            return;
        }

        SetCanvasAlpha(opponentBuffGroup, newStateData.duration);

        UpdateOpponentBuffDuration(newStateData.duration);
        imgOpponentBuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// 対戦相手のバフ継続時間の更新
    /// </summary>
    /// <param name="duration"></param>
    public void UpdateOpponentBuffDuration(int duration)
    {
        txtOpponentBuffDuration.text = duration.ToString();
    }

    /// <summary>
    /// 対戦相手のデバフの初期設定
    /// </summary>
    /// <param name="defaultDuration"></param>
    public void SetUpOpponentDebuff()
    {
        SetCanvasAlpha(opponentDebuffGroup, 0);

        UpdateOpponentDebuffDuration(0);
        imgOpponentDebuff.sprite = null;
    }

    /// <summary>
    /// 対戦相手のデバフ更新
    /// </summary>
    /// <param name="currentDuration"></param>
    public void UpdateOpponentDebuff(SimpleStateData newStateData)
    {
        if (newStateData == null)
        {
            SetUpOpponentDebuff();

            return;
        }

        SetCanvasAlpha(opponentDebuffGroup, newStateData.duration);

        UpdateOpponentDebuffDuration(newStateData.duration);
        imgOpponentDebuff.sprite = IconManager.instance.GetStateIcon(DataBaseManager.instance.stateDataSO.stateDataList[newStateData.stateId].spriteId);
    }

    /// <summary>
    /// 対戦相手のデバフ継続時間の更新
    /// </summary>
    /// <param name="duration"></param>
    public void UpdateOpponentDebuffDuration(int duration)
    {
        txtOpponentDebuffDuration.text = duration.ToString();
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
