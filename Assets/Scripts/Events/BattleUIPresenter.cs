using System;
using UnityEngine;
using UniRx;

public class BattleUIPresenter : MonoBehaviour
{
    [SerializeField] private BattleUIView battleUIView;

    private IDisposable subscription;


    /// <summary>
    /// プレイヤーのHP用UIをMVPパターンで設定
    /// </summary>
    public void SubscribePlayerHp()
    {
        battleUIView.SetUpPlayerHp(GameData.instance.GetPlayer().GetHp);

        // プレイヤーのHPの購読処理(HPSlider更新用にprevValueも使う場合)
        //GameData.instance.GetPlayer().Hp
        //    .Zip(GameData.instance.GetPlayer().Hp.Skip(1), (prevValue, currentValue) => (prevValue, currentValue))  // Zipオペレータは2つのOvservableシーケンスを組み合わせ、Skipオペレータはシーケンスの最初の値を無視する
        //    .Subscribe(values => battleUIView.UpdatePlayerHp(values.prevValue, values.currentValue))
        //    .AddTo(this);
        GameData.instance.GetPlayer().Hp
            .Subscribe(value => battleUIView.UpdatePlayerHp(value))
            .AddTo(this);
    }

    /// <summary>
    /// 対戦相手のHP用UIをMVPパターンで設定
    /// </summary>
    public void SubscribeOpponentHp()
    {
        battleUIView.SetUpOpponentHp(GameData.instance.GetOpponent().GetHp);

        // 対戦相手のHPの購読処理(対戦相手が変わるたびに購読するので、AddToではなく、対戦相手がいなくなるたびに毎回購読を止める必要がある)
        subscription = GameData.instance.GetOpponent().Hp
            .Subscribe(value => battleUIView.UpdateOpponentHp(value));
    }

    /// <summary>
    /// バトル終了時に紐付けする予定
    /// </summary>
    public void EndBattle()
    {
        // 対戦相手のHPの購読を停止(ここで停止しておかないと、次の対戦相手のHPの購読が重複して発生してしまう)
        subscription?.Dispose();

        // 処理自体は変数内に残っているので、nullにすることで残っている処理も削除する
        subscription = null;
    }
}
