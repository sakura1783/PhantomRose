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

    public int currentCoolTime;

    public bool IsCoolTime => currentCoolTime != 0;  // get専門のプロパティ


    /// <summary>
    /// 初期設定。カード見た目と効果を別々に設定
    /// </summary>
    public void SetUp(CardData data, DescriptionPop descriptionPop)
    {
        cardData = data;

        SetCardDetail(data);

        // リフレクションを利用し、カードから同名のカード効果を持つクラス・インスタンスを生成
        cardEffect = CardEffectFactory.CreateCardEffect(cardData);

        // ボタンの購読処理
        btnCard.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => descriptionPop.ShowPopUp(data))
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
        txtAttackPointOrInterval.text = data.attackPower <= 0 ? "" : data.attackPower.ToString();  //TODO 回復値、攻撃値、シールド値が別々に設定されているため、どうするか検討する。
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
    public void SetCoolTime()
    {
        currentCoolTime = cardData.coolTime;

        Debug.Log($"{cardData.name} : {currentCoolTime}");
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

        if (currentCoolTime == 0)
        {
            // もう一度このカードを選べるようにする
            SetSelectable(false);
        }

        Debug.Log($"クールタイム更新{cardData.name} : {currentCoolTime}");

        // TODO カードの絵を変える、クールタイム表示の更新
    }
}
