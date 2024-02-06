using System;
using UnityEngine;

/// <summary>
/// CardEffectBaseを継承しているカードの効果用サブクラスを生成するファクトリークラス
/// </summary>
public static class CardEffectFactory  // staticクラスなので、ゲーム実行時に、見えないが自動生成される。どのクラスからでもアクセスできる
{
    /// <summary>
    /// カードの効果を作成
    /// </summary>
    /// <param name="cardData"></param>
    /// <returns></returns>
    public static CardEffectBase CreateCardEffect(CardData cardData)
    {
        // stringからTypeを取得(nameSpaceがある場合には、nameSpaceからの正式名称でクラス名を指定する)
        Type type = Type.GetType(cardData.englishName);

        // クラスが見つからないか、CardEffectBaseを継承していない場合
        if (type == null || !typeof(CardEffectBase).IsAssignableFrom(type))
        {
            Debug.Log("CardEffect null");
            return null;
        }

        // Typeからインスタンスを生成。第2引数には、コンストラクタ用の引数を指定できる(デフォルトのコンストラクタ不要でエラーも出ない)
        CardEffectBase cardEffect = (CardEffectBase)Activator.CreateInstance(type, cardData);

        return cardEffect;
    }
}
