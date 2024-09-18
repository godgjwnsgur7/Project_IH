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

        // Ű �ʱ�ȭ ����
        ItemType = EItemType.Key; // ����: �߰� ���� Ű
        _param = new ItemParam();
        return true;
    }


}