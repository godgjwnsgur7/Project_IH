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
        //Invoke("DestroyItem", 1.0f); // �ִϸ��̼� ��� �� ������ �ı� �� ���� (�����ð��� �ξ� �ִϸ��̼��� ���� �� ����)
        DestroyItem();
    }


    protected override void DestroyItem()
    {
        base.DestroyItem();


        // ������ ������ ������ Ÿ������ ������ ����
        SpawnItems(itemTypesToSpawn);
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

  
}

