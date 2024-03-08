using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleEntryPop : PopupBase
{
    [SerializeField] private DifficultyDetailPopController difficultyDetailPopPrefab;

    [SerializeField] private Transform silverTran;
    [SerializeField] private Transform goldTran;
    [SerializeField] private Transform diamondTran;

    private Dictionary<DifficultyType, Transform> generateTranDic = new();

    private List<DifficultyDetailPopController> diamondPopList = new();


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        InitGenerateTranDic();

        // 各難易度のポップアップを生成
        foreach (var difficultyData in DataBaseManager.instance.difficultyLevelDataSO.difficultyLevelDataList)
        {
            if (generateTranDic.ContainsKey(difficultyData.difficultyType))
            {
                var pop = Instantiate(difficultyDetailPopPrefab, generateTranDic[difficultyData.difficultyType]);
                pop.SetUp(difficultyData, this);

                // 難易度ダイヤモンドのポップアップであれば、リストに追加
                if (difficultyData.difficultyType == DifficultyType.Diamond)
                {
                    diamondPopList.Add(pop);
                }
            }
        }

        SortDiamondPopOrder();
    }

    /// <summary>
    /// generateTranDic変数の初期設定
    /// </summary>
    private void InitGenerateTranDic()
    {
        generateTranDic.Add(DifficultyType.Silver, silverTran);
        generateTranDic.Add(DifficultyType.Gold, goldTran);
        generateTranDic.Add(DifficultyType.Diamond, diamondTran);
    }

    /// <summary>
    /// ダイヤモンド1→ダイヤモンド2 というように、レベルを切り替える
    /// </summary>
    /// <param name="currentLevel"></param>
    public void SwitchDifficultyLevel(int currentLevel, bool levelUp)
    {
        var nextLevel = levelUp ? currentLevel + 1 : currentLevel - 1;

        // 現在のポップアップを非表示にして、次のポップアップを表示する
        var currentPop = diamondPopList.FirstOrDefault(pop => pop.Level == currentLevel);
        if (currentPop)
        {
            currentPop.CanvasGroup.alpha = 0;
            currentPop.CanvasGroup.blocksRaycasts = false;
        }
        var nextPop = diamondPopList.FirstOrDefault(pop => pop.Level == nextLevel);
        if (nextPop)
        {
            nextPop.CanvasGroup.alpha = 1;
            nextPop.CanvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// ポップアップを、レベルが低い順に並び替え。レベルが高いほど、後ろへ配置
    /// </summary>
    private void SortDiamondPopOrder()
    {
        foreach (var pop in diamondPopList)
        {
            pop.transform.SetSiblingIndex(pop.Level);
        }
    }
}
