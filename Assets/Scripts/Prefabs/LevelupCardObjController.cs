using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class LevelupCardObjController : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private CardController card;
    public CardController Card => card;

    private int serialNo;  // カードと同じ通し番号をここでも保持しておく
    public int SerialNo
    {
        get => serialNo;
        set => serialNo = value;
    }

    private LevelupDetailPop levelupDetailPop;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="cardData"></param>
    public void SetUp(CardData cardData, CardLevelUpPop cardLevelupPop)
    {
        levelupDetailPop = cardLevelupPop.LevelupDetailPop;

        // カードの見た目を設定
        card.SetCardDetail(cardData);

        // 強化の詳細ポップアップを開く
        button.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                //PopupManager.instance.Show<LevelupDetailPop>(true, false);  <= これだとCardDataの情報が渡せない
                levelupDetailPop.ShowLevelupDetailPopUp(cardData, serialNo);  // CardDataの情報を渡す

                PopupManager.instance.History.Push(PopupManager.instance.CurrentViewPop);  // Historyに追加してLevelupDetailPop内で戻るボタンを押したとき、CardLevelupPopに戻れるようにする
                Debug.Log($"{PopupManager.instance.CurrentViewPop}をhistoryに追加しました : {PopupManager.instance.History}");
            })
            .AddTo(this);
    }
}
