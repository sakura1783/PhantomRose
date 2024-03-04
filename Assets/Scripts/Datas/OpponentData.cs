using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class OpponentData
{
    public int id;
    public int hp;
    public string name;

    public List<CardData> handCards = new();  // 手札のカード

    public int[] dropCards;  // バトル勝利後にもらえる可能性のあるカード


    /// <summary>
    /// コンストラクタ。GSSからスプレッドシートに情報を取得
    /// </summary>
    /// <param name="datas"></param>
    public OpponentData(string[] datas)
    {
        id = int.Parse(datas[0]);
        hp = int.Parse(datas[1]);
        name = datas[2];

        string[] handCardArray = datas[3].Split("/");
        foreach(var cardId in handCardArray)
        {
            handCards.Add(DataBaseManager.instance.cardDataSO.cardDataList[int.Parse(cardId)]);
        }

        string[] dropCardArray = datas[4].Split("/");
        dropCards = dropCardArray.Select(int.Parse).ToArray();
    }
}
