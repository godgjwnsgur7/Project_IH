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

public abstract class BaseItem : BaseObject
{
    public EItemType ItemType { get; set; } = EItemType.None; // ������ Ÿ��

    protected Animator animator; // �ִϸ����� ������Ʈ

    protected bool isPlayerInRange = false;

    

    Player player = null;

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
        Debug.Log(this + "Destroy");
        // �ڽ��� �ı�
        Managers.Object.DespawnObject(gameObject);

    }

    protected virtual void SpawnItems(EItemType spawnItemType)
    {
        Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
        Quaternion spawnRotation = Quaternion.identity;

        switch (spawnItemType)
        {
            case EItemType.HealPack:
            case EItemType.ManaPack:
                spawnPosition.y += 1f;
                break;
        }
        Managers.Object.SpawnObject(spawnItemType, spawnPosition, spawnRotation);

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            isPlayerInRange = true;
            player = collision.collider.gameObject.GetComponent<Player>();
            switch (ItemType)
            {
                case EItemType.HealPack:
                    // player ��
                    break;
                case EItemType.ManaPack:
                    // player ���� ȸ��
                    break;
                default:
                    //input �κ��丮��
                    break;



            }
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            isPlayerInRange = false;
        }

    }
}


