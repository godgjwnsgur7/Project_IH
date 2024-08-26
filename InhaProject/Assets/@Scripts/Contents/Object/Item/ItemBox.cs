using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemBox : BaseItem
{


    [SerializeField]
    private EItemType itemTypesToSpawn; // 생성될 아이템 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 Player 태그를 가진 경우
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

        // 씬에서 설정된 아이템 타입으로 아이템 생성
        SpawnItems(itemTypesToSpawn);
    }

    public override void SetInfo(int templateID = (int)EItemType.ItemBox)
    {
        base.SetInfo(templateID);

    }

  
}

