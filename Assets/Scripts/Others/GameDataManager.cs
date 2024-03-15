using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GameDataManager : AbstractSingleton<GameDataManager>
{
    public GameData gameData;


    protected override void Awake()
    {
        base.Awake();

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => PlayerPrefsHelper.SaveGameData());

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.L))
            .Subscribe(_ => PlayerPrefsHelper.LoadGameData());
    }
}
