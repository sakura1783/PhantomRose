using Cysharp.Threading.Tasks;

/// <summary>
/// イベント用のインターフェース
/// </summary>
public interface IEvent
{
    UniTask ExecuteEvent();

    /// <summary>
    /// 初期設定
    /// </summary>
    //void SetUp();
}
