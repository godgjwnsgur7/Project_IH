using UnityEngine;

public class Key : BaseItem
{
    public override bool Init()
    {
        if (!base.Init())
            return false;

        // Ű �ʱ�ȭ ����
        ItemType = EItemType.Key; // ����: �߰� ���� Ű
        return true;
    }

  

}