using UnityEngine;
using UniRx;

public class BattleUIPresenter : MonoBehaviour
{
    [SerializeField] private BattleUIView battleUIView;

    //private IDisposable subscription;
    private CompositeDisposable subscriptions = new();  // 購読停止したい処理がいくつかあるのでこれに変更


    /// <summary>
    /// バトル毎に実行する初期化と購読処理
    /// </summary>
    public void SubscribeEveryBattle()
    {
        SubscribeOpponentHp();

        SubscribePlayerShieldValue();
        SubscribeOpponentShieldValue();

        SubscribePlayerBuff();
        SubscribePlayerDebuff();
        SubscribeOpponentBuff();
        SubscribeOpponentDebuff();
    }

    /// <summary>
    /// プレイヤーのHP用UIをMVPパターンで設定
    /// </summary>
    public void SubscribePlayerHp()
    {
        battleUIView.SetUpPlayerHp(GameData.instance.GetPlayer().Hp.Value);

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
    private void SubscribeOpponentHp()
    {
        battleUIView.SetUpOpponentHp(GameData.instance.GetOpponent().Hp.Value);

        // 対戦相手のHPの購読処理(対戦相手が変わるたびに購読するので、AddToではなく、対戦相手がいなくなるたびに毎回購読を止める必要がある)
        GameData.instance.GetOpponent().Hp
            .Subscribe(value => battleUIView.UpdateOpponentHp(value))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// プレイヤーのシールド値の初期設定と購読処理
    /// </summary>
    private void SubscribePlayerShieldValue()
    {
        battleUIView.UpdatePlayerShieldValue(0);

        // バトルが終わるたびに毎回購読を止める(シールド値は毎バトル0からスタート)
        GameData.instance.GetPlayer().Shield
            .Subscribe(value => battleUIView.UpdatePlayerShieldValue(value))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// 対戦相手のシールド値の初期設定と購読処理
    /// </summary>
    private void SubscribeOpponentShieldValue()
    {
        battleUIView.UpdateOpponentShieldValue(0);

        GameData.instance.GetOpponent().Shield
            .Subscribe(value => battleUIView.UpdateOpponentShieldValue(value))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// プレイヤーのバフ継続時間の初期設定と購読処理
    /// </summary>
    private void SubscribePlayerBuff()
    {
        battleUIView.SetUpPlayerBuff();

        GameData.instance.GetPlayer().Buff
            .Subscribe(data => battleUIView.UpdatePlayerBuff(data))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// プレイヤーのデバフ継続時間の初期設定と購読処理
    /// </summary>
    private void SubscribePlayerDebuff()
    {
        battleUIView.SetUpPlayerDebuff();

        GameData.instance.GetPlayer().Debuff
            .Subscribe(data => battleUIView.UpdatePlayerDebuff(data))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// 対戦相手のバフ継続時間の初期設定と購読処理
    /// </summary>
    private void SubscribeOpponentBuff()
    {
        battleUIView.SetUpOpponentBuff();

        GameData.instance.GetOpponent().Buff
            .Subscribe(data => battleUIView.UpdateOpponentBuff(data))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// 対戦相手のデバフ継続時間の初期設定と購読処理
    /// </summary>
    private void SubscribeOpponentDebuff()
    {
        battleUIView.SetUpOpponentDebuff();

        GameData.instance.GetOpponent().Debuff
            .Subscribe(data => battleUIView.UpdateOpponentDebuff(data))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// バトル終了時に紐付けする予定
    /// </summary>
    public void EndBattle()
    {
        // 購読を停止(ここで停止しておかないと、次の対戦相手のHPの購読が重複して発生してしまう)
        subscriptions?.Dispose();

        // TODO 処理自体は変数内に残っているので、nullにすることで残っている処理も削除する
        //subscriptions = null;
    }
}
