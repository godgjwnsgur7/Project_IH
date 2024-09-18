using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class ItemBox : BaseItem
{

 
    [SerializeField]
    private EItemType itemTypesToSpawn; // ������ ������ 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
            Quaternion spawnRotation = Quaternion.identity;

            Managers.Object.DespawnObject(transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
            if(itemTypesToSpawn == EItemType.Key)
            {
                Key key = new Key();
                Managers.Game.Player.OnGetInventroyItem(key);
            }
            else
                Managers.Object.SpawnItemObject(itemTypesToSpawn, spawnPosition, spawnRotation);
        }
    }
  



    public override void SetInfo(int templateID = (int)EItemType.ItemBox)
    {
        base.SetInfo(templateID);

    }

  
}



