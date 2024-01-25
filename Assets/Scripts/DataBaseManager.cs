using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public CardDataSO cardDataSO;
    public StateDataSO stateDataSO;


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
    /// TODO
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
