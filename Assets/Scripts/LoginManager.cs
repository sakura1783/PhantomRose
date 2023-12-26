using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;  //UniTask

public static class LoginManager  //ゲーム開始時に一個だけインスタンスが作られる
{
    /// <summary>
    /// コンストラクタ(Startメソッドの代わり)
    /// </summary>
    static LoginManager()
    {
        // タイトルIDの設定
        PlayFabSettings.staticSettings.TitleId = "AAF92";

        // ログを出す
        Debug.Log($"タイトル設定：{PlayFabSettings.staticSettings.TitleId}");
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]  //Awakeよりも早く動くメソッドが作れる
    private static async UniTaskVoid InitializeAsync()
    {
        Debug.Log("初期化開始");

        // Playfabへのログイン準備と、ログイン
        await PrepareLoginPlayfab();

        // ログを出す
        Debug.Log("初期化完了");
    }

    /// <summary>
    /// Playfabへのログイン準備と、ログイン
    /// </summary>
    /// <returns></returns>
    public static async UniTask PrepareLoginPlayfab()
    {
        Debug.Log("ログイン準備開始");

        await LoginAndUpdateLocalCacheAsync();

        //// 仮のログイン情報(リクエスト)を作成して、設定する
        //var request = new LoginWithCustomIDRequest
        //{
        //    CustomId = "userId",
        //    CreateAccount = true
        //};

        //// Playfabへログイン(情報が確認できるまで待機)
        //var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);  //これを使ってplayfabとやりとりする

        //// エラーの内容を見て、ログインに成功しているか判定する(エラーハンドリング)
        //string message = result.Error is null
        //    ? $"ログイン成功 PlayFabのIDは {result.Result.PlayFabId}"
        //    : result.Error.GenerateErrorReport();

        //// ログを出す
        //Debug.Log(message);
    }

    /// <summary>
    /// UserDataとTitleDataを初期化
    /// </summary>
    /// <returns></returns>
    public static async UniTask LoginAndUpdateLocalCacheAsync()  //非同期の処理は後ろにAsyncをつける
    {
        Debug.Log("初期化開始");

        // userIdの取得を試みる
        string userId = PlayerPrefsManager.UserId;

        // userIdが取得できない場合は、新規作成して匿名ログインする
        // 取得できた場合は、userIdを使って、ログインする
        var loginResult = string.IsNullOrEmpty(userId)
            ? await CreateNewUserAsync()
            : new LoginResult();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static async UniTask<LoginResult> CreateNewUserAsync()
    {
        Debug.Log("ユーザーデータなし。新規ユーザー作成");

        while (true)
        {
            // userIdを採番する
            var newUserId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);  //ランダムな英数字を作ってくれる

            // リクエストを作成する
            var request = new LoginWithCustomIDRequest
            {
                CustomId = newUserId,
                CreateAccount = true
            };

            // PlayFabにログインする
            var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

            // エラーハンドリングする
            string message = result.Error is null
            ? $"ログイン成功 PlayFabのIDは {result.Result.PlayFabId}"
            : result.Error.GenerateErrorReport();

            // もしもuserIdがぶつかった場合、リトライする
            if (result.Result.LastLoginTime.HasValue)
            {
                continue;
            }

            // PlayerPrefsにuserIdを記録する
            PlayerPrefsManager.UserId = newUserId;

            return result.Result;
        }
    }
}
