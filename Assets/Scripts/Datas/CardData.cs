using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public int id;

    public string name;

    public CardType cardType;

    public int level;

    public int attackPower;
    public int shieldPower;
    public int recoveryPower;

    public int coolTime;

    public Color cardColor;  // レベルごとにカードの色を変える

    public int spriteId;

    [Multiline] public string description;

    public int price;  // ショップなどで買う際の値段

    public List<SimpleStateData> stateList = new();  // 手動で値を設定する。これにより、状態の数や継続時間をカード個別に設定できる。


    /// <summary>
    /// コンストラクタ
    /// スプレッドシートから届く情報は全て文字列になっているため、データ型を合わせる
    /// </summary>
    /// <param name="datas"></param>
    public CardData(string[] datas)
    {
        // 取得した情報の確認
        //for (int i = 0; i < datas.Length; i++)
        //{
        //    Debug.Log(datas[i]);
        //}

        // 取得した情報をキャストして代入
        id = int.Parse(datas[0]);
        name = datas[1];
        cardType = (CardType)System.Enum.Parse(typeof(CardType), datas[2]);
        level = int.Parse(datas[3]);
        attackPower = int.Parse(datas[4]);
        shieldPower = int.Parse(datas[5]);
        recoveryPower = int.Parse(datas[6]);
        coolTime = int.Parse(datas[7]);
        ColorUtility.TryParseHtmlString(datas[8], out cardColor);
        spriteId = int.Parse(datas[9]);
        description = datas[10];
        price = int.Parse(datas[11]);
    }
}
