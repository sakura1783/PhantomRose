using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// カード用のプレハブにアタッチし、カードのゲームオブジェクト自体の管理をする。(カードの効果とは別)
/// </summary>
public class CardController : MonoBehaviour
{
    [SerializeField] private Button btnCard;

    [SerializeField] private Image imgCard;
    [SerializeField] private Image imgBase;  //カードの色変更用

    [SerializeField] private Text txtAttackPointOrInterval;

    [SerializeField] private CanvasGroup disabledColorGroup;

    [SerializeField] private Transform cardImageTran;
    [SerializeField] private Transform attackPointOrIntervalTextTran;

    public IObservable<Unit> OnClickAsObservable => btnCard.OnClickAsObservable();

    public ReactiveProperty<bool> IsSelectable = new();

    private ICommand cardEffect;  // クローンすると失われる
    public ICommand CardEffect
    {
        get => cardEffect;
        set => cardEffect = value;
    }

    private CardData cardData;
    public CardData CardData => cardData;

    private int currentCoolTime;
    public int CurrentCoolTime => currentCoolTime;

    //public bool IsCoolTime => currentCoolTime != 0;  // get専門のプロパティ


    /// <summary>
    /// 初期設定。カード見た目と効果を別々に設定
    /// </summary>
    public void SetUp(CardData data, DescriptionPop descriptionPop)
    {
        cardData = data;

        SetCardDetail(data);

        // リフレクションを利用し、カードから同名のカード効果を持つクラス・インスタンスを生成
        cardEffect = CardEffectFactory.CreateCardEffect(cardData);

        // 購読処理
        btnCard.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => descriptionPop.ShowPopUp(data))
            .AddTo(this);

        cardData.AttackPower
            .Subscribe(value => DisplayAttackPower(value))  // 攻撃力が変更された時、表示を更新
            .AddTo(this);
    }

    /// <summary>
    /// カードに情報を設定する
    /// </summary>
    private void SetCardDetail(CardData data)
    {
        // 各値を設定
        imgCard.sprite = IconManager.instance.GetCardIcon(data.spriteId);
        imgBase.color = data.cardColor;
        DisplayAttackPower(data.AttackPower.Value);  //TODO 置く場所や状況によって攻撃力に変動があるものは 4+ などのように表示する
    }

    /// <summary>
    /// カード選択中かどうかの切り替え
    /// </summary>
    /// <param name="selectable"></param>
    public void SetSelectable(bool selectable)
    {
        IsSelectable.Value = selectable;
    }

    /// <summary>
    /// ボタンの無効化
    /// </summary>
    public void SetInactive()
    {
        btnCard.enabled = false;
    }

    /// <summary>
    /// クールタイムの設定
    /// カードをスロットにセットした後に実行させる
    /// </summary>
    public void SetCoolTime(int value)
    {
        //currentCoolTime = cardData.coolTime;
        currentCoolTime = value;

        // カードにクールタイムを表示
        DisplayCoolTime(true);
    }

    /// <summary>
    /// クールタイムの更新
    /// </summary>
    public void UpdateCoolTime()
    {
        // クールタイムが0なら、処理しない
        if (currentCoolTime == 0)
        {
            return;
        }

        currentCoolTime--;
        txtAttackPointOrInterval.text = currentCoolTime.ToString();

        if (currentCoolTime == 0)
        {
            // もう一度このカードを選べるようにする
            SetSelectable(false);

            // カードに攻撃力を表示
            DisplayCoolTime(false);
        }
    }

    /// <summary>
    /// カードにクールタイムを表示
    /// </summary>
    /// <param name="isCoolTime"></param>
    private void DisplayCoolTime(bool isCoolTime)
    {
        if (isCoolTime)
        {
            // カードの色を暗くして、クールタイム中であることを示す
            disabledColorGroup.alpha = 1;

            // クールタイムを上に、カードの画像を下に移動
            cardImageTran.localPosition = new Vector3(0f, -30.5f, 0f);
            attackPointOrIntervalTextTran.localPosition = new Vector3(0f, 53f, 0f);

            // クールタイムを表示
            txtAttackPointOrInterval.text = currentCoolTime.ToString();
        }
        else
        {
            disabledColorGroup.alpha = 0;

            cardImageTran.localPosition = new Vector3(0f, 30.5f, 0f);
            attackPointOrIntervalTextTran.localPosition = new Vector3(0f, -53f, 0f);

            // 攻撃力を表示
            DisplayAttackPower(cardData.AttackPower.Value);
        }
    }

    /// <summary>
    /// UIのTextに攻撃力を表示
    /// </summary>
    private void DisplayAttackPower(int attackPower)
    {
        txtAttackPointOrInterval.text = attackPower <= 0 ? "" : attackPower.ToString();
    }
}
