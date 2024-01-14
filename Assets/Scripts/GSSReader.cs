using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// シート名の登録用
/// </summary>
public enum SheetName
{
    None,
    CardData,
    EventData,

    // その他にもあれば追加
}

/// <summary>
/// 読み込むシートのデータ群。シートの情報を管理する。
/// </summary>
[System.Serializable]
public class SheetData
{
    public SheetName SheetName;
    public List<string[]> DatasList = new List<string[]>();
}

/// <summary>
/// スプレッドシートのシートへアクセスして情報を取得する。
/// </summary>
public class GSSReader : MonoBehaviour
{
    public string SheetID = "読み込むスプレッドシートのアドレス";

    public UnityEvent OnLoadEnd;  // この変数にインスペクターからメソッドを登録しておくと、スプレッドシートを読み込み後にコールバックする

    [Header("読み込みたいシート名を選択")]
    public SheetData[] sheetDatas;


    //public void Reload() => StartCoroutine(GetFromWeb());
    public async UniTask Reload() => await GetFromWebAsync();

    /// <summary>
    /// スプレッドシートの取得
    /// </summary>
    /// <returns></returns>
    public async UniTask GetFromWebAsync()
    {
        // CancellationTokenの作成
        var token = this.GetCancellationTokenOnDestroy();  // thisで、このクラスが破棄された時に処理をキャンセルする

        // 複数のシートの読み込み
        for (int i = 0; i < sheetDatas.Length; i++)
        {
            // シート名だけ毎回読み込み先を変更する
            string url = "https://docs.google.com/spreadsheets/d/" + SheetID + "/gviz/tq?tqx=out:csv&sheet=" + sheetDatas[i].SheetName.ToString();

            // ウェブのGoogleSpreadSheetを取得
            UnityWebRequest request = UnityWebRequest.Get(url);

            // 取得できるまで待機
            //yield return request.SendWebRequest();  // SendWebRequest()で受信(サーバーとの通信)開始
            await request.SendWebRequest().WithCancellation(token);  // CancellationTokenの設定も行い、非同期処理をキャンセルした場合には処理が停止するようにセットする
            Debug.Log(request.downloadHandler.text);

            // エラーが発生しているか確認
            bool protocol_error = request.result == UnityWebRequest.Result.ProtocolError ? true : false;
            bool connection_error = request.result == UnityWebRequest.Result.ConnectionError ? true : false;

            // エラーがある場合
            if (protocol_error || connection_error)
            {
                // エラー表示を行い、処理を終了する
                Debug.LogError(request.error);

                //yield break;
                return;
            }

            // GSSの各シートごとのデータをList<string[]>の形で取得
            sheetDatas[i].DatasList = ConvertToArrayListFromCSV(request.downloadHandler.text);
        }

        // GSSReaderのメソッドを登録しておいて実行する
        OnLoadEnd.Invoke();
    }

    /// <summary>
    /// 取得したGoogleSpreadSheetのCSVファイル(データをコンマで区切る形式で、スプレッドシートのセルやテキストファイルなどで一般的に使用される)の情報をArrayList形式に変換
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private List<string[]> ConvertToArrayListFromCSV(string text)
    {
        StringReader reader = new StringReader(text);
        reader.ReadLine();  // 1行目はヘッダー情報なので、読み飛ばす  //StringReader.ReadLine()で、現在の文字列から一行分の文字を読み取る。

        List<string[]> rows = new List<string[]>();

        while (reader.Peek() >= 0)  // Peekメソッドを使うと、戻り値の値によりファイルの末尾まで達しているか確認できる。末尾になると-1が戻るので、そうなるまで繰り返す
        {
            string line = reader.ReadLine();  // 一行ずつ読み込み
            string[] elements = line.Split(',');  // 行のセルは「,」で区切られているので、それを分割して一個ずつの情報が入った配列にする

            // 1セルずつ取り出す
            for (int i = 0; i < elements.Length; i++)
            {
                //取り出したセル(文字列)が空白である場合はスキップ
                if (elements[i] == "\"\"")  // 「\"\"」は空白のセルを表すために使用される。
                {
                    continue;
                }

                // 文字列の最初と最後にある""を削除する
                elements[i] = elements[i].TrimStart('"').TrimEnd('"');

                //Debug.Log(elements[i]);
            }

            rows.Add(elements);
        }

        return rows;
    }
}
