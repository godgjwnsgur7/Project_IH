using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMap : BaseObject
{
    public string mapName;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        // �� �ʱ�ȭ ����
        gameObject.SetActive(false); // ���� �ʱ�ȭ �� ��Ȱ��ȭ
        return true;
    }

    public virtual void LoadMap()
    {
        if (!_init)
        {
            return;
        }

        gameObject.SetActive(true);
    }

    public virtual void UnloadMap()
    {
        gameObject.SetActive(false);
    }
}
