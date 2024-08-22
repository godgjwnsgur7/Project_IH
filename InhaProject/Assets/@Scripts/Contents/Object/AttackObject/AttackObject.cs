using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using static Define;

public interface IHitEvent
{
    public void OnHit(AttackParam param = null);
}

public class AttackObject : InitBase
{
    public Rigidbody Rigid {  get; protected set; }
    public BoxCollider Collider { get; protected set; }

    Action<IHitEvent> onAttackTarget;
    ETag targetTag = ETag.Untagged;

    private void Reset()
    {
        Rigid ??= Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Collider ??= Util.GetOrAddComponent<BoxCollider>(this.gameObject);

        Rigid.useGravity = false;
        Rigid.isKinematic = true;
        Collider.isTrigger = true;
        Collider.enabled = false;

        this.gameObject.layer = (int)ELayer.Default;
        this.tag = ETag.Untagged.ToString();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Reset();
        
        return true;
    }

    public void SetInfo(ETag targetTag, Action<IHitEvent> onAttackTarget)
    {
        this.targetTag = targetTag;
        this.onAttackTarget = onAttackTarget;
    }

    public void SetActiveAttackObject(bool isActive)
    {
        Collider.enabled = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(targetTag.ToString()))
        {
            onAttackTarget?.Invoke(other.GetComponent<IHitEvent>());
        }
    }
}
