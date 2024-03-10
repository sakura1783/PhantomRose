using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// 指定したクラスをstring型のJson形式でPlayerPrefsクラスにセーブ・ロードするためのHelperクラス
/// </summary>
public static class PlayerPrefsHelper
{
    /// <summary>
    /// 指定したキーのデータが存在しているか確認
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool ExistsData(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    /// <summary>
    /// 指定されたオブジェクトのデータをセーブ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public static void Save<T>(string key, T obj)
    {
        // オブジェクトのデータをJson形式に変換
        string json = JsonUtility.ToJson(obj);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        Debug.Log($"{key}をセーブしました");
    }

    /// <summary>
    /// 指定されたオブジェクトのデータをロード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Load<T>(string key)
    {
        Debug.Log($"{key}をロードします");

        string json = PlayerPrefs.GetString(key);

        // 読み込む型を指定し、変換して取得
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// 指定したデータを削除
    /// </summary>
    /// <param name="key"></param>
    public static void ClearSaveData(string key)
    {
        PlayerPrefs.DeleteKey(key);

        Debug.Log($"{key}のデータを削除しました");
    }

    /// <summary>
    /// GameDataクラスをセーブ
    /// </summary>
    public static void SaveGameData()
    {
        // GameDataをJSONにシリアライズ
        string jsonString = JsonConvert.SerializeObject(GameDataManager.instance.gameData);

        // JSON文字列を保存
        PlayerPrefs.SetString(ConstData.GAME_DATA_SAVE_KEY, jsonString);
        PlayerPrefs.Save();

        Debug.Log($"{GameDataManager.instance.gameData}をセーブしました");
    }

    /// <summary>
    /// GameDataクラスをロード
    /// </summary>
    public static void LoadGameData()
    {
        // JSON文字列をロード
        string jsonString = PlayerPrefs.GetString(ConstData.GAME_DATA_SAVE_KEY);

        // JSON文字列をGameDataクラスにデシリアライズ
        GameDataManager.instance.gameData = JsonConvert.DeserializeObject<GameData>(jsonString);

        Debug.Log($"{GameDataManager.instance.gameData}をロードしました");
    }
}
