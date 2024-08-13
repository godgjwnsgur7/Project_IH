using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitEvent
{
    public void OnHit();
}

public class BasePlayerWeapon : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IHitEvent>() != null)
        {
            Debug.Log($"공격 물체 감지 : {other}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
