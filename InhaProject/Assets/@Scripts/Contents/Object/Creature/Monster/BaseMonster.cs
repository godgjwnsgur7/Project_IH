using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public enum EMonsterSkillType
{
    ScratchingAttack = 1,
    
}

public enum EMonsterType
{
    NormalMonster = 0,

}

public abstract class BaseMonster : Creature, IHitEvent
{
    [field: SerializeField, ReadOnly] public EMonsterType MonsterType { get; protected set; }
    [SerializeField, ReadOnly] MonsterCollisionBarrier collisionBarrier;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Monster;

        this.gameObject.layer = (int)ELayer.Monster;
        this.tag = ETag.Monster.ToString();

        return true;
    }

    protected override void Reset()
    {
        base.Reset();

        collisionBarrier ??= Util.FindChild<MonsterCollisionBarrier>(this.gameObject);
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        collisionBarrier.SetInfo(ELayer.Player);
    }

    public abstract void OnHit(AttackParam param = null);
}
