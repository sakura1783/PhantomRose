using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// 何かのイベントを実行するためのインターフェース。カードの効果を実行する、探索イベントを実行するなど、統一処理を行ってハンドルすることができる
/// </summary>
public interface ICommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="token"></param>
    /// <param name="slotNo">何番目のスロットか</param>
    /// <returns></returns>
    UniTask ExecuteAsync(OwnerStatus owner, CancellationToken token, int slotNo);

    int GetId();
}
