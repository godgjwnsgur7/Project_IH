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

        // 맵 초기화 로직
        gameObject.SetActive(false); // 맵을 초기화 시 비활성화
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
