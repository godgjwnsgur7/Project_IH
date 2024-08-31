using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

public enum EMonsterType
{
    SkeletonWarrior = 0,
    SkeletonWizard = 1,
}

public enum  EMonsterState
{
    None,
    Idle,
    Patrol,
    Chase,
    Attack,
    Hit,
    Dead
}

[System.Serializable]
public class MonsterData
{
    public float CurrHp;
    public float StrikingPower;     // °ø°Ý·Â
    public float MoveSpeed;
    public float ChaseSpeed;
    public float DetectDistance;
    public float ChaseDistance;
    public float AttackDistance;

    public MonsterData(JMonsterData jMonsterData)
    {
        CurrHp = jMonsterData.MaxHp;
        StrikingPower = jMonsterData.StrikingPower;
        MoveSpeed = jMonsterData.MoveSpeed;
        ChaseSpeed = jMonsterData.ChaseSpeed;
        DetectDistance = jMonsterData.DetectDistance;
        ChaseDistance = jMonsterData.ChaseDistance;
        AttackDistance = jMonsterData.AttackDistance;
    }
}

public class Monster : Creature, IHitEvent
{
    [field: SerializeField, ReadOnly] public EMonsterType MonsterType { get; protected set; }
    [field: SerializeField, ReadOnly] public MonsterData MonsterInfo { get; protected set; }

    [SerializeField, ReadOnly] protected AttackObject attackObject;
    [SerializeField, ReadOnly] protected MonsterAttackRange attackRange;
    [SerializeField, ReadOnly] List<Renderer> rendererList;

    [SerializeField, ReadOnly]
    private EMonsterState _monsterState;
    public virtual EMonsterState MonsterState
    {
        get { return _monsterState; }
        protected set
        {
            if (_monsterState == EMonsterState.Dead)
                return;

            if (_monsterState == value)
                return;

            bool isChangeState = true;
            switch (value)
            {
                case EMonsterState.Idle:
                    isChangeState = IdleStateCondition();
                    break;
                case EMonsterState.Patrol:
                    isChangeState = PatrolStateCondition();
                    break;
                case EMonsterState.Chase:
                    isChangeState = ChaseStateCondition();
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
                case EMonsterState.Patrol:
                    PatrolStateExit();
                    break;
                case EMonsterState.Chase:
                    ChaseStateExit();
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

            switch (value)
            {
                case EMonsterState.Idle:
                case EMonsterState.Hit:
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
                case EMonsterState.Idle:
                    IdleStateEnter();
                    break;
                case EMonsterState.Patrol:
                    PatrolStateEnter();
                    break;
                case EMonsterState.Chase:
                    ChaseStateEnter();
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

        rendererList = new List<Renderer>();
        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren)
            if(child.GetComponent<ParticleSystem>() == null && 
                child.TryGetComponent<Renderer>(out Renderer renderer))
                rendererList.Add(renderer);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Monster;

        this.gameObject.layer = (int)ELayer.Monster;
        this.tag = ETag.Monster.ToString();

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        MonsterType = Util.ParseEnum<EMonsterType>(gameObject.name);
        MonsterInfo = new MonsterData(Managers.Data.MonsterDict[(int)MonsterType]);
        MonsterInfo.AttackDistance *= 2;

        attackObject.SetInfo(ETag.Monster, OnAttackTarget);
        attackRange.SetInfo(OnAttackRangeInTarget);

        StartCoroutine(CoUpdateAI());
    }

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
                case EMonsterState.None:
                    MonsterState = EMonsterState.Idle;
                    break;
                case EMonsterState.Idle:
                    UpdateIdleState();
                    break;
                case EMonsterState.Patrol:
                    UpdatePatrolState();
                    break;
                case EMonsterState.Chase:
                    UpdateChaseState();
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
        if (ChaseTarget != null)
            return;

        Vector3 subVec = new Vector3(0, this.transform.localScale.y * Collider.center.y, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.right * MonsterInfo.DetectDistance * lookDirX, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.right * lookDirX, out RaycastHit hit, MonsterInfo.DetectDistance, 1 << (int)ELayer.Player)
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

            float chaseDistanceSqr = MonsterInfo.ChaseDistance * MonsterInfo.ChaseDistance;
            if (chaseDistanceSqr < chaseTargetDistance.sqrMagnitude)
            {
                ChaseTarget = null;
                MonsterState = EMonsterState.Idle;
                return prevState != MonsterState;
            }
            else
            {
                MonsterState = EMonsterState.Chase;
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
        if(IsChaseOrAttackTarget())
            return;

        // Patrol
        {
            int patrolPercent = 10;
            int rand = Random.Range(0, 100);
            if (rand <= patrolPercent)
            {
                LookLeft = (rand % 2 == 0);
                MonsterState = EMonsterState.Patrol;
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

        SetRigidVelocityX(lookDirX * MonsterInfo.MoveSpeed);
        patrolTime -= Time.deltaTime;

        if(patrolTime < 0.0f)
        {
            MonsterState = EMonsterState.Idle;
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
        if (creatureFoot.IsLandingGround == false)
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
            MonsterState = EMonsterState.Idle;
            return;
        }

        if (IsChaseOrAttackTarget())
            return;

        if (ChaseTarget == null)
        {
            MonsterState = EMonsterState.Idle;
            return;
        }

        SetRigidVelocityX(lookDirX * MonsterInfo.ChaseSpeed);
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

    }

    protected virtual void UpdateAttackState()
    {
        InitRigidVelocityX();

        if (IsEndCurrentState(EMonsterState.Attack))
        {
            MonsterState = EMonsterState.Idle;
            return;
        }
    }

    protected virtual void AttackStateExit()
    {
        InitRigidVelocityX();
    }

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        attackTarget?.OnHit(new AttackParam(this, LookLeft));
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
    public void OnHit(AttackParam param = null)
    {
        if (param == null)
            return;

        UIDamageParam damageParam = new((int)param.damage
            , transform.position + (Collider.size.y * Vector3.up * this.transform.localScale.y * 1.2f)
            , true);
        Managers.UI.SpawnObjectUI<UI_Damage>(EUIObjectType.UI_Damage, damageParam);

        MonsterInfo.CurrHp -= param.damage;
        LookLeft = !param.isAttackerLeft;
        hitForceDir.x = param.pushPower * ((param.isAttackerLeft) ? -1 : 1);
        MonsterState = EMonsterState.Dead;

        if (MonsterInfo.CurrHp > 0)
        {
            if (ChaseTarget == null && param.attacker is BaseObject target)
                ChaseTarget = target;

            if (MonsterState == EMonsterState.Hit)
            {
                ReplayAnimation(EMonsterState.Hit);
                SetRigidVelocity(hitForceDir);
            }
            MonsterState = EMonsterState.Hit;
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
        if (IsEndCurrentState(EMonsterState.Hit) && creatureFoot.IsLandingGround)
        {
            MonsterState = EMonsterState.Idle;
        }
    }

    protected virtual void HitStateExit()
    {
        
    }
    #endregion

    #region Dead Motion
    protected virtual bool DeadStateCondition()
    {
        if (MonsterInfo.CurrHp > 0f)
            return false;

        return true;
    }
    protected virtual void DeadStateEnter() { }
    protected virtual void UpdateDeadState()
    { 
        if (IsEndCurrentState(EMonsterState.Dead))
        {
            StartCoroutine(CoDestroyEffect(1.5f));
            return;
        }
    }
    protected virtual void DeadStateExit() { }

    private IEnumerator CoDestroyEffect(float fadeTime)
    {
        while(true)
        {
            int count = 0;
            float time = fadeTime * 0.01f * Time.deltaTime;
            foreach (Renderer randerer in rendererList)
            {
                Color tempColor = randerer.material.color;
                if(tempColor.a > 0.01f)
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

        Managers.Resource.Destroy(gameObject);
    }
    #endregion

    #endregion
}