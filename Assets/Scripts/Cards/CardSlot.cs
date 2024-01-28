using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class CardSlot : MonoBehaviour
{
    public int slotIndex;

    public OwnerStatus owner;

    public CardController cardController;

    public IObservable<Unit> OnClickAsObservable => btnSlot.OnClickAsObservable();  // ただ公開させるだけ

    [SerializeField] private Button btnSlot;

    private Vector2 size;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="slotIndex"></param>
    public void SetUp(int slotIndex)
    {
        this.slotIndex = slotIndex;

        // TODO デバッグ用
        owner = OwnerStatus.Player;
    }

    /// <summary>
    /// カードをスロットに配置
    /// </summary>
    /// <param name="newCard"></param>
    public void SetCard(CardController newCard)
    {
        // 選択されているカードを複製し、スロットと親子関係にする
        CardController cardClone = Instantiate(newCard, transform);

        if (size == Vector2.zero)
        {
            size = GetComponent<RectTransform>().sizeDelta;

            Debug.Log($"size : {size}");
        }

        // 配置したカードの位置とサイズを調整
        if (cardClone.TryGetComponent(out RectTransform rectTransform))
        {
            rectTransform.pivot = Vector2.zero;
            rectTransform.sizeDelta = size;
        }

        // ボタンの無効化
        //TODO cardClone.SetInactive();

        // Cloneされた新しいオブジェクトはClone元の参照情報が失われるので、再度設定する
        //TODO cardClone.CardEffect = newCard.CardEffect;

        cardController = cardClone;
    }

    /// <summary>
    /// 生成されたクローンのカードを削除
    /// </summary>
    public void DeleteCard()
    {
        Destroy(cardController.gameObject);

        cardController = null;
    }

    /// <summary>
    /// スロットに配置できる対象先の変更
    /// </summary>
    /// <param name="newOwner"></param>
    public void ChangeStatus(OwnerStatus newOwner)
    {
        owner = newOwner;
    }
}
