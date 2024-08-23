using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{

    public BaseStage currentStage;

    public void Init()
    {
        if (currentStage != null)
        {
            currentStage.Init();
            currentStage.StartStage();
        }
    }

    public void Clear()
    {
        // ���� Ŭ���� ó�� ����

    }

    public void UseKey(EItemType keyType)
    {
        // ���� �ʿ��� Ű�� ����� �� �ִ��� Ȯ��
        if (currentStage != null &&  keyType == EItemType.Key)
        {
            // ���� �� Ŭ���� ���� �� �� �ִ� Ű ���
            Debug.Log("Boss Map");
            currentStage.NextMap();
        }
        else if (keyType == EItemType.Key)
        {
            // �Ϲ� �ʿ��� �߰� ���� ���� ���� Ű ���
            Debug.Log("Middle Boss Door Opened!");
            currentStage.NextMap();
        }
    }

}
  