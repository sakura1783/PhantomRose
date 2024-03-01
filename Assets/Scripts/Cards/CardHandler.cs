using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public class CardHandler
{
    private Subject<ICommand> commandSubject = new();
    public IObservable<ICommand> CommandSubject => commandSubject;


    /// <summary>
    /// カードの実処理。左端にセットされているカードから順番に1つずつ処理する
    /// </summary>
    /// <param name="commandList"></param>
    /// <param name="ownerList"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask<BattleState> ExecuteCommandsAsync(List<ICommand> commandList, List<OwnerStatus> ownerList, CancellationToken token)
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            UnityEngine.Debug.Log(ownerList.Count);
            UnityEngine.Debug.Log(token);

            await commandList[i].ExecuteAsync(ownerList[i], token);

            // 仮の待機時間。本来はエフェクトなどの絡みがあるので、各ExecuteAsync内にかく
            await UniTask.Delay(1500, cancellationToken: token);

            // HPのチェック。いずれかが0になったら、以降の処理はキャンセルする
            if (GameData.instance.GetPlayer().Hp.Value <= 0)
            {
                return BattleState.Lose;
            }
            else if (GameData.instance.GetOpponent().Hp.Value <= 0)
            {
                return BattleState.Win;
            }
        }

        // 外部で購読することで、処理の終了をアナウンスできる
        commandSubject.OnNext(null);  // OnNextでSubjectの状態が変化したことを通知する

        return BattleState.Continue;
    }
}
