using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Item2 : Item
{
    protected override void Start()
    {
        base.Start();
        // Item2�� �߰����� �ʱ�ȭ �۾�
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� Player �±׸� ���� ���
        {
            Debug.Log("�浹 üũ");
            if (Input.GetKey(KeyCode.Z)) //�ӽ� ���
            {
                Debug.Log("züũ");
                ItemState = EItemState.Use; // ���¸� Use�� ����
                /*
                 ��ȣ�ۿ� �Լ�
                 */
                DestroyAndSpawn();
            } 
        }
    }
    protected override bool UseStateCondition()
    {
        // Item10�� Use ���� ����
        return base.UseStateCondition();
    }

    protected override void UseStateEnter()
    {
        // Item10�� Use ���� ���� �� �۾�
        base.UseStateEnter();
    }

    protected override void UsedStateEnter()
    {
        // Item10�� Used ���� ���� �� �۾�
        base.UsedStateEnter();
    }
}