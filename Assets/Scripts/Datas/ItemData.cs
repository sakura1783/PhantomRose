using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;
    public string itemName;

    public int spriteId;

    [Multiline] public string description;

    /// <summary>
    /// コンストラクタ。スプレッドシートから情報を取得
    /// </summary>
    /// <param name="datas"></param>
    public ItemData(string[] datas)
    {
        id = int.Parse(datas[0]);
        itemName = datas[1];
        spriteId = int.Parse(datas[2]);
        description = datas[3];
    }
}
