using UnityEngine;

/// <summary>
/// 派生クラスに対するシングルトンの基本機能を提供する抽象クラス
/// </summary>
/// <typeparam name="T">シングルトンのインスタンスの型</typeparam>
public abstract class AbstractSingleton<T> : MonoBehaviour where T : Component  // where T: Componentで、TがComponenrクラスまたはその継承クラスであることを指定している(whereは型の制約を指定するもの)
{
    public static T instance;


    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;  // このクラスをT型に変換(asでキャスト)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
