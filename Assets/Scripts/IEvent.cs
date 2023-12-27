using Cysharp.Threading.Tasks;

/// <summary>
/// イベント用のインターフェース
/// </summary>
public interface IEvent
{
    UniTask ExecuteEvent();
}
