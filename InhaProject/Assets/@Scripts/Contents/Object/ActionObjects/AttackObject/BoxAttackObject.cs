using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAttackObject : BaseAttackObject
{
    public Rigidbody Rigid { get; protected set; }
    public BoxCollider Collider { get; protected set; }

    protected override void Reset()
    {
        base.Reset();

        Rigid ??= Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Collider ??= Util.GetOrAddComponent<BoxCollider>(this.gameObject);

        Rigid.useGravity = false;
        Rigid.isKinematic = true;
        Collider.isTrigger = true;
        Collider.enabled = true;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        AttackObjectType = EAttackObjectType.Box;

        return true;
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
