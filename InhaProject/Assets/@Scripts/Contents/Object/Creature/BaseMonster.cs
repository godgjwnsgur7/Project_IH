using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterSkillType
{
    ScratchingAttack = 1,
    
}

public class BaseMonster : Creature
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }

    protected override void Reset()
    {
        base.Reset();

        
    }
}
