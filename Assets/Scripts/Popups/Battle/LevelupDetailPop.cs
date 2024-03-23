using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class LevelupDetailPop : PopupBase
{
    [SerializeField] private Button btnLevelUp;

    [SerializeField] private Text txtRubyCount;

    [SerializeField] private Transform originalCardPlace;
    [SerializeField] private Transform enhancedCardPlace;

    [SerializeField] private Transform originalCardDetailPlace;
    [SerializeField] private Transform enhancedCardDetailPlace;

    [SerializeField] private CardController cardPrefab;

    [SerializeField] private CardDescriptionPopController levelupDetailPrefab;

    [SerializeField] private BattleAlwaysPop battleAlwaysPop;

    [SerializeField] private CardLevelUpPop cardLevelupPop;

    [SerializeField] private CardDeckPop cardDeckPop;

    private CardData selectCard;

    private int selectCardSerialNo;

    private List<GameObject> generateObjects = new();


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        // ボタンの購読処理
        btnLevelUp.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ =>
            {
                // ルビーを減らす
                GameDataManager.instance.gameData.RubyCount.Value -= selectCard.price;

                // GameDataのlevelupCardSerialNumbersリストにカードの通し番号を追加
                GameDataManager.instance.gameData.levelupCardSerialNumbers.Add(selectCardSerialNo);

                // TODO インベントリのカードにレベルアップ後の見た目を反映(今は情報をセットしているだけ)
                cardDeckPop.GeneratedCardList.Where(card => card.SerialNo == selectCardSerialNo).FirstOrDefault().EditCardData(selectCard.id);
                //cardDeckPop.GeneratedCardList.Where(card => card.SerialNo == selectCardSerialNo).FirstOrDefault().SetUp(selectCard, , )

                battleAlwaysPop.SetReturnButtonActivation(false);
                cardLevelupPop.HidePopUp();
            })
            .AddTo(this);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    /// <param name="cardData"></param>
    public void ShowLevelupDetailPopUp(CardData cardData, int serialNo)
    {
        selectCard = cardData;
        selectCardSerialNo = serialNo;

        // 前回生成した各オブジェクトを破棄
        foreach (var obj in generateObjects)
        {
            Destroy(obj);
        }

        // カードの生成
        var originalCard = Instantiate(cardPrefab, originalCardPlace);
        originalCard.SetCardDetail(cardData);

        var enhancedCard = Instantiate(cardPrefab, enhancedCardPlace);
        enhancedCard.SetCardDetail(null, DataBaseManager.instance.levelUpCardDataSO.levelUpCardDataList.Where(data => data.cardId == cardData.id).FirstOrDefault());

        // カード詳細オブジェクトの生成
        var originalCardDetailObj = Instantiate(levelupDetailPrefab, originalCardDetailPlace);
        originalCardDetailObj.SetUp(cardData, false);

        var enhancedCardDetailObj = Instantiate(levelupDetailPrefab, enhancedCardDetailPlace);
        enhancedCardDetailObj.SetUp(cardData, true);

        // Listに各オブジェクトを追加
        generateObjects.Add(originalCard.gameObject);
        generateObjects.Add(enhancedCard.gameObject);
        generateObjects.Add(originalCardDetailObj.gameObject);
        generateObjects.Add(enhancedCardDetailObj.gameObject);

        // 消費ルビーの表示
        txtRubyCount.text = $"{GameDataManager.instance.gameData.RubyCount}→{GameDataManager.instance.gameData.RubyCount.Value - cardData.price}";

        // ルビーが足りない場合
        if (GameDataManager.instance.gameData.RubyCount.Value < cardData.price)  // TODO レベルアップ用のコストを用意する
        {
            // 文字色の変更
            txtRubyCount.color = ColorManager.instance.GetColor(ColorType.Red);

            // ボタンを押せないようにする
            btnLevelUp.interactable = false;
        }
        else
        {
            txtRubyCount.color = ColorManager.instance.GetColor(ColorType.VeryLightRed);
            btnLevelUp.interactable = true;
        }

        battleAlwaysPop.SetReturnButtonActivation(true);

        base.ShowPopUp();
    }
}
