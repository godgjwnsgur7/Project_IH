using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;

public enum EMonsterType
{
    Skeleton1,
    Skeleton2, // 아직 사용 불가
    Skeleton3, // 아직 사용 불가

}
public class BaseMonster : Creature, IHitEvent
{
    [SerializeField] protected BaseAttackObject attackObject;

    // 임시 데이터들
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float ChaseDistance;
    [SerializeField] protected float AttackDistance;

    public override ECreatureState CreatureState
    {
        get { return base.CreatureState; }
        protected set
        {
            if (_creatureState != value)
            {
                base.CreatureState = value;
                switch (value)
                {
                    case ECreatureState.None:
                        UpdateAITick = 0.0f;
                        break;
                    case ECreatureState.Idle:
                        UpdateAITick = 0.5f;
                        break;
                    case ECreatureState.Move:
                        UpdateAITick = 0.0f;
                        break;
                    case ECreatureState.Fall:
                        UpdateAITick = 0.0f;
                        break;
                    case ECreatureState.Land:
                        UpdateAITick = 0.0f;
                        break;
                    case ECreatureState.Attack:
                        UpdateAITick = 0.0f;
                        break;
                    case ECreatureState.Dead:
                        UpdateAITick = 1.0f;
                        break;
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, AttackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, ChaseDistance);
    }
#endif

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SetInfo();
        StartCoroutine(CoUpdateAI());

        // 임시
        MoveSpeed = 3.0f;
        ChaseDistance = 8.0f;
        AttackDistance = 1.5f;

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        CreatureType = ECreatureType.Monster;
        attackObject.SetInfo(ETag.Player, OnAttackTarget);
    }

    #region AI
    public float UpdateAITick { get; protected set; } = 0.0f;
    [SerializeField, ReadOnly] Player Target;

    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            switch (CreatureState)
            {
                case ECreatureState.None:
                    CreatureState = ECreatureState.Idle;
                    break;
                case ECreatureState.Idle:
                    UpdateIdleState();
                    break;
                case ECreatureState.Move:
                    UpdateMoveState();
                    break;
                case ECreatureState.Fall:
                    UpdateFallState();
                    break;
                case ECreatureState.Land:
                    UpdateLandState();
                    break;
                case ECreatureState.Attack:
                    UpdateAttackState();
                    break;
                case ECreatureState.Hit:
                    UpdateHitState();
                    break;
                case ECreatureState.Dead:
                    UpdateDeadState();
                    break;
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    private bool DetectTarget(float detectDistance)
    {
        Vector3 subVec = new Vector3(0, Collider.center.y, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.right * detectDistance * LookDirX, Color.red, 0.1f);
        if (!Physics.Raycast(transform.position + subVec, Vector3.right * LookDirX, out RaycastHit hit, detectDistance, 1 << (int)ELayer.Player))
            return false;

        if (hit.transform.GetComponent<Player>() != null)
            Target = hit.transform.GetComponent<Player>();
        
        return true;
    }

    private bool IsMovementCheck()
    {
        Vector3 subVec = new Vector3((0.1f + Collider.center.x + (Collider.size.x / 2)) * LookDirX, 0, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.down* 2, Color.blue);
        if (!Physics.Raycast(transform.position + subVec, Vector3.down, out RaycastHit hit, 2, 1 << (int)ELayer.Platform))
            return false;

        return true;
    }
     
    private bool IsChaseOrAttackTarget()
    {
        ECreatureState prevState = CreatureState;

        if (Target != null && DetectTarget(AttackDistance))
        {
            CreatureState = ECreatureState.Attack;
            return prevState != CreatureState;
        }

        DetectTarget(ChaseDistance);

        if (Target == null)
            return false;

        Vector3 targetDistance = this.transform.position - Target.transform.position;
        LookLeft = (targetDistance.x > 0.0f);

        if (Mathf.Abs(targetDistance.x) < 0.1f)
        {
            if (DetectTarget(AttackDistance))
                CreatureState = ECreatureState.Attack;
            else
                CreatureState = ECreatureState.Idle;
            
            return prevState != CreatureState;
        }

        float chaseDistanceSqr = ChaseDistance * ChaseDistance;
        if (chaseDistanceSqr >= targetDistance.sqrMagnitude)
        {
            CreatureState = ECreatureState.Move;
            return prevState != CreatureState;
        }

        Target = null;
        return false;
    }

    #region Idle Motion
    protected override bool IdleStateCondition()
    {
        if (base.IdleStateCondition() == false)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected override void IdleStateEnter()
    {
        base.IdleStateEnter();

        InitRigidVelocityX();
    }

    protected virtual void UpdateIdleState()
    {
        IsChaseOrAttackTarget();

        CreatureState = ECreatureState.Move;
    }
    #endregion

    #region Move Motion
    protected override bool MoveStateCondition()
    {
        if (base.MoveStateCondition() == false)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void UpdateMoveState()
    {
        if (creatureFoot.IsLandingGround == false)
        {
            CreatureState = ECreatureState.Land;
            return;
        }

        if (IsMovementCheck() == false)
        {
            InitRigidVelocityX();
            Target = null;
            LookLeft = !LookLeft;
            return;
        }

        if (IsChaseOrAttackTarget())
            return;

        SetRigidVelocityX(LookDirX * MoveSpeed);
    }

    #endregion

    #region Attack Motion

    protected override void AttackStateEnter()
    {
        base.AttackStateEnter();

        attackObject.SetActiveAttackObject(true);
    }

    protected virtual void UpdateAttackState()
    {
        if (IsMovementCheck())
            SetRigidVelocityX(LookDirX * MoveSpeed);
        else
            InitRigidVelocityX();

        if (IsEndCurrentState(ECreatureState.Attack))
        {
            CreatureState = ECreatureState.Idle;
            return;
        }
    }

    protected override void AttackStateExit()
    {
        base.AttackStateExit();

        attackObject.SetActiveAttackObject(false);
    }

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        attackTarget?.OnHit(new AttackParam(LookLeft));
    }
    #endregion

    #region Fall Motion
    protected override bool FallStateCondition()
    {
        if (base.FallStateCondition() == false)
            return false;

        if (Rigid.velocity.y >= 0)
            return false;

        return true;
    }

    protected override void FallStateEnter()
    {
        base.FallStateEnter();
    }

    private void UpdateFallState()
    {
        if (creatureFoot.IsLandingGround)
        {
            CreatureState = ECreatureState.Land;
            return;
        }
    }

    protected virtual void FallDownCheck()
    {
        if (creatureFoot.IsLandingGround == false && Rigid.velocity.y < 0)
            CreatureState = ECreatureState.Fall;
    }
    #endregion

    #region Land Motion
    protected override bool LandStateCondition()
    {
        if (base.LandStateCondition() == false)
            return false;

        if (Rigid.velocity.y >= 0.01f)
            return false;

        return true;
    }

    protected override void LandStateEnter()
    {
        base.LandStateEnter();
    }

    protected virtual void UpdateLandState()
    {
        if (IsEndCurrentState(ECreatureState.Land))
        {
            CreatureState = ECreatureState.Move;
            CreatureState = ECreatureState.Idle;
        }
    }

    public void OnLand()
    {

    }
    #endregion

    #region Hit Motion
    Vector3 hitForceDir = Vector3.zero;

    protected override void HitStateEnter()
    {
        base.HitStateEnter();
    }

    protected virtual void UpdateHitState()
    {
        if (IsEndCurrentState(ECreatureState.Hit))
        {
            FallDownCheck();
            CreatureState = ECreatureState.Idle;
        }
    }

    protected override void HitStateExit()
    {
        base.HitStateExit();
    }

    public void OnHit(AttackParam param = null)
    {
        if (param == null)
            return;

        LookLeft = param.isAttackerLeft;
        hitForceDir.x = (param.isAttackerLeft) ? -1 : 1;
        hitForceDir.y = 1;
        isCreatureStateLock = false;
        CreatureState = ECreatureState.Hit;
    }
    #endregion

    #region Dead Motion
    protected override bool DeadStateCondition()
    {
        return true;
    }

    protected virtual void UpdateDeadState()
    {

    }

    protected override void DeadStateEnter()
    {
        base.DeadStateEnter();
    }

    #endregion

    #endregion
}