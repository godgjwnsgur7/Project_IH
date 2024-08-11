using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using static TMPro.Examples.TMP_ExampleScript_01;

public abstract class BaseObject : InitBase
{
    public EObjectType ObjectType { get; protected set; } = EObjectType.None;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(int templateID) { }

    protected virtual void FlipX(bool isLeft)
    {
        float rotationY = 90;
        if (isLeft) rotationY *= -1;

        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
