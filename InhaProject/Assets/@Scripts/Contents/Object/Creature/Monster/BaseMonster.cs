using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        attackTarget?.OnHit();
    }

    public void OnHit(AttackParam param = null)
    {
        Debug.Log("몬스터 히트당함");
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

    private void DetectTargetToAttack()
    {
        if (Target != null)
            return;

        Vector3 subVec = new Vector3(0, Collider.center.y, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.right * AttackDistance * moveDirX, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.right * moveDirX, out RaycastHit hit, AttackDistance, 1 << (int)ELayer.Player))
        {
            CreatureState = ECreatureState.Attack;
        }
    }

    private bool IsMovementCheck()
    {
        Vector3 subVec = new Vector3((Collider.center.x + (Collider.size.x / 2)) * moveDirX, 0, 0);
        Debug.DrawRay(transform.position + subVec, Vector3.down, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.down, out RaycastHit hit, 1, 1 << (int)ELayer.Platform))
            return true;

        return false;
    }
     
    private void ChaseOrAttackTarget()
    {
        // DetectTarget();

        if (Target == null)
            return;

        Vector3 vec = this.transform.position - Target.transform.position;

        if (AttackDistance >= vec.x)
        {
            LookLeft = (vec.x > 0);
            CreatureState = ECreatureState.Attack;
            return;
        }

        float ChaseDistanceSqr = ChaseDistance * ChaseDistance;
        if (ChaseDistanceSqr >= vec.sqrMagnitude)
        {
            LookLeft = (vec.x > 0);
            CreatureState = ECreatureState.Move;
            return;
        }

        Target = null;
    }

    #region Idle Motion
    
    protected virtual void UpdateIdle()
    {
        if (IsMovementCheck() == false)
        {
            LookLeft = !LookLeft;
            CreatureState = ECreatureState.Move;
        }
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
        if(IsMovementCheck() == false)
        {
            CreatureState = ECreatureState.Idle;
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
            CreatureState = ECreatureState.Idle;
        }
    }

    protected override void AttackStateExit()
    {
        base.AttackStateExit();

        attackObject.SetActiveAttackObject(false);
    }
    #endregion

    #region Attack Motion
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
    #endregion

    #endregion
}