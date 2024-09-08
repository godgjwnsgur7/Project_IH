using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using static Define;
using static UnityEngine.Rendering.DebugUI;

public class MonsterCollisionBarrier : InitBase
{
    public Rigidbody Rigid { get; private set; }
    public CapsuleCollider Collider { get; private set; }

    private void Reset()
    {
        Rigid = Util.GetOrAddComponent<Rigidbody>(gameObject);
        Collider = Util.GetOrAddComponent<CapsuleCollider>(gameObject);
        Rigid.useGravity = false;
        Rigid.isKinematic = true;
        Collider.isTrigger = true;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Rigid ??= GetComponent<Rigidbody>();
        Collider ??= GetComponent<CapsuleCollider>();
        Rigid.excludeLayers += 1 << (int)ELayer.Monster;
        return true;
    }

    public void SetInfo(ELayer includeLayer)
    {
        Rigid.includeLayers += 1 << (int)includeLayer;
        Rigid.isKinematic = false;
        Collider.isTrigger = false;
    }
}