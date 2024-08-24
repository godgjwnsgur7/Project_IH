using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;


// 아이템 프리펩이랑 이름 같을것
public enum EItemType
{
    None,
    ItemBox,
    HealPotion, 
    ManaPotion,
    HealPack,
    ManaPack,
    Key,
    
    Max
}

public class BaseItem : BaseObject
{
   

    public EItemType ItemType { get; set; } = EItemType.None; // 아이템 타입

    private Animator animator; // 애니메이터 컴포넌트
    


    public override bool Init()
    {
        if (!base.Init())
            return false;

        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
                                             // 필요한 경우 다른 컴포넌트 초기화
        SetInfo();
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);
        // templateID에 따라 아이템 타입 및 상태 설정
        ItemType = (EItemType)templateID;
    }



    protected virtual void DestroyItem()
    {
        Debug.Log(this +"Destroy");
        // 자신을 파괴
        Managers.Object.DespawnObject(gameObject);
        
    }

    protected virtual void SpawnItems(EItemType spawnItemType)
    {
        Vector3 spawnPosition = transform.position; // 현재 아이템 위치에 생성
        Quaternion spawnRotation = Quaternion.identity;
        Managers.Object.SpawnObject(spawnItemType, spawnPosition,spawnRotation);

    }
}


