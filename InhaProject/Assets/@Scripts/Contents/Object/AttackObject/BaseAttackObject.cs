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

public class BaseAttackObject : InitBase
{
    public BoxCollider Collider { get; private set; }

    Action<IHitEvent> onAttackTarget;
    ETag targetTag = ETag.Untagged;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<BoxCollider>();
        Collider.enabled = false;

        this.gameObject.layer = (int)ELayer.Default;
        this.tag = ETag.Untagged.ToString();

        return true;
    }

    public void SetInfo(ETag targetTag, Action<IHitEvent> onAttackTarget)
    {
        this.targetTag = targetTag;
        this.onAttackTarget = onAttackTarget;
    }

    public void SetActiveWeapon(bool isActive)
    {
        Collider.enabled = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.tag}, {targetTag.ToString()}");
        if (other.tag.Equals(targetTag.ToString()))
        {
            if (onAttackTarget == null)
                Debug.Log("æÍ ≥Œ¿” §ª§ª ø÷?");

            onAttackTarget?.Invoke(other.GetComponent<IHitEvent>());
        }
    }
}
