using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class FloatingMessage : MonoBehaviour
{
    [SerializeField] private Image imgEffect;

    [SerializeField] private Text txtEffectValue;


    /// <summary>
    /// 初期設定
    /// </summary>
    public async UniTask SetUp(Sprite effectImage, int effectValue)
    {
        imgEffect.sprite = effectImage;
        txtEffectValue.text = effectValue.ToString();

        // 画像がない場合は効果の量だけ表示
        if (!effectImage)
        {
            imgEffect.enabled = false;
        }

        // 生成位置が重ならないようにする
        transform.localPosition = new Vector2(transform.localPosition.x + Random.Range(-10, 30), transform.localPosition.y);

        // 1秒待つ
        await UniTask.Delay(1000);

        Destroy(gameObject);
    }
}
