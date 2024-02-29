using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGalleryPop : PopupBase
{
    [SerializeField] private Transform cardTran;

    [SerializeField] private CardController cardPrefab;

    [SerializeField] private GameObject nullCardPrefab;


    // 全カード - 発見したカード の数だけ、カードの枠を作る

    // 発見したカード の数だけ、カードのプレハブを生成。

    // カードのプレハブの位置を、id番号にしたがって、並び替え


    // 新しく発見したカードを追加する処理
}
