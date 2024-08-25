using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterAttackRange : InitBase
{
    public Rigidbody Rigid { get; private set; } = null;
    public BoxCollider Collider { get; private set; } = null;

    Action<Player> onAttackRangeInTarget;

    private void Reset()
    {
        Rigid ??= Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Collider ??= Util.GetOrAddComponent<BoxCollider>(this.gameObject);

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

    public void SetInfo(Action<Player> onAttackRangeInTarget, Monster attacker)
    {
        this.onAttackRangeInTarget = onAttackRangeInTarget;
        Collider.center += attacker.Collider.center;
        Collider.size = new Vector3(2, attacker.Collider.size.y, attacker.MonsterInfo.AttackDistance * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(ETag.Player.ToString()))
        {
            onAttackRangeInTarget?.Invoke(other.GetComponent<Player>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(ETag.Player.ToString()))
        {
            onAttackRangeInTarget?.Invoke(null);
        }
    }
}
