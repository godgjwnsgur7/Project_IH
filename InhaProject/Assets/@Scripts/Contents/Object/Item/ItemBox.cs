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

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if(collision.collider.CompareTag("Player"))
        {

            DestroyItem();
        }
    }




    protected override void DestroyItem()
    {
        Managers.Object.DespawnObject(transform.parent.gameObject);

        // ������ ������ ������ Ÿ������ ������ ����
        SpawnItems(itemTypesToSpawn);
    }

    public override void SetInfo(int templateID = (int)EItemType.ItemBox)
    {
        base.SetInfo(templateID);

    }

  
}

