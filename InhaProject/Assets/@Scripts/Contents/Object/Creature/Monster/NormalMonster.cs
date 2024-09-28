using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class NormalMonsterData
{
    public float MaxHp;
    public float CurrHp;
    public float StrikingPower;     // 공격력
    public float MoveSpeed;
    public float ChaseSpeed;
    public float DetectDistance;
    public float ChaseDistance;
    public float AttackDistance;

    public NormalMonsterData(Data.JNormalMonsterData jMonsterData)
    {
        MaxHp = jMonsterData.MaxHp;
        CurrHp = jMonsterData.MaxHp;
        StrikingPower = jMonsterData.StrikingPower;
        MoveSpeed = jMonsterData.MoveSpeed;
        ChaseSpeed = jMonsterData.ChaseSpeed;
        DetectDistance = jMonsterData.DetectDistance;
        ChaseDistance = jMonsterData.ChaseDistance;
        AttackDistance = jMonsterData.AttackDistance;
    }
}

public enum ENormalMonsterState
{
    None,
    Spawn,
    Idle,
    Patrol,
    Chase,
    Attack,
    Hit,
    Dead
}

public enum ENormalMonsterType
{
    SkeletonWarrior = 0,
    SkeletonWizard = 1,
    Max,
}

public class NormalMonster : BaseMonster
{
    [field: SerializeField, ReadOnly] public ENormalMonsterType NormalMonsterType { get; protected set; }
    [SerializeField, ReadOnly] protected NormalMonsterData monsterData;
    [SerializeField, ReadOnly] protected MonsterAttackRange attackRange;
    [SerializeField] protected BaseAttackObject attackObject;
    
    [SerializeField, ReadOnly]
    private ENormalMonsterState _monsterState;
    public virtual ENormalMonsterState MonsterState
    {
        get { return _monsterState; }
        protected set
        {
            if (_monsterState == ENormalMonsterState.Dead)
                return;

            if (_monsterState == value)
                return;

            bool isChangeState = true;
            switch (value)
            {
                case ENormalMonsterState.Idle: isChangeState = IdleStateCondition(); break;
                case ENormalMonsterState.Patrol: isChangeState = PatrolStateCondition(); break;
                case ENormalMonsterState.Chase: isChangeState = ChaseStateCondition(); break;
                case ENormalMonsterState.Attack: isChangeState = AttackStateCondition(); break;
                case ENormalMonsterState.Hit: isChangeState = HitStateCondition(); break;
                case ENormalMonsterState.Dead: isChangeState = DeadStateCondition(); break;
            }

            if (isChangeState == false)
                return;

            switch (_monsterState)
            {
                case ENormalMonsterState.Idle: IdleStateExit(); break;
                case ENormalMonsterState.Patrol: PatrolStateExit(); break;
                case ENormalMonsterState.Chase: ChaseStateExit(); break;
                case ENormalMonsterState.Attack: AttackStateExit(); break;
                case ENormalMonsterState.Hit: HitStateExit(); break;
                case ENormalMonsterState.Dead: DeadStateExit(); break;
            }

            switch (value)
            {
                case ENormalMonsterState.Idle:
                    UpdateAITick = 0.5f;
                    break;
                default:
                    UpdateAITick = 0.0f;
                    break;
            }

            _monsterState = value;
            PlayAnimation(value);

            switch (value)
            {
                case ENormalMonsterState.Idle: IdleStateEnter(); break;
                case ENormalMonsterState.Patrol: PatrolStateEnter(); break;
                case ENormalMonsterState.Chase: ChaseStateEnter(); break;
                case ENormalMonsterState.Attack: AttackStateEnter(); break;
                case ENormalMonsterState.Hit: HitStateEnter(); break;
                case ENormalMonsterState.Dead: DeadStateEnter(); break;
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        MonsterType = EMonsterType.NormalMonster;

        return true;
    }

    protected override void Reset()
    {
        base.Reset();

        attackRange ??= Util.FindChild<MonsterAttackRange>(this.gameObject);
        attackObject ??= Util.FindChild<BaseAttackObject>(this.gameObject, "AttackObject", true);
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        NormalMonsterType = Util.ParseEnum<ENormalMonsterType>(gameObject.name);
        monsterData = new NormalMonsterData(Managers.Data.NormalMonsterDict[(int)MonsterType]);
        monsterData.AttackDistance *= 2;

        attackObject.SetInfo(ETag.Monster, OnAttackTarget);
        attackRange.SetInfo(OnAttackRangeInTarget);

        Managers.UI.SpawnObjectUI<UI_MonsterStatus>(EUIObjectType.UI_MonsterStatus, new UIMonsterStatusParam(this));
        StartCoroutine(CoUpdateAI());
    }

    public override float GetMaxHp() => monsterData.MaxHp;

    #region AI
    public float UpdateAITick { get; protected set; } = 0.0f;
    [SerializeField, ReadOnly] BaseObject ChaseTarget;
    [SerializeField, ReadOnly] BaseObject AttackTarget;

    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            switch (MonsterState)
            {
                case ENormalMonsterState.None:
                    MonsterState = ENormalMonsterState.Idle; 
                    break;
                case ENormalMonsterState.Idle: UpdateIdleState(); break;
                case ENormalMonsterState.Patrol: UpdatePatrolState(); break;
                case ENormalMonsterState.Chase: UpdateChaseState(); break;
                case ENormalMonsterState.Attack: UpdateAttackState(); break;
                case ENormalMonsterState.Hit: UpdateHitState(); break;
                case ENormalMonsterState.Dead: UpdateDeadState(); break;
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    private void ChaseDetectTarget()
    {
        if (ChaseTarget != null)
            return;

        Vector3 subVec = new Vector3(0, this.transform.localScale.y * Collider.center.y, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.right * monsterData.DetectDistance * lookDirX, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.right * lookDirX, out RaycastHit hit, monsterData.DetectDistance, 1 << (int)ELayer.Player)
            && hit.transform.GetComponent<Player>() != null)
        {
            ChaseTarget = hit.transform.GetComponent<Player>();
        }
    }

    private bool IsMovementCheck()
    {
        Vector3 subVec = new Vector3((this.transform.localScale.x * (Collider.center.x + (Collider.size.x / 2)) * lookDirX), 0, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.down, Color.blue);
        if (!Physics.Raycast(transform.position + subVec, Vector3.down, out RaycastHit hit, 2, 1 << (int)ELayer.Platform))
            return false;

        return true;
    }

    private bool IsChaseOrAttackTarget()
    {
        ENormalMonsterState prevState = MonsterState;

        if (AttackTarget != null)
        {
            Vector3 attackTargetDistance = this.transform.position - AttackTarget.transform.position;
            LookLeft = (attackTargetDistance.x > 0.0f);
            MonsterState = ENormalMonsterState.Attack;
            return prevState != MonsterState;
        }

        ChaseDetectTarget();

        if (ChaseTarget != null)
        {
            Vector3 chaseTargetDistance = this.transform.position + Collider.center - ChaseTarget.transform.position;
            LookLeft = (chaseTargetDistance.x > 0.0f);

            if (Mathf.Abs(chaseTargetDistance.x) < 0.5f)
            {
                MonsterState = ENormalMonsterState.Idle;
                return prevState != MonsterState;
            }

            float chaseDistanceSqr = monsterData.ChaseDistance * monsterData.ChaseDistance;
            if (chaseDistanceSqr < chaseTargetDistance.sqrMagnitude)
            {
                ChaseTarget = null;
                MonsterState = ENormalMonsterState.Idle;
                return prevState != MonsterState;
            }
            else
            {
                MonsterState = ENormalMonsterState.Chase;
                return prevState != MonsterState;
            }
        }

        return false;
    }

    #region Idle Motion
    protected virtual bool IdleStateCondition()
    {
        if (CreatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void IdleStateEnter()
    {
        InitRigidVelocityX();
    }

    protected virtual void UpdateIdleState()
    {
        if (IsChaseOrAttackTarget())
            return;

        // Patrol
        {
            int patrolPercent = 10;
            int rand = Random.Range(0, 100);
            if (rand <= patrolPercent)
            {
                LookLeft = (rand % 2 == 0);
                MonsterState = ENormalMonsterState.Patrol;
                return;
            }
        }
    }

    protected virtual void IdleStateExit()
    {

    }
    #endregion

    #region Patrol Motion
    [SerializeField, ReadOnly] float patrolTime = 0.0f;
    protected virtual bool PatrolStateCondition()
    {
        if (AttackTarget != null)
            return false;

        if (ChaseTarget != null)
            return false;

        return true;
    }

    protected virtual void PatrolStateEnter()
    {
        patrolTime = Random.Range(2.0f, 8.0f);
    }

    protected virtual void UpdatePatrolState()
    {
        if (IsMovementCheck() == false)
        {
            InitRigidVelocityX();
            LookLeft = !LookLeft;
            return;
        }

        if (IsChaseOrAttackTarget())
            return;

        SetRigidVelocityX(lookDirX * monsterData.MoveSpeed);
        patrolTime -= Time.deltaTime;

        if (patrolTime < 0.0f)
        {
            MonsterState = ENormalMonsterState.Idle;
        }
    }

    protected virtual void PatrolStateExit()
    {
        patrolTime = 0.0f;
    }
    #endregion

    #region Chase Motion
    protected virtual bool ChaseStateCondition()
    {
        if (CreatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void ChaseStateEnter()
    {

    }

    protected virtual void UpdateChaseState()
    {
        if (IsMovementCheck() == false)
        {
            InitRigidVelocityX();
            ChaseTarget = null;
            MonsterState = ENormalMonsterState.Idle;
            return;
        }

        if (IsChaseOrAttackTarget())
            return;

        if (ChaseTarget == null)
        {
            MonsterState = ENormalMonsterState.Idle;
            return;
        }

        SetRigidVelocityX(lookDirX * monsterData.ChaseSpeed);
    }

    protected virtual void ChaseStateExit()
    {

    }
    #endregion

    #region Attack Motion
    protected virtual bool AttackStateCondition()
    {
        return true;
    }

    protected virtual void AttackStateEnter()
    {
        InitRigidVelocityX();
    }

    protected virtual void UpdateAttackState()
    {
        if (IsEndCurrentState(ENormalMonsterState.Attack))
        {
            MonsterState = ENormalMonsterState.Idle;
            return;
        }
    }

    protected virtual void AttackStateExit()
    {
        InitRigidVelocityX();
    }

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        attackTarget?.OnHit(new AttackParam(this, LookLeft, monsterData.StrikingPower));
    }

    public void OnAttackRangeInTarget(Player player)
    {
        AttackTarget = player;
        if (player != null)
            ChaseTarget = player;
    }
    #endregion

    #region Hit Motion
    Vector3 hitForceDir = Vector3.zero;
    public override void OnHit(AttackParam param = null)
    {
        if (MonsterState == ENormalMonsterState.Dead)
            return;

        if (param == null)
            return;

        UIDamageParam damageParam = new((int)param.damage
            , transform.position + (Collider.size.y * Vector3.up * this.transform.localScale.y * 1.2f)
            , true);
        Managers.UI.SpawnObjectUI<UI_Damage>(EUIObjectType.UI_Damage, damageParam);

        monsterData.CurrHp -= param.damage;
        OnChangedCurrHp?.Invoke(monsterData.CurrHp);

        LookLeft = !param.isAttackerLeft;
        hitForceDir.x = param.pushPower * ((param.isAttackerLeft) ? -1 : 1);
        MonsterState = ENormalMonsterState.Dead;

        if (monsterData.CurrHp > 0)
        {
            if (ChaseTarget == null && param.attacker is BaseObject target)
                ChaseTarget = target;

            if (MonsterState == ENormalMonsterState.Hit)
            {
                ReplayAnimation(ENormalMonsterState.Hit);
                SetRigidVelocity(hitForceDir);
            }
            MonsterState = ENormalMonsterState.Hit;
        }
    }

    protected virtual bool HitStateCondition()
    {


        return true;
    }

    protected virtual void HitStateEnter()
    {
        if (hitForceDir != Vector3.zero)
        {
            SetRigidVelocity(hitForceDir);
        }
    }

    protected virtual void UpdateHitState()
    {
        if (IsEndCurrentState(ENormalMonsterState.Hit) && CreatureFoot.IsLandingGround)
        {
            MonsterState = ENormalMonsterState.Idle;
        }
    }

    protected virtual void HitStateExit()
    {

    }
    #endregion

    #region Dead Motion
    bool isDead = false;
    protected virtual bool DeadStateCondition()
    {
        if (monsterData.CurrHp > 0f)
            return false;

        return true;
    }
    protected virtual void DeadStateEnter()
    {
        coDisappearEffect = StartCoroutine(CoDisappearEffect(1f, () => isDead, OnDeadMonster));
    }

    protected virtual void UpdateDeadState()
    {
        if (IsEndCurrentState(ENormalMonsterState.Dead))
        {
            isDead = true;
            return;
        }
    }

    public void OnDeadMonster()
    {
        // 일정 확률에 따라 아이템 드롭해야 함 (임시)
        int random = UnityEngine.Random.Range((int)EItemType.ItemBox + 1, (int)EItemType.Key);
        Vector3 spawnPosVec = transform.position;
        
        Managers.Object.SpawnItemObject((EItemType)random, spawnPosVec);
        Managers.Resource.Destroy(this.gameObject);
    }

    protected virtual void DeadStateExit() { }
    #endregion
    #endregion

    #region Animation
    protected void PlayAnimation(ENormalMonsterState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString());
    }

    protected void ReplayAnimation(ENormalMonsterState state)
    {
        if (IsState(state))
            return;

        animator.Play(state.ToString(), -1, 0f);
    }

    public bool IsState(ENormalMonsterState state)
    {
        if (animator == null)
            return false;

        return IsState(animator.GetCurrentAnimatorStateInfo(0), state);
    }

    protected bool IsState(AnimatorStateInfo stateInfo, ENormalMonsterState state)
    {
        return stateInfo.IsName(state.ToString());
    }

    public bool IsEndCurrentState(ENormalMonsterState state)
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
    #endregion

    #region Animation Clip Event
    public override void OnActiveAttackObject()
    {
        base.OnActiveAttackObject();
        attackObject.SetActiveCollider(true);
    }

    public override void OnDeactiveAttackObject()
    {
        base.OnDeactiveAttackObject();
        attackObject.SetActiveCollider(false);
        InitRigidVelocity();
    }

    public override void OnMoveEvent(float moveSpeed)
    {
        base.OnMoveEvent(moveSpeed);
        if(IsMovementCheck())
            SetRigidVelocityX(moveSpeed * ((LookLeft) ? -1 : 1));
    }
    #endregion
}
