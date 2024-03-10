using UnityEngine;
using Newtonsoft.Json;

public class SaveLoadExample : MonoBehaviour
{
    /// <summary>
    /// GameDataクラスをセーブ
    /// </summary>
    public void SaveGameData()
    {
        // GameDataをJSONにシリアライズ
        string jsonString = JsonConvert.SerializeObject(GameDataManager.instance);

        // JSON文字列を保存
        PlayerPrefs.SetString(ConstData.GAME_DATA_SAVE_KEY, jsonString);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// GameDataクラスをロード
    /// </summary>
    public void LoadGameData()
    {
        // JSON文字列をロード
        string jsonString = PlayerPrefs.GetString(ConstData.GAME_DATA_SAVE_KEY);

        // JSON文字列をGameDataクラスにデシリアライズ
        GameDataManager.instance = JsonConvert.DeserializeObject<GameDataManager>(jsonString);
    }
}
