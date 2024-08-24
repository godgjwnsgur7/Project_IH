using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemBox : BaseItem
{


    [SerializeField]
    private EItemType itemTypesToSpawn; // ������ ������ 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            DestroyItem();

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("player"))
        {

        }
    }




    protected override void DestroyItem()
    {
        base.DestroyItem();


        // ������ ������ ������ Ÿ������ ������ ����
        SpawnItems(itemTypesToSpawn);
    }

    public override void SetInfo(int templateID = (int)EItemType.ItemBox)
    {
        base.SetInfo(templateID);
        itemTypesToSpawn = EItemType.HealPack;

    }

  
}

