using UnityEngine;

public class Key : BaseItem, IInventoryItem
{

    public string Name
    {
        get { return "Key"; }
    }

    public Sprite _Image = null;

    public Sprite Image
    {
        get { return _Image; }
    }

    public int _count = 1;

    public int Count
    {
        get { return _count; }
        set { _count = value; }
    }

    ItemParam _param;
    public ItemParam Param
    {
        get { return _param; }
        set { _param = value; }
    }

    public void OnPickup()
    {

    }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        // 키 초기화 로직
        ItemType = EItemType.Key; // 예시: 중간 보스 키
        _param = new ItemParam();
        return true;
    }


}