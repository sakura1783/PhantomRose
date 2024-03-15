using UnityEngine;

public class FloatingMessageManager : AbstractSingleton<FloatingMessageManager>
{
    [SerializeField] private FloatingMessage floatingMessagePrefab;

    [SerializeField] private Transform playerFloatingTran;
    [SerializeField] private Transform opponentFloatingTran;


    /// <summary>
    /// フロート表示の生成
    /// </summary>
    /// <param name="value"></param>
    /// <param name="spriteId"></param>
    /// <param name="owner">ownerの位置に生成する</param>
    public void GenerateFloatingMessage(int value, int spriteId, OwnerStatus owner)
    {
        var generateTran = owner == OwnerStatus.Player ? playerFloatingTran : opponentFloatingTran;

        var messageObj = Instantiate(floatingMessagePrefab, generateTran);
        messageObj.SetUp(value, spriteId, generateTran.childCount);
    }
}
