using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BoxAttackObject : BaseAttackObject
{
    [field: SerializeField] public Rigidbody Rigid { get; protected set; }
    [field: SerializeField] public BoxCollider Collider { get; protected set; }

    protected override void Reset()
    {
        base.Reset();

        Rigid ??= Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Collider ??= Util.GetOrAddComponent<BoxCollider>(this.gameObject);

        Rigid.useGravity = false;
        Rigid.isKinematic = true;
        Collider.isTrigger = true;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        AttackObjectType = EAttackObjectType.Box;
        Rigid = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();

        return true;
    }
    public override void SetInfo(ETag masterTag, Action<IHitEvent> onAttackTarget)
    {
        base.SetInfo(masterTag, onAttackTarget);
    }

    public override void SetActiveCollider(bool isActive)
    {
        Collider.enabled = isActive;
    }

    protected override void OnAttackTarget(IHitEvent hitEvent)
    {
        base.OnAttackTarget(hitEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(masterTag.ToString()))
            return;

        if (other.TryGetComponent<IHitEvent>(out var hitEvent))
        {
            OnAttackTarget(hitEvent);
        }
    }
}
