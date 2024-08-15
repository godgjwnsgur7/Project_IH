using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemBox : Item
{


    [SerializeField]
    private EItemType itemTypesToSpawn; // ������ ������ 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            ItemState = EItemState.Use; // ���¸� Use�� ����
        }
    }

    protected override bool UseStateCondition()
    {
        // Use ������ ������ ���� (��: ���� ������ ����ϴ� ����)
        return true;
    }

    protected override void UseStateEnter()
    {
        // Use ���¿� ������ �� ������ �۾�
        base.UseStateEnter();
        //PlayAnimation(EItemState.Use); // Use ���¿� �´� �ִϸ��̼� ���
        Invoke("DestroyAndSpawn", 1.0f); // �ִϸ��̼� ��� �� ������ �ı� �� ���� (�����ð��� �ξ� �ִϸ��̼��� ���� �� ����)
    }


    protected override void DestroyAndSpawn()
    {
        base.DestroyAndSpawn();


        // ������ ������ ������ Ÿ������ ������ ����
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
            Debug.Log("ObjectMgr �������� �Ǵ� ������ ������ X");
            return;
        }
        Debug.Log("����");

        Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
        Quaternion spawnRotation = Quaternion.identity;
        objectMgr.SpawnObject(objectMgr.itemPrefabsPath[(int)itemTypesToSpawn - 1], spawnPosition, spawnRotation); // ObjectMgr�� ���� ������ ����


    }
}

