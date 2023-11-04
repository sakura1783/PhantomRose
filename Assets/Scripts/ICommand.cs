using Cysharp.Threading.Tasks;
using System.Threading;

public interface ICommand
{
    UniTask ExecuteAsync(CancellationToken token);

    int GetId();
}
