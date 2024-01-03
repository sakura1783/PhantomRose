using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CardPrefab : MonoBehaviour
{
    [SerializeField] private Button cardPrefab;

    [SerializeField] private Image imgCard;

    [SerializeField] private Text txtAttackPointOrInterval;


    //TODO カードの情報を取得して、何のポップアップを何個用意すればいいか確認する。

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUp()
    {
        //cardPrefab.OnClickAsObservable()
        //    .ThrottleFirst(System.TimeSpan.FromSeconds(2))
        //    .Subscribe(_ => )
        //    .AddTo(this);
    }
}
