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


    /// <summary>
    /// GSSのデータ取得準備
    /// </summary>
    /// <returns></returns>
    public async UniTask PrepareGSSLoadStartAsync()
    {
        IsLoading = true;

        // 読み込んでいない場合には、GSSを取得してスクリプタブルオブジェクトに取得。(この処理が終わるまでここで待機する)
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

            DataBaseManager.instance.stateDataSO.stateDataList =
                new List<StateData>(sheetDatasList.Find(x => x.SheetName == SheetName.StateData).DatasList.Select(x => new StateData(x)).ToList());

            DataBaseManager.instance.opponentDataSO.opponentDataList =
                new List<OpponentData>(sheetDatasList.Find(x => x.SheetName == SheetName.OpponentData).DatasList.Select(x => new OpponentData(x)).ToList());

            DataBaseManager.instance.stageDetailDataSO.stageDetailDataList =
                new List<StageDetailData>(sheetDatasList.Find(x => x.SheetName == SheetName.StageDetailData).DatasList.Select(x => new StageDetailData(x)).ToList());

            //TODO 他にもあれば追加
        }
    }
}
