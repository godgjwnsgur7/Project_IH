using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;


// ������ �������̶� �̸� ������
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
   

    public EItemType ItemType { get; set; } = EItemType.None; // ������ Ÿ��

    private Animator animator; // �ִϸ����� ������Ʈ
    


    public override bool Init()
    {
        if (!base.Init())
            return false;

        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
                                             // �ʿ��� ��� �ٸ� ������Ʈ �ʱ�ȭ
        SetInfo();
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);
        // templateID�� ���� ������ Ÿ�� �� ���� ����
        ItemType = (EItemType)templateID;
    }



    protected virtual void DestroyItem()
    {
        Debug.Log(this +"Destroy");
        // �ڽ��� �ı�
        Managers.Object.DespawnObject(gameObject);
        
    }

    protected virtual void SpawnItems(EItemType spawnItemType)
    {
        Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
        Quaternion spawnRotation = Quaternion.identity;
        Managers.Object.SpawnObject(spawnItemType, spawnPosition,spawnRotation);

    }
}


