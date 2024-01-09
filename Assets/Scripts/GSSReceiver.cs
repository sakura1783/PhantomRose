using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// スプレッドシートからGSSReaderが取得した情報(SheetData配列変数内に格納されている)をシート単位で任意のスクリプタブルオブジェクトに値として取り込む。
/// </summary>
[RequireComponent(typeof(GSSReader))]
public class GSSReceiver : MonoBehaviour
{
    public bool IsLoading { get; set; }


    private void Awake()
    {
        // GSSのデータ取得準備
        //StartCoroutine(PrepareGSSLoadStart());
        PrepareGSSLoadStartAsync().Forget();
    }

    /// <summary>
    /// GSSのデータ取得準備
    /// </summary>
    /// <returns></returns>
    private async UniTask PrepareGSSLoadStartAsync()
    {
        IsLoading = true;

        //TODO 読み込んでいない場合には、GSSを取得してスクリプタブルオブジェクトに取得。(この処理が終わるまでここで待機する)
        //yield return StartCoroutine(GetComponent<GSSReader>().GetFromWeb());
        await GetComponent<GSSReader>().GetFromWebAsync();

        IsLoading = false;

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
