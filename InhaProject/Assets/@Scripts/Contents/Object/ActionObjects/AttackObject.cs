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

public enum EAttackObjectType
{
    PlayerAttackObject,
}

public class AttackObject : InitBase
{
    public Rigidbody Rigid {  get; protected set; }
    public BoxCollider Collider { get; protected set; }

    Action<IHitEvent> onAttackTarget;
    ETag masterTag = ETag.Untagged;

    private void Reset()
    {
        Rigid ??= Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Collider ??= Util.GetOrAddComponent<BoxCollider>(this.gameObject);
        
        Rigid.useGravity = false;
        Rigid.isKinematic = true;
        Collider.isTrigger = true;
        Collider.enabled = true;

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

    public void SetInfo(ETag masterTag, Action<IHitEvent> onAttackTarget)
    {
        this.masterTag = masterTag;
        this.onAttackTarget = onAttackTarget;
    }
    
    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(masterTag.ToString()))
            return;

        if (other.TryGetComponent<IHitEvent>(out var hitEvent))
        {
            onAttackTarget?.Invoke(hitEvent);
        }
    }
}
