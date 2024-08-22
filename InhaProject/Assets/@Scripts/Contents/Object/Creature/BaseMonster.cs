using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

public enum EMonsterType
{
    Wizard,
}

public enum  EMonsterState
{
    None,
    Idle,
    Walk, // 순찰
    Move, // 추격
    Attack,
    Hit,
    Dead
}

public class BaseMonster : Creature, IHitEvent
{
    public EMonsterType MonsterType { get; protected set; }
    public MonsterData MonsterData { get; protected set; }

    [SerializeField] protected AttackObject attackObject;
    [SerializeField, ReadOnly] protected MonsterAttackRange attackRange;

    [SerializeField, ReadOnly]
    private EMonsterState _monsterState;
    public virtual EMonsterState MonsterState
    {
        get { return _monsterState; }
        protected set
        {
            if (_monsterState == value)
                return;

            bool isChangeState = true;
            switch (value)
            {
                case EMonsterState.Idle:
                    isChangeState = IdleStateCondition();
                    break;
                case EMonsterState.Walk:
                    isChangeState = WalkStateCondition();
                    break;
                case EMonsterState.Move:
                    isChangeState = MoveStateCondition();
                    break;
                case EMonsterState.Attack:
                    isChangeState = AttackStateCondition();
                    break;
                case EMonsterState.Hit:
                    isChangeState = HitStateCondition();
                    break;
                case EMonsterState.Dead:
                    isChangeState = DeadStateCondition();
                    break;
            }

            if (isChangeState == false)
                return;

            switch (_monsterState)
            {
                case EMonsterState.Idle:
                    IdleStateExit();
                    break;
                case EMonsterState.Walk:
                    WalkStateExit();
                    break;
                case EMonsterState.Move:
                    MoveStateExit();
                    break;
                case EMonsterState.Attack:
                    AttackStateExit();
                    break;
                case EMonsterState.Hit:
                    HitStateExit();
                    break;
                case EMonsterState.Dead:
                    DeadStateExit();
                    break;
            }

            _monsterState = value;
            PlayAnimation(value);
            if (_monsterState != value)
            {
                switch (value)
                {
                    case EMonsterState.Idle:
                        UpdateAITick = 0.5f;
                        break;
                    case EMonsterState.Dead:
                        UpdateAITick = 1.0f;
                        break;
                    default:
                        UpdateAITick = 0.0f;
                        break;
                }
            }

            switch (value)
            {
                case EMonsterState.Idle:
                    IdleStateEnter();
                    break;
                case EMonsterState.Walk:
                    WalkStateEnter();
                    break;
                case EMonsterState.Move:
                    MoveStateEnter();
                    break;
                case EMonsterState.Attack:
                    AttackStateEnter();
                    break;
                case EMonsterState.Hit:
                    HitStateEnter();
                    break;
                case EMonsterState.Dead:
                    DeadStateEnter();
                    break;
            }
        }
    }

    protected override void Reset()
    {
        base.Reset();
        attackObject ??= Util.FindChild<AttackObject>(this.gameObject);
        attackRange ??= Util.FindChild<MonsterAttackRange>(this.gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Monster;

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        MonsterType = Util.ParseEnum<EMonsterType>(gameObject.name); // 임시
        MonsterData = Managers.Data.MonsterDict[(int)MonsterType];

        attackObject.SetInfo(ETag.Player, OnAttackTarget);
        attackRange.SetInfo(OnAttackRangeInTarget, this);

        StartCoroutine(CoUpdateAI());
    }

    #region AI
    public float UpdateAITick { get; protected set; } = 0.0f;
    [SerializeField, ReadOnly] Player ChaseTarget;
    [SerializeField, ReadOnly] Player AttackTarget;

    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            switch (MonsterState)
            {
                case EMonsterState.None:
                    MonsterState = EMonsterState.Idle;
                    break;
                case EMonsterState.Idle:
                    UpdateIdleState();
                    break;
                case EMonsterState.Walk:
                    UpdateWalkState();
                    break;
                case EMonsterState.Move:
                    UpdateMoveState();
                    break;
                case EMonsterState.Attack:
                    UpdateAttackState();
                    break;
                case EMonsterState.Hit:
                    UpdateHitState();
                    break;
                case EMonsterState.Dead:
                    UpdateDeadState();
                    break;
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    private void ChaseDetectTarget()
    {
        Vector3 subVec = new Vector3(0, Collider.center.y, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.right * MonsterData.ChaseDistance * lookDirX, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.right * lookDirX, out RaycastHit hit, MonsterData.ChaseDistance, 1 << (int)ELayer.Player)
            && hit.transform.GetComponent<Player>() != null)
        {
            ChaseTarget = hit.transform.GetComponent<Player>();
        }
    }

    private bool IsMovementCheck()
    {
        Vector3 subVec = new Vector3((0.1f + Collider.center.x + (Collider.size.x / 2)) * lookDirX, 0, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.down* 2, Color.blue);
        if (!Physics.Raycast(transform.position + subVec, Vector3.down, out RaycastHit hit, 2, 1 << (int)ELayer.Platform))
            return false;

        return true;
    }
     
    private bool IsChaseOrAttackTarget()
    {
        EMonsterState prevState = MonsterState;

        if (AttackTarget != null)
        {
            Vector3 attackTargetDistance = this.transform.position - AttackTarget.transform.position;
            LookLeft = (attackTargetDistance.x > 0.0f);
            MonsterState = EMonsterState.Attack;
            return prevState != MonsterState;
        }

        ChaseDetectTarget();

        if (ChaseTarget != null)
        {
            Vector3 chaseTargetDistance = this.transform.position + Collider.center - ChaseTarget.transform.position;
            LookLeft = (chaseTargetDistance.x > 0.0f);

            if (Mathf.Abs(chaseTargetDistance.x) < 0.1f)
            {
                MonsterState = EMonsterState.Idle;
                return prevState != MonsterState;
            }

            float chaseDistanceSqr = MonsterData.ChaseDistance * MonsterData.ChaseDistance;
            if (chaseDistanceSqr >= chaseTargetDistance.sqrMagnitude)
            {
                MonsterState = EMonsterState.Move;
                return prevState != MonsterState;
            }
        }

        return false;
    }

    #region Idle Motion
    protected virtual bool IdleStateCondition()
    {
        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void IdleStateEnter()
    {
        InitRigidVelocityX();
    }

    protected virtual void UpdateIdleState()
    {
        IsChaseOrAttackTarget();

        MonsterState = EMonsterState.Move;
    }

    protected virtual void IdleStateExit()
    {

    }
    #endregion

    protected virtual bool WalkStateCondition()
    {


        return true;
    }

    protected virtual void WalkStateEnter()
    {

    }

    protected virtual void UpdateWalkState()
    {

    }

    protected virtual void WalkStateExit()
    {

    }

    #region Move Motion
    protected virtual bool MoveStateCondition()
    {
        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void MoveStateEnter()
    {

    }

    protected virtual void UpdateMoveState()
    {
        if (IsMovementCheck() == false)
        {
            InitRigidVelocityX();
            ChaseTarget = null;
            LookLeft = !LookLeft;
            return;
        }

        if (IsChaseOrAttackTarget())
            return;

        SetRigidVelocityX(lookDirX * MonsterData.MoveSpeed);
    }

    protected virtual void MoveStateExit()
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

    }

    protected virtual void UpdateAttackState()
    {
        if (IsMovementCheck())
            SetRigidVelocityX(lookDirX * MonsterData.MoveSpeed);
        else
            InitRigidVelocityX();

        if (IsEndCurrentState(EMonsterState.Attack))
        {
            MonsterState = EMonsterState.Idle;
            return;
        }
    }

    protected virtual void AttackStateExit()
    {

    }

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        attackTarget?.OnHit(new AttackParam(LookLeft));
    }

    public void OnAttackRangeInTarget(Player player)
    {
        AttackTarget = player;
    }
    #endregion

    #region Hit Motion
    Vector3 hitForceDir = Vector3.zero;
    protected virtual bool HitStateCondition()
    {


        return true;
    }

    protected virtual void HitStateEnter()
    {
        if (hitForceDir != Vector3.zero)
        {
            // 왜 공중으로 뜨지 않지..?
            SetRigidVelocity(hitForceDir);
        }
    }

    protected virtual void UpdateHitState()
    {
        if (IsEndCurrentState(EMonsterState.Hit))
        {
            // 히트로 인해 낙하상태로 들어갈 수 있음. (메모)
            MonsterState = EMonsterState.Idle;
        }
    }

    protected virtual void HitStateExit()
    {

    }

    public void OnHit(AttackParam param = null)
    {
        if (param == null)
            return;

        LookLeft = !param.isAttackerLeft;
        hitForceDir.x = (param.isAttackerLeft) ? -1 : 1;
        hitForceDir.y = 1;
        MonsterState = EMonsterState.Hit;
    }
    #endregion

    #region Dead Motion
    protected virtual bool DeadStateCondition()
    {
        return true;
    }

    protected virtual void DeadStateEnter()
    {

    }

    protected virtual void UpdateDeadState()
    {

    }

    protected virtual void DeadStateExit()
    {

    }
    #endregion

    #endregion
}