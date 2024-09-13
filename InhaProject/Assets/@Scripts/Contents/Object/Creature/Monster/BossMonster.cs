using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Creature, IHitEvent
{
    public virtual void OnHit(AttackParam param = null)
    {
        
    }
}
