using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public int id;

    public string name;
    public string englishName;

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
        englishName = datas[2];
        cardType = (CardType)System.Enum.Parse(typeof(CardType), datas[3]);
        level = int.Parse(datas[4]);
        attackPower = int.Parse(datas[5]);
        shieldPower = int.Parse(datas[6]);
        recoveryPower = int.Parse(datas[7]);
        coolTime = int.Parse(datas[8]);
        ColorUtility.TryParseHtmlString(datas[9], out cardColor);
        spriteId = int.Parse(datas[10]);
        price = int.Parse(datas[11]);
        description = datas[12];

        // ""という文字列である場合、処理しない
        if (datas[13] == "\"\"")  // \"\"でダブルクォート
        {
            return;
        }

        // stateListの作成
        // Splitメソッドを利用し、'/"で分割してList用の個々のデータを文字列の配列(クラスごと)として取得
        string[] stateInfo = datas[13].Split('/');
        stateList = new();

        Debug.Log($"stateInfo : {stateInfo.Length}");

        foreach (string data in stateInfo)
        {
            // Splitメソッドを利用し、':'で分割して、クラス内の変数用のデータを文字列の配列として取得
            string[] stateDetails = data.Split(':');

            // クラスのインスタンス作成
            SimpleStateData simpleState = new();

            // クラス内の各変数に文字列をキャストして代入
            simpleState.stateId = int.Parse(stateDetails[0]);
            simpleState.duration = int.Parse(stateDetails[1]);

            stateList.Add(simpleState);
        }
    }
}
