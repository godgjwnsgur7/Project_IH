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
    Box,
    Particle,
}

public class BaseAttackObject : InitBase
{
    [field: SerializeField, ReadOnly] 
    public EAttackObjectType AttackObjectType { get; protected set; }
    protected Action<IHitEvent> onAttackTarget;
    protected ETag masterTag = ETag.Untagged;

    protected virtual void Reset()
    {
        this.gameObject.layer = (int)ELayer.Default;
        this.tag = ETag.Untagged.ToString();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(ETag masterTag, Action<IHitEvent> onAttackTarget)
    {
        this.masterTag = masterTag;
        this.onAttackTarget = onAttackTarget;
    }
    
    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
