using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class ItemBox : BaseItem
{

 
    [SerializeField]
    private EItemType itemTypesToSpawn; // 생성될 아이템 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) // 충돌한 객체가 Player 태그를 가진 경우
        {
            Vector3 spawnPosition = transform.position; // 현재 아이템 위치에 생성
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



