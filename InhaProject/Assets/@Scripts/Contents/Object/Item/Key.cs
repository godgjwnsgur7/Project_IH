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

    protected override bool UseStateCondition()
    {
        // Ű ��� ������ Ȯ�� (��: �÷��̾ �� �տ� �ִ���)
        return true;
    }

    protected override void UseStateEnter()
    {
        base.UseStateEnter();
        // Ű ���� ���� (��: �� ����)
        Managers.Game.UseKey(ItemType);    
        DestroyItem();
    }
}