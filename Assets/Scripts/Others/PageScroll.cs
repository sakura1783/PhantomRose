using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// ScrollRectを改造して、次のページに吸着するように自動スクロールさせる
/// なので、ScrollViewのScrollRectはRemoveして、このクラスをアタッチする
/// </summary>
public class PageScroll : ScrollRect
{
    private float pageWidth = 1015;  // 1ページの幅 (現在のページの中心から次のページの中心の長さ)

    private int currentPageIndex;  // 現在のページのindex。最も左を0とする


    /// <summary>
    /// ドラッグを開始したとき
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    /// <summary>
    /// ドラッグを終了したとき
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        // スナップさせるページが決まった後も慣性が効いてしまうので、ドラッグを終了したとき、スクロールを止める
        //StopMovement();

        // スナップさせるページを決定する
        int pageIndex = Mathf.RoundToInt(content.anchoredPosition.x / pageWidth);  // RoundToInt(float f)で、fに最も近い整数を返す。

        // ページが変わっていない、かつ、素早くドラッグした場合
        if (pageIndex == currentPageIndex && Mathf.Abs(eventData.delta.x) >= 5)  // PointEventData.deltaは、最後の更新からのデルタポインタ
        {
            pageIndex += (int)Mathf.Sign(eventData.delta.x);  // Mathf.Sign(float f)で、fの符号を返す。fが正か0の場合1を、負の場合-1を返す。
        }

        // スクロール位置を決定する
        float destX = pageIndex * pageWidth;
        //content.anchoredPosition = new Vector2(destX, content.anchoredPosition.y);
        content.DOAnchorPosX(destX, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() => currentPageIndex = pageIndex);
    }
}
