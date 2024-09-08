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
    [SerializeField, ReadOnly] protected List<Renderer> rendererList;

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

        rendererList = new List<Renderer>();
        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
            if (child.GetComponent<ParticleSystem>() == null && child.TryGetComponent<Renderer>(out Renderer renderer))
                rendererList.Add(renderer);
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        collisionBarrier.SetInfo(ELayer.Player);
    }

    public abstract void OnHit(AttackParam param = null);

    protected Coroutine coDestroyEffect = null;
    protected IEnumerator CoDestroyEffect(float fadeTime)
    {
        while (true)
        {
            int count = 0;
            float time = fadeTime * 0.01f * Time.deltaTime;
            foreach (Renderer randerer in rendererList)
            {
                Color tempColor = randerer.material.color;
                if (tempColor.a > 0.01f)
                {
                    count++;
                    tempColor.a -= time;
                    if (tempColor.a <= 0f) tempColor.a = 0f;
                    randerer.material.color = tempColor;
                }
            }

            if (count == 0)
                break;

            yield return null;
        }

        coDestroyEffect = null;
        Managers.Resource.Destroy(gameObject);
    }

    #region Animation Clip Event
    public virtual void OnActiveAttackObject() { }
    public virtual void OnDeactiveAttackObject() { }
    #endregion
}
