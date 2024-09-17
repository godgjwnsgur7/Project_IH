using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public enum EFixedBossMonsterState
{
    None = 0,
    Pattern1 = 1,
    Pattern2 = 2,
    Pattern3 = 3,
    Idle,
    Dead,
}

public enum EFixedBossMonsterType
{
    SkeletonWizardBoss = 0,
    Max,
}

public class FixedBossMonster : BaseObject, IHitEvent
{
    [field: SerializeField, ReadOnly] private List<BossGimmickPoint> gimmickPointList;
    [field: SerializeField, ReadOnly] private List<MonsterSpawnPoint> monsterSpawnPointList;
    
    [SerializeField] BoxAttackObject attackObject1;
    [SerializeField] BoxAttackObject attackObject3;

    #region Stat
    [field: SerializeField] public float MaxHp { get; protected set; }
    [field: SerializeField] public float CurrHp { get; protected set; }
    #endregion
    
    [SerializeField] public BoxCollider Collider { get; private set; }
    protected Animator animator;

    [SerializeField, ReadOnly] string monsterName = null;

    [SerializeField, ReadOnly] private EFixedBossMonsterState _monsterState;
    public EFixedBossMonsterState MonsterState
    {
        get { return _monsterState; }
        protected set
        {
            if (_monsterState == value)
                return;
            
            switch (value)
            {
                case EFixedBossMonsterState.Idle:
                    UpdateAITick = 3.0f;
                    break;
                default:
                    UpdateAITick = 0.0f;
                    break;
            }

            _monsterState = value;
            PlayAnimation(value);

            switch(value)
            {
                case EFixedBossMonsterState.Pattern2:
                    SpawnMonsterPattern();
                    break;
                case EFixedBossMonsterState.Dead:
                    // 뒤로 물러나며 사라지는 모션 추가 예정
                    break;
            }
        }
    }

    [SerializeField, ReadOnly] UI_BossMonsterStatus monsterStatusUI = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();

        ObjectType = EObjectType.BossMonster;

        this.gameObject.tag = ETag.Monster.ToString();
        this.gameObject.layer = (int)ELayer.Monster;
        Collider.excludeLayers += 1 << (int)ELayer.Monster;

        SetMonsterData();
       
        return true;
    }

    private void SetMonsterData()
    {
        // 엑셀 데이터로 분리 예정 (임시)
        CurrHp = 100000;
        MaxHp = 100000;
        monsterName = "스켈레톤 킹";
    }

    public void SetInfo(List<BossGimmickPoint> gimmickPointList, List<MonsterSpawnPoint> monsterSpawnPointList)
    {
        this.gimmickPointList = gimmickPointList;
        this.monsterSpawnPointList = monsterSpawnPointList;

        attackObject1.SetInfo(Define.ETag.Monster, OnAttackTarget);
        attackObject3.SetInfo(Define.ETag.Monster, OnAttackTarget);

        UIBossMonsterStatusParam param = new UIBossMonsterStatusParam(monsterName, MaxHp);
        monsterStatusUI = Managers.UI.SpawnObjectUI<UI_BossMonsterStatus>(EUIObjectType.UI_BossMOnsterStatus, param);

        StartCoroutine(CoUpdateAI());
    }

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        if (patternNum == 1) attackTarget?.OnHit(new AttackParam(this, true, 1500));
        if (patternNum == 3) attackTarget?.OnHit(new AttackParam(this, true, 5000));
    }

    public void OnHit(AttackParam param = null)
    {
        if (param == null || MonsterState == EFixedBossMonsterState.Dead)
            return;

        UIDamageParam damageParam = new((int)param.damage
            , transform.position + (Collider.size.y * Vector3.up * this.transform.localScale.y)
            , true);
        Managers.UI.SpawnObjectUI<UI_Damage>(EUIObjectType.UI_Damage, damageParam);

        OnDamaged(damageParam.damage);

        if(CurrHp <= 0)
            MonsterState = EFixedBossMonsterState.Dead;
    }

    private void OnDamaged(float damage)
    {
        CurrHp -= damage;
        monsterStatusUI.OnChangedCurrHp(CurrHp);
    }

    #region AI
    public float UpdateAITick { get; protected set; } = 0.0f;

    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            switch (MonsterState)
            {
                case EFixedBossMonsterState.None:
                    MonsterState = EFixedBossMonsterState.Idle;
                    break;
                case EFixedBossMonsterState.Idle:
                    UpdateIdleState();
                    break;
                case EFixedBossMonsterState.Pattern1:
                case EFixedBossMonsterState.Pattern2:
                case EFixedBossMonsterState.Pattern3:
                    UpdatePattern(MonsterState);
                    break;
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    [SerializeField, ReadOnly] int patternNum = 0;
    protected void UpdateIdleState()
    {
        patternNum++;
        if (patternNum > 3)
            patternNum = 1;

        MonsterState = (EFixedBossMonsterState)patternNum;
    }

    protected void UpdatePattern(EFixedBossMonsterState state)
    {
        if (IsEndCurrentState(state))
        {
            MonsterState = EFixedBossMonsterState.Idle;
            return;
        }
    }

    protected void SpawnMonsterPattern()
    {
        foreach(var point in monsterSpawnPointList)
        {
            int randomNum = (UnityEngine.Random.Range(0, (int)ENormalMonsterType.Max));
            ENormalMonsterType spawnMonsterType = (ENormalMonsterType)randomNum;
            SpawnMonsterEffectParam param = new SpawnMonsterEffectParam(spawnMonsterType);
            Managers.Object.SpawnEffectObject(EEffectObjectType.MonsterSpawnEffect, point.transform.position, param);
        }
    }
    #endregion

    #region Animation
    protected void PlayAnimation(EFixedBossMonsterState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString());
    }

    protected void ReplayAnimation(EFixedBossMonsterState state)
    {
        if (IsState(state))
            return;

        animator.Play(state.ToString(), -1, 0f);
    }

    public bool IsState(EFixedBossMonsterState state)
    {
        if (animator == null)
            return false;

        return IsState(animator.GetCurrentAnimatorStateInfo(0), state);
    }

    protected bool IsState(AnimatorStateInfo stateInfo, EFixedBossMonsterState state)
    {
        return stateInfo.IsName(state.ToString());
    }

    public bool IsEndCurrentState(EFixedBossMonsterState state)
    {
        if (animator == null)
        {
            Debug.LogWarning("animator is Null");
            return false;
        }

        // 다른 애니메이션이 재생 중
        if (!IsState(state))
            return false;

        return IsEndState(animator.GetCurrentAnimatorStateInfo(0));
    }
    protected bool IsEndState(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime >= 1.0f;
    }
    #endregion

    #region Animation Clip Event
    public virtual void OnActiveAttackObject(int patternNum) 
    {
        SetActiveAttackObject(patternNum, true);
    }
    public virtual void OnDeactiveAttackObject(int patternNum)
    {
        SetActiveAttackObject(patternNum, false);
    }

    private void SetActiveAttackObject(int patternNum, bool isActive)
    {
        switch (patternNum)
        {
            case 1: attackObject1.SetActiveCollider(isActive); break;
            case 3: attackObject3.SetActiveCollider(isActive); break;
        }
    }
    #endregion
}
