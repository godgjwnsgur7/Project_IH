using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitEvent
{
    public void OnHit();
}

public class BasePlayerWeapon : InitBase
{
    public BoxCollider Collider { get; private set; }

    Action<IHitEvent> onHitTarget;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<BoxCollider>();
        Collider.enabled = false;

        return true;
    }

    public void SetInfo(Action<IHitEvent> onHitTarget)
    {
        this.onHitTarget = onHitTarget;
    }

    public void SetActiveWeapon(bool isActive)
    {
        Collider.enabled = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IHitEvent>() != null)
        {
            onHitTarget?.Invoke(other.GetComponent<IHitEvent>());
        }
    }
}
