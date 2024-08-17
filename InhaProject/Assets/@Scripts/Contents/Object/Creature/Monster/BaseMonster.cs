using System.Collections;
using System.Collections.Generic;
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
    protected float MoveSpeed = 1;
    [SerializeField] protected float ChaseDistance = 8f;
    [SerializeField] protected float AttackDistance = 1.5f;

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
                    case ECreatureState.Idle:
                        UpdateAITick = 0.5f;
                        break;
                    case ECreatureState.Move:
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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SetInfo();
        coUpdateAI = StartCoroutine(CoUpdateAI());

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
    Coroutine coUpdateAI = null;

    [SerializeField, ReadOnly] Player Target;

    protected IEnumerator CoUpdateAI()
    {
        while (true)
        {
            switch (CreatureState)
            {
                case ECreatureState.Idle:
                    UpdateIdle();
                    break;
                case ECreatureState.Move:
                    UpdateMove();
                    break;
                case ECreatureState.Attack:
                    UpdateAttack();
                    break;
                case ECreatureState.Hit:
                    UpdateHit();
                    break;
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    private bool IsDetectTarget(float detectDistance)
    {
        Vector3 subVec = new Vector3(0, Collider.center.y, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.right * detectDistance * moveDirX, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.right * moveDirX, out RaycastHit hit, detectDistance, 1 << (int)ELayer.Player))
        {
            Target = hit.transform.GetComponent<Player>();
            return true;
        }
        return false;
    }

    private bool IsMovementCheck()
    {
        Vector3 subVec = new Vector3((Collider.center.x + (Collider.size.x / 2)) * moveDirX, 0, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.down, Color.red);
        if (Physics.Raycast(transform.position + subVec, Vector3.down, out RaycastHit hit, 1, 1 << (int)ELayer.Platform))
            return true;

        return false;
    }
     
    private bool IsChaseOrAttackTarget()
    {
        if(Target != null && IsDetectTarget(AttackDistance))
        {
            CreatureState = ECreatureState.Attack;
            return true;
        }

        IsDetectTarget(ChaseDistance);
        
        if (Target == null)
            return false;

        Vector3 targetDistance = this.transform.position - Target.transform.position;
        LookLeft = (targetDistance.x > 0);

        if (Mathf.Abs(targetDistance.x) < 0.1f)
        {
            if (IsDetectTarget(AttackDistance))
                CreatureState = ECreatureState.Attack;
            else
                CreatureState = ECreatureState.Idle;

            return true;
        }

        float chaseDistanceSqr = ChaseDistance * ChaseDistance;
        if (chaseDistanceSqr >= targetDistance.sqrMagnitude)
        {
            CreatureState = ECreatureState.Move;
            return true;
        }

        Target = null;
        return false;
    }

    #region Idle Motion
    
    protected virtual void UpdateIdle()
    {
        if (IsChaseOrAttackTarget())
            return;

        CreatureState = ECreatureState.Move;
    }
    #endregion

    #region Move Motion
    protected override bool MoveStateCondition()
    {
        if (base.MoveStateCondition() == false)
            return false;

        if (IsMovementCheck() == false)
            return false;

        return true;
    }

    protected virtual void UpdateMove()
    {
        if (IsChaseOrAttackTarget())
            return;

        if (IsMovementCheck() == false)
        {
            Target = null;
            LookLeft = !LookLeft;
        }
    }
    #endregion

    #region Attack Motion

    protected override void AttackStateEnter()
    {
        base.AttackStateEnter();

        attackObject.SetActiveAttackObject(true);
    }

    protected virtual void UpdateAttack()
    {
        if (IsMovementCheck())
            PushRigidVelocityX(moveDirX * MoveSpeed * 0.5f);
        else
            SetRigidVelocityZeroToX();

        if (IsEndCurrentState(ECreatureState.Attack))
        {
            CreatureState = ECreatureState.Move;
            CreatureState = ECreatureState.Idle;
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

    #region Hit Motion
    [SerializeField, ReadOnly] Vector3 hitForceDir = Vector3.zero;

    protected override void HitStateEnter()
    {
        base.HitStateEnter();
    }

    protected virtual void UpdateHit()
    {
        if (IsEndCurrentState(ECreatureState.Hit))
        {
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

        LookLeft = !param.isLeftAttackDir;
        hitForceDir.x = (param.isLeftAttackDir) ? 1 : -1;
        hitForceDir.y = 1;
        isCreatureStateLock = false;
        CreatureState = ECreatureState.Hit;
    }
    #endregion

    #endregion
}