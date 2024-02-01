using UnityEngine;

public class BattleItemSlot : MonoBehaviour
{
    private bool hasBattleItem = false;  // アイテムが子オブジェクトにあるかどうか。インベントリの並び替えで利用する
    public bool HasBattleItem
    {
        get => hasBattleItem;
        set => hasBattleItem = value;
    }
}
