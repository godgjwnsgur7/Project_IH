using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPack : BaseItem
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            //heal �ڵ�
        }
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.ManaPack;

        return true;
    }
}
