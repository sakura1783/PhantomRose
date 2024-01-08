using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public CardDataSO cardDataSO;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetCardData()
    {
        //foreach (CardData cardData in cardDataSO.cardDataList)
        //{
        //    GameData.instance.myCardList.Add(cardData);
        //}

        // 上記をLinqを使ってリファクタリング
        GameData.instance.myCardList.AddRange(cardDataSO.cardDataList);
    }
}
