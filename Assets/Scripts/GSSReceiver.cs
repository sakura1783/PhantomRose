using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// スプレッドシートからGSSReaderが取得した情報(SheetData配列変数内に格納されている)をシート単位で任意のスクリプタブルオブジェクトに値として取り込む。
/// </summary>
[RequireComponent(typeof(GSSReader))]
public class GSSReceiver : MonoBehaviour
{
    public bool IsLoading { get; set; }  // プロパティだとインスペクターから確認できないため、一時的に変数にしても良い。


    private void Awake()
    {
        // GSSのデータ取得準備
        StartCoroutine(PrepareGSSLoadStart());
    }

    /// <summary>
    /// GSSのデータ取得準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepareGSSLoadStart()
    {
        IsLoading = true;

        //TODO GSSを取得してスクリプタブルオブジェクトに取得。(この処理が終わるまでここで待機する)
        yield return StartCoroutine(GetComponent<GSSReader>().GetFromWeb());

        IsLoading = false;

        //TODO 確認
        Debug.Log("GSSデータをスクリプタブルオブジェクトに取得");
    }

    /// <summary>
    /// インスペクターからGSSReaderにこのメソッドを追加することでGSSの読み込み完了時にコールバックされる
    /// </summary>
    public void OnGSSLoadEnd()
    {
        GSSReader reader = GetComponent<GSSReader>();

        // スプレッドシートから取得した各シートの配列をListに変換
        List<SheetData> sheetDatasList = reader.sheetDatas.ToList();

        // 情報が取得できた場合
        if (sheetDatasList != null)
        {
            // 各スクリプタブルオブジェクトに代入
            DataBaseManager.instance.cardDataSO.cardDataList =
                new List<CardData>(sheetDatasList.Find(x => x.SheetName == SheetName.CardData).DatasList.Select(x => new CardData(x)).ToList());  //Find()で、SheetName.CardDataと一致するシートのデータを取得し、それをSelect()でCardData型に変換して、ToList()でCardData型の新しいリストを作成。

            //TODO 他にもあれば追加
        }
    }
}
