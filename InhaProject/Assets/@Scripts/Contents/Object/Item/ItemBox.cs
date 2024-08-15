using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemBox : Item
{


    [SerializeField]
    private EItemType itemTypesToSpawn; // 생성될 아이템 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 Player 태그를 가진 경우
        {
            ItemState = EItemState.Use; // 상태를 Use로 변경
        }
    }

    protected override bool UseStateCondition()
    {
        // Use 상태의 조건을 정의 (예: 상태 변경을 허용하는 조건)
        return true;
    }

    protected override void UseStateEnter()
    {
        // Use 상태에 진입할 때 수행할 작업
        base.UseStateEnter();
        //PlayAnimation(EItemState.Use); // Use 상태에 맞는 애니메이션 재생
        Invoke("DestroyAndSpawn", 1.0f); // 애니메이션 재생 후 아이템 파괴 및 생성 (지연시간을 두어 애니메이션이 끝난 후 실행)
    }


    protected override void DestroyAndSpawn()
    {
        base.DestroyAndSpawn();


        // 씬에서 설정된 아이템 타입으로 아이템 생성
        SpawnItems();
    }
    protected override void Start()
    {
        base.Start();


    }
    public override void SetInfo(int templateID = (int)EItemType.ItemBox)
    {
        base.SetInfo(templateID);
        itemTypesToSpawn = EItemType.Item2;

    }

    private void SpawnItems()
    {
        if (objectMgr == null || itemTypesToSpawn == EItemType.None)
        {
            Debug.Log("ObjectMgr 못가져옴 또는 스폰될 아이템 X");
            return;
        }
        Debug.Log("스폰");

        Vector3 spawnPosition = transform.position; // 현재 아이템 위치에 생성
        Quaternion spawnRotation = Quaternion.identity;
        objectMgr.SpawnObject(objectMgr.itemPrefabsPath[(int)itemTypesToSpawn - 1], spawnPosition, spawnRotation); // ObjectMgr를 통해 아이템 생성


    }
}

