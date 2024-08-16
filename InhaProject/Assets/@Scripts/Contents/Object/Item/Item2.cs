using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Item2 : BaseItem
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
                DestroyItem();
            } 
        }
    }
    protected override bool UseStateCondition()
    {
        // Item2�� Use ���� ����
        return base.UseStateCondition();
    }

    protected override void UseStateEnter()
    {
        // Item2�� Use ���� ���� �� �۾�
        base.UseStateEnter();
    }

    protected override void UsedStateEnter()
    {
        // Item2�� Used ���� ���� �� �۾�
        base.UsedStateEnter();
    }
}