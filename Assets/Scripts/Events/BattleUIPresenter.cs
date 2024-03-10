using UnityEngine;
using UniRx;

public class BattleUIPresenter : MonoBehaviour
{
    [SerializeField] private BattleUIView battleUIView;

    //private IDisposable subscription;
    private CompositeDisposable subscriptions = new();  // 購読停止したい処理がいくつかあるのでこれに変更


    /// <summary>
    /// バトル毎に実行する敵のステータスの初期化と購読処理
    /// </summary>
    public void SubscribeEveryBattle()
    {
        SubscribeOpponentHp();
        SubscribeOpponentShieldValue();
        SubscribeOpponentBuff();
        SubscribeOpponentDebuff();
    }

    /// <summary>
    /// プレイヤーのHP用UIをMVPパターンで設定
    /// </summary>
    public void SubscribePlayerHp()
    {
        battleUIView.SetUpPlayerHp(GameDataManager.instance.gameData.GetPlayer().Hp.Value);

        // プレイヤーのHPの購読処理(HPSlider更新用にprevValueも使う場合)
        //GameData.instance.GetPlayer().Hp
        //    .Zip(GameData.instance.GetPlayer().Hp.Skip(1), (prevValue, currentValue) => (prevValue, currentValue))  // Zipオペレータは2つのOvservableシーケンスを組み合わせ、Skipオペレータはシーケンスの最初の値を無視する
        //    .Subscribe(values => battleUIView.UpdatePlayerHp(values.prevValue, values.currentValue))
        //    .AddTo(this);
        GameDataManager.instance.gameData.GetPlayer().Hp
            .Subscribe(value => battleUIView.UpdatePlayerHp(value))
            .AddTo(this);
    }

    /// <summary>
    /// プレイヤーのシールド値の初期設定と購読処理
    /// </summary>
    public void SubscribePlayerShieldValue()
    {
        battleUIView.UpdatePlayerShieldValue(0);

        GameDataManager.instance.gameData.GetPlayer().Shield
            .Subscribe(value => battleUIView.UpdatePlayerShieldValue(value))
            .AddTo(this);
    }

    /// <summary>
    /// プレイヤーのバフの初期設定と購読処理
    /// </summary>
    public void SubscribePlayerBuff()
    {
        battleUIView.SetUpPlayerBuff();

        GameDataManager.instance.gameData.GetPlayer().Buff
            .Subscribe(data => battleUIView.UpdatePlayerBuff(data))
            .AddTo(this);

        GameDataManager.instance.gameData.GetPlayer().BuffDuration
            .Subscribe(value =>
            {
                battleUIView.UpdatePlayerBuffDuration(value);

                // 継続時間が0以下の場合
                if (value <= 0)
                {
                    battleUIView.SetUpPlayerBuff();
                }
            })
            .AddTo(this);
    }

    /// <summary>
    /// プレイヤーのデバフ継続時間の初期設定と購読処理
    /// </summary>
    public void SubscribePlayerDebuff()
    {
        battleUIView.SetUpPlayerDebuff();

        GameDataManager.instance.gameData.GetPlayer().Debuff
            .Subscribe(data => battleUIView.UpdatePlayerDebuff(data))
            .AddTo(this);

        GameDataManager.instance.gameData.GetPlayer().DebuffDuration
            .Subscribe(value =>
            {
                battleUIView.UpdatePlayerDebuffDuration(value);

                if (value <= 0)
                {
                    battleUIView.SetUpPlayerDebuff();
                }
            })
            .AddTo(this);
    }

    /// <summary>
    /// 対戦相手のHP用UIをMVPパターンで設定
    /// </summary>
    private void SubscribeOpponentHp()
    {
        battleUIView.SetUpOpponentHp(GameDataManager.instance.gameData.GetOpponent().Hp.Value);

        // 対戦相手のHPの購読処理(対戦相手が変わるたびに購読するので、AddToではなく、対戦相手がいなくなるたびに毎回購読を止める必要がある)
        GameDataManager.instance.gameData.GetOpponent().Hp
            .Subscribe(value => battleUIView.UpdateOpponentHp(value))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// 対戦相手のシールド値の初期設定と購読処理
    /// </summary>
    private void SubscribeOpponentShieldValue()
    {
        battleUIView.UpdateOpponentShieldValue(0);

        GameDataManager.instance.gameData.GetOpponent().Shield
            .Subscribe(value => battleUIView.UpdateOpponentShieldValue(value))
            .AddTo(subscriptions);
    }

    /// <summary>
    /// 対戦相手のバフ継続時間の初期設定と購読処理
    /// </summary>
    private void SubscribeOpponentBuff()
    {
        battleUIView.SetUpOpponentBuff();

        GameDataManager.instance.gameData.GetOpponent().Buff
            .Subscribe(data => battleUIView.UpdateOpponentBuff(data))
            .AddTo(subscriptions);

        GameDataManager.instance.gameData.GetOpponent().BuffDuration
            .Subscribe(value =>
            {
                battleUIView.UpdateOpponentBuffDuration(value);

                if (value <= 0)
                {
                    battleUIView.SetUpOpponentBuff();
                }
            })
            .AddTo(subscriptions);
    }

    /// <summary>
    /// 対戦相手のデバフ継続時間の初期設定と購読処理
    /// </summary>
    private void SubscribeOpponentDebuff()
    {
        battleUIView.SetUpOpponentDebuff();

        GameDataManager.instance.gameData.GetOpponent().Debuff
            .Subscribe(data => battleUIView.UpdateOpponentDebuff(data))
            .AddTo(subscriptions);

        GameDataManager.instance.gameData.GetOpponent().DebuffDuration
            .Subscribe(value =>
            {
                battleUIView.UpdateOpponentDebuffDuration(value);

                if (value <= 0)
                {
                    battleUIView.SetUpOpponentDebuff();
                }
            })
            .AddTo(subscriptions);
    }

    /// <summary>
    /// バトル終了時に紐付けする予定
    /// </summary>
    public void EndBattle()
    {
        // TODO 購読を停止(ここで停止しておかないと、次の対戦相手のHPの購読が重複して発生してしまう)
        //subscriptions?.Dispose();
        subscriptions.Clear();

        // TODO 処理自体は変数内に残っているので、nullにすることで残っている処理も削除する
        //subscriptions = null;
    }
}
