using UnityEngine;
using UnityEngine.UI;

public class FloatingMessage : MonoBehaviour
{
    [SerializeField] private Image imgEffect;

    [SerializeField] private Text txtEffectValue;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="effectValue"></param>
    /// <param name="spriteId"></param>
    /// <param name="childCount">親の下に生成されているフロート表示の数</param>
    public void SetUp(int effectValue, int spriteId, int childCount)
    {
        if (spriteId >= 0)
        {
            // 画像がある場合だけ設定
            imgEffect.sprite = IconManager.instance.GetStateIcon(spriteId);
        }
        txtEffectValue.text = effectValue.ToString();

        // 画像がない場合は効果の量だけ表示
        if (spriteId < 0)
        {
            imgEffect.enabled = false;
        }

        // 生成位置が重ならないようにする
        transform.localPosition = new Vector2(transform.localPosition.x + Random.Range(-10, 10), transform.localPosition.y + 70 * (childCount - 1));

        Destroy(gameObject, 1f);
    }
}
