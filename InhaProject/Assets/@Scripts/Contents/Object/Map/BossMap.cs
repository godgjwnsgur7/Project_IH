using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMap : BaseMap
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        isCleared = false;
        // �� �ʱ�ȭ ���� �߰�

        return true;
    }
}