using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDescriptionPop : PopupBase
{
    [SerializeField] private Image imgCard;

    [SerializeField] private Text txtCardType;
    [SerializeField] private Text txtCardLevel;
    [SerializeField] private Text txtCardName;
    [SerializeField] private Text txtCardDescription;


    //TODO ポップアップを開く処理は、カードのプレハブ側で行う。ここでは、Canvas下への生成と、ポップアップの設定だけ行う。

    //TODO ポップアップ生成時、カード側から作る個数とそのカードの情報を渡してもらう。

    protected override void Reset()
    {
        if (transform.parent.TryGetComponent(out canvas))
        {
            Debug.Log($"{canvas} 取得しました");
        }
        else
        {
            Debug.Log($"{this} Canvasが取得できません");
        }

        if (transform.parent.TryGetComponent(out canvasGroup))
        {
            Debug.Log($"{canvasGroup} 取得しました");
        }
        else
        {
            Debug.Log($"{this} CanvasGroupが取得できません");
        }
    }

    private void GeneratePop()
    {

    }

    private void SetCardDetail()
    {

    }
}
