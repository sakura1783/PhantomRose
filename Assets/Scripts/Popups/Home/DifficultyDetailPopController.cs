using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class DifficultyDetailPopController : MonoBehaviour
{
    [SerializeField] private Image imgDifficulty;
    [SerializeField] private Image imgCrown;

    [SerializeField] private Text txtDifficulty;
    [SerializeField] private Text txtDiamondRewardCount;

    [SerializeField] private Button btnLevelDown;
    [SerializeField] private Button btnLevelUp;

    [SerializeField] private Button btnStart;

    [SerializeField] Transform gemRewardTran;

    [SerializeField] private CanvasGroup canvasGroup;
    public CanvasGroup CanvasGroup
    {
        get => canvasGroup;
        set => canvasGroup = value;
    }

    [SerializeField] private CanvasGroup levelButtonGroup;

    [SerializeField] private ClearRewardController clearRewardPrefab;

    // 各画像のアサイン用
    [SerializeField] private Sprite difficultySilverSprite;
    [SerializeField] private Sprite difficultyGoldSprite;
    [SerializeField] private Sprite difficultyDiamondSprite;
    [SerializeField] private Sprite silverCrownSprite;
    [SerializeField] private Sprite goldCrownSprite;
    [SerializeField] private Sprite diamondCrownSprite;

    private int level;  // ダイヤモンド2の、2にあたる部分
    public int Level => level;

    private Dictionary<DifficultyType, Sprite> difficultySpriteDic = new();
    private Dictionary<DifficultyType, Sprite> crownSpriteDic = new();


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="difficultyData"></param>
    public void SetUp(DifficultyLevelDataSO.DifficultyLevelData difficultyData, BattleEntryPop battleEntryPop)
    {
        InitDictionaries();

        // 各値の設定
        if (difficultySpriteDic.ContainsKey(difficultyData.difficultyType))
        {
            imgDifficulty.sprite = difficultySpriteDic[difficultyData.difficultyType];
        }
        if (crownSpriteDic.ContainsKey(difficultyData.difficultyType))
        {
            imgCrown.sprite = crownSpriteDic[difficultyData.difficultyType];
        }
        txtDifficulty.text = difficultyData.difficultyName;
        txtDiamondRewardCount.text = difficultyData.diamond.ToString();

        // ジェム報酬の生成
        var rewardGems = GetRewardGemDetail(difficultyData);
        for (int i = 0; i < rewardGems.Count; i++)
        {
            var gem = Instantiate(clearRewardPrefab, gemRewardTran);
            gem.SetUp(rewardGems.ElementAtOrDefault(i).Key, rewardGems.ElementAtOrDefault(i).Value);
        }

        // TODO 前の難易度をクリアしてから新しい難易度を解放させる
        btnStart.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2f))
            .Subscribe(_ => PopupManager.instance.SwitchToBattleOrHomeScene("Battle", difficultyData))
            .AddTo(this);

        // 自身が何番目のレベルか把握
        level = transform.parent.childCount;

        // 難易度がシルバーまたはゴールドの場合
        if (difficultyData.difficultyType != DifficultyType.Diamond)
        {
            levelButtonGroup.alpha = 0;
            levelButtonGroup.blocksRaycasts = false;

            // ボタンの処理は必要ないので、以下は処理しない
            return;
        }

        // レベルが1(つまり、最小値)なら
        if (level == 1)
        {
            btnLevelDown.interactable = false;
        }
        // レベルが最大値なら
        if (level == DataBaseManager.instance.difficultyLevelDataSO.difficultyLevelDataList.Count(data => data.difficultyType == difficultyData.difficultyType))
        {
            btnLevelUp.interactable = false;
        }

        // レベルが1でないなら
        if (level != 1)
        {
            // ポップアップを非表示にする
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

        // ボタンの購読処理
        btnLevelDown.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => battleEntryPop.SwitchDifficultyLevel(level, false))
            .AddTo(this);

        btnLevelUp.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => battleEntryPop.SwitchDifficultyLevel(level, true))
            .AddTo(this);
    }

    /// <summary>
    /// 各Dicの初期化
    /// </summary>
    private void InitDictionaries()
    {
        difficultySpriteDic.Add(DifficultyType.Silver, difficultySilverSprite);
        difficultySpriteDic.Add(DifficultyType.Gold, difficultyGoldSprite);
        difficultySpriteDic.Add(DifficultyType.Diamond, difficultyDiamondSprite);

        crownSpriteDic.Add(DifficultyType.Silver, silverCrownSprite);
        crownSpriteDic.Add(DifficultyType.Gold, goldCrownSprite);
        crownSpriteDic.Add(DifficultyType.Diamond, diamondCrownSprite);
    }

    /// <summary>
    /// 報酬のジェムの種類と個数を取得
    /// </summary>
    /// <param name="difficultyData"></param>
    /// <returns></returns>
    private Dictionary<GemType, int> GetRewardGemDetail(DifficultyLevelDataSO.DifficultyLevelData difficultyData)
    {
        // 下記をリファクタリング
        var gemRewardDic = new Dictionary<GemType, int>
        {
            { GemType.Purple, difficultyData.purpleGem },
            { GemType.Gold, difficultyData.goldGem},
            { GemType.Diamond, difficultyData.diamondGem },
            { GemType.Ruby, difficultyData.rubyGem }
        }
        .Where(dic => dic.Value > 0)
        .ToDictionary(dic => dic.Key, dic => dic.Value);

        //if (difficultyData.purpleGem > 0)
        //{
        //    gemRewardDic.Add(GemType.Purple, difficultyData.purpleGem);
        //}
        //if (difficultyData.goldGem > 0)
        //{
        //    gemRewardDic.Add(GemType.Gold, difficultyData.goldGem);
        //}

        return gemRewardDic;
    }
}
