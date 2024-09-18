using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public enum EMonsterType
{
    NormalMonster = 0,
    NamedMonster = 1, // 미사용 중
    BossMonster = 2,
    Max
}

public abstract class BaseMonster : Creature, IHitEvent
{
    [field: SerializeField, ReadOnly] public EMonsterType MonsterType { get; protected set; }
    [SerializeField, ReadOnly] protected MonsterCollisionBarrier collisionBarrier;
    [SerializeField, ReadOnly] protected List<Renderer> rendererList;

    public Action<float> OnChangedCurrHp;

    protected override void Reset()
    {
        base.Reset();

        collisionBarrier ??= Util.FindChild<MonsterCollisionBarrier>(this.gameObject);

        rendererList = new List<Renderer>();
        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
            if (child.GetComponent<ParticleSystem>() == null && 
                child.GetComponent<TrailRenderer>() == null &&
                child.TryGetComponent<Renderer>(out Renderer renderer))
                rendererList.Add(renderer);
    }

    protected virtual void Start()
    {
        SetInfo();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Monster;

        this.gameObject.layer = (int)ELayer.Monster;
        this.tag = ETag.Monster.ToString();
        Collider.excludeLayers += 1 << (int)ELayer.Monster;

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        collisionBarrier?.SetInfo(ELayer.Player);
        Managers.Object.baseMonsters.Add(this);
    }

    public abstract float GetMaxHp();
    public abstract void OnHit(AttackParam param = null);
    public override Vector3 GetTopPosition() => base.GetTopPosition();
    public override float GetSizeX() => base.GetSizeX();

    protected Coroutine coDisappearEffect = null;
    protected IEnumerator CoDisappearEffect(float fadeTime, Func<bool> waitCondition, Action onDisappear = null)
    {
        yield return new WaitUntil(waitCondition);

        while (true)
        {
            int count = 0;
            float time = fadeTime * Time.deltaTime;
            foreach (Renderer randerer in rendererList)
            {
                if(randerer != null && randerer.material != null)
                {
                    Color tempColor = randerer.material.color;
                    if (tempColor.a > 0.01f)
                    {
                        count++;
                        tempColor.a -= time;
                        if (tempColor.a <= 0.1f) tempColor.a = 0f;
                        randerer.material.color = tempColor;
                    }
                }
            }

            if (count == 0)
                break;

            yield return null;
        }

        coDisappearEffect = null;
        onDisappear?.Invoke();
    }

    #region Animation Clip Event
    public virtual void OnActiveAttackObject() { }
    public virtual void OnDeactiveAttackObject() { }
    public virtual void OnMoveEvent(float moveSpeed) { }
    #endregion
}
