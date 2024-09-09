using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBase : MonoBehaviour
{
    protected bool _init = false;

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    /// <summary>
    /// 재정의 및 확장하지 말고 Init 사용해주세요.
    /// </summary>
    protected virtual void Awake()
    {
        Init();
    }
}
