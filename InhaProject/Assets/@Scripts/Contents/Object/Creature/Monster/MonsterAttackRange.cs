using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterAttackRange : InitBase
{
    public Rigidbody Rigid { get; private set; } = null;
    public SphereCollider Collider { get; private set; } = null;

    Action<Player> onAttackRangeInTarget;

    private void Reset()
    {
        Rigid ??= Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Collider ??= Util.GetOrAddComponent<SphereCollider>(this.gameObject);

        Rigid.useGravity = false;
        Rigid.isKinematic = true;
        Collider.isTrigger = true;

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

    public void SetInfo(Action<Player> onAttackRangeInTarget, BaseMonster attacker)
    {
        this.onAttackRangeInTarget = onAttackRangeInTarget;
        Collider.center += attacker.Collider.center;
        Collider.radius = attacker.AttackDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(ETag.Player.ToString()))
        {
            onAttackRangeInTarget?.Invoke(other.GetComponent<Player>());
        }
    }
}
