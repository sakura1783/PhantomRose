using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class PopupManager : AbstractSingleton<PopupManager>  // <型引数>に指定したクラスがシングルトン化される
{
    [SerializeField] private List<PopupBase> popupList = new();

    [SerializeField] private MainGameManager mainGameManager;

    private PopupBase currentViewPop;
    public PopupBase CurrentViewPop => currentViewPop;
    //public ReactiveProperty<PopupBase> currentViewPop = new();  // テスト用。監視

    readonly Stack<PopupBase> history = new();  // 以前開いていたポップアップを保持するためのStack(スタック。新しい要素を追加し、最後に追加された要素を取り出す)

    private PopupBase descriptionPop;
    public PopupBase DescriptionPop => descriptionPop;


    /// <summary>
    /// 初期設定
    /// </summary>
    public async void SetUp()
    {
        // インスペクターでアサインしない場合、ここで取得(その場合、対象としたい全てのポップアップをこのゲームオブジェクトの子にする必要がある)
        //popupList = m_Root.GetComponentsInChildren<PopupBase>(true).ToList();

        await InitPopupsAsync();

        // 管理しているポップアップの初期設定
        //foreach (var pop in popupList) pop.SetUp();
        // 上記をLinqで書いた場合
        popupList.ForEach(pop => pop.SetUp());

        descriptionPop = popupList.OfType<DescriptionPop>().SingleOrDefault();

        //TODO キャンバスのリサイズなど(その場合はメンバ変数を増やす)
    }

    /// <summary>
    /// 管理しているポップアップの初期化(ここはポップアップの初期化を行いたい時に実行するので、実行回数は1回とは限らない)
    /// </summary>
    private async UniTask InitPopupsAsync()
    {
        popupList.ForEach(pop => pop.HidePopUp());

        // Stackをクリア
        history.Clear();

        await UniTask.DelayFrame(1);
    }

    /// <summary>
    /// 指定された型の最初に登録されたPopupBaseを継承しているクラスを検索して取得
    /// </summary>
    /// <typeparam name="T">検索対象のPopupBaseクラス</typeparam>
    /// <returns>検索結果となる、指定された型のPopupBaseインスタンス。見つからない場合はnull</returns>
    public T GetPopup<T>() where T : PopupBase  // TがPopupBaseクラスまたはその継承クラスであることを指定
    {
        //foreach (var pop in popupList)
        //{
        //    if (pop is T tPop)
        //    {
        //        return tPop;
        //    }
        //}

        // 上記をLinqで書いた場合
        return popupList.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// 指定された型のPopupBaseを継承したクラスを検索して表示する
    /// </summary>
    /// <typeparam name="T">検索対象のPopupBaseクラス</typeparam>
    /// <param name="keepInHistory">現在開いているポップアップを履歴スタックに追加するかどうか。trueで追加</param>
    public PopupBase Show<T>(bool keepInHistory = true, bool closeCurrentPop = true) where T : PopupBase
    {
        //foreach (var pop in popupList)
        //{
            //if (pop is T)
            //{
            //    OpenPopup(pop, keepInHistory);

            //    break;
            //}
        //}

        // 上記をLinqで書いた場合
        var targetPop = popupList.OfType<T>().SingleOrDefault();  // FirstOrDefaultでも良い

        // 指定された型のポップアップが見つからない場合
        if (targetPop == null)
        {
            Debug.Log("指定された型のポップアップが見つかりません。");

            return null;
        }

        // すでに開いているポップアップの場合には処理しない
        if (currentViewPop == targetPop)
        {
            Debug.Log($"{targetPop}はすでに開いています。");

            return null;
        }

        // ポップアップを開く
        OpenPopup(targetPop, keepInHistory, closeCurrentPop);

        return targetPop;
    }

    /// <summary>
    /// ポップアップを表示し、他のポップアップを非表示にする
    /// </summary>
    /// <param name="pop">表示するポップアップ/param>
    /// <param name="keepInHistory">現在開いているポップアップを履歴スタックに追加するかどうか。trueで追加</param>
    private void OpenPopup(PopupBase pop, bool keepInHistory = true, bool closeCurrentPop = true)
    {
        // 現在開いているポップアップが存在している場合
        if (currentViewPop != null)
        {
            // 現在開いているポップアップを覚えておきたい場合
            if (keepInHistory)
            {
                // StackにPushして保持
                history.Push(currentViewPop);

                Debug.Log($"{currentViewPop}をhistoryに追加しました : {history}");
            }

            if (closeCurrentPop)
            {
                // 現在開いているポップアップを閉じる
                currentViewPop.HidePopUp();
            }
        }

        // 新しいポップアップを開く
        pop.ShowPopUp();

        // 現在開いているポップアップを更新
        currentViewPop = pop;
    }

    /// <summary>
    /// 履歴スタックがある場合、以前に表示されていた1つ前のポップアップに戻る
    /// </summary>
    public void GoBack()
    {
        // Stackの中身がある場合(保持されている以前のポップアップがある場合)
        if (history.Count != 0)
        {
            // 前回のポップアップを再表示(これはStackに積む必要はないのでfalseで実行する)
            //OpenPopup(history.Pop(), false);
            OpenPopup(history.Peek(), false);  // Stackから取り除かないPeekに変更(RoutePopなど、何回も戻りたいポップアップもあるので)
        }
        else
        {
            Debug.Log("historyがありません");
        }
    }

    /// <summary>
    /// 引数で受け取った値に応じて、それぞれのポップアップの表示、非表示を切り替える
    /// バトル画面↔︎ホーム画面に移動したい時に使用する
    /// </summary>
    /// <param name="sceneName"></param>
    public void SwitchToBattleOrHomeScene(string nextSceneName, DifficultyLevelDataSO.DifficultyLevelData difficultyData = null)
    {
        if (nextSceneName == "Battle")
        {
            ShowBattlePop(difficultyData);
        }
        else if (nextSceneName == "Home")
        {
            ShowHomePop();
        }
        else
        {
            Debug.Log("該当のシーンがありません。");
        }
    }

    /// <summary>
    /// ホーム画面のポップアップを全て非表示にして、バトル用のポップアップを表示する
    /// </summary>
    private async void ShowBattlePop(DifficultyLevelDataSO.DifficultyLevelData difficultyData)
    {
        //ルート作成
        mainGameManager.GenerateRoute(difficultyData);

        await InitPopupsAsync();

        // バトル用のポップアップを開く
        Show<BattleBackgroundPop>(false);
        Show<RoutePop>(false, false);
        Show<BattleAlwaysPop>(true, false);
    }

    /// <summary>
    /// バトル用のポップアップを全て非表示にして、ホーム画面のポップアップを表示する
    /// </summary>
    private async void ShowHomePop()
    {
        await InitPopupsAsync();

        Show<HomeAlwaysPop>(false);
        Show<MyRoomPop>(false, false);
    }

    /// <summary>
    /// カード詳細ポップアップを開く
    /// </summary>
    public void ShowDescription()
    {
        OpenPopup(descriptionPop);
    }

    /// <summary>
    /// カード詳細ポップアップを閉じる
    /// </summary>
    public void HideDescription()
    {
        descriptionPop.HidePopUp();
    }
}
