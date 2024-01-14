using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PopupChangeTester : MonoBehaviour
{
    void Start()
    {
        // キーボードのPボタンを押したらStoreGoodsPopを表示する
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.P))
            .Subscribe(_ => PopupManager.instance.Show<StoreGoodsPop>());

        // Oボタンを押したらCardDescriptionPopを表示
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.O))
            .Subscribe(_ => PopupManager.instance.Show<CardDescriptionPop>());

        //Iボタンを押したらTitlePopを表示
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.I))
            .Subscribe(_ => PopupManager.instance.Show<TitlePop>());
    }
}
