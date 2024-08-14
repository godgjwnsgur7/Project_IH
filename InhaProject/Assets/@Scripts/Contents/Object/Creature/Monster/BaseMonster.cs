using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseMonster : Creature, IHitEvent
{
    [SerializeField] protected BaseAttackObject attackObject;

    // 임시 데이터들
    protected float MoveSpeed = 1;
    [SerializeField] protected float AttackDistance = 2;

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
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    private bool DetectTargetToAttack()
    {
        Vector3 subVec = new Vector3(0, Collider.center.y, 0);

        Debug.DrawRay(transform.position + subVec, Vector3.right * AttackDistance * moveDirX, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.right * moveDirX, out RaycastHit hit, AttackDistance, 1 << (int)ELayer.Player))
        {
            return true;
        }

        return false;
    }

    private bool MovementCheckToRay()
    {
        Vector3 subVec = new Vector3((Collider.center.x + (Collider.size.x / 2)) * moveDirX, 0, 0);

        Debug.DrawRay(transform.position + subVec, Vector3.down, Color.red, 0.1f);
        if (Physics.Raycast(transform.position + subVec, Vector3.down, out RaycastHit hit, 1, 1 << (int)ELayer.Platform))
            return true;

        return false;
    }

    protected virtual void UpdateIdle()
    {
        if (DetectTargetToAttack())
        {
            CreatureState = ECreatureState.Attack;
            return;
        }
        
        DetectTargetToAttack();
        SetRigidVelocityZeroToX();

        LookLeft = !LookLeft;
        CreatureState = ECreatureState.Move;
    }

    protected virtual void UpdateMove()
    {
        if (DetectTargetToAttack())
        {
            CreatureState = ECreatureState.Attack;
            return;
        }

        if (MovementCheckToRay() == false)
        {
            CreatureState = ECreatureState.Idle;
            return;
        }

        PushRigidVelocityX(moveDirX * MoveSpeed * 0.1f);
    }

    protected virtual void UpdateAttack()
    {
        if (MovementCheckToRay())
            PushRigidVelocityX(moveDirX * MoveSpeed * 0.5f);
        else
            SetRigidVelocityZeroToX();

        if (IsEndCurrentState(ECreatureState.Attack))
        {
            CreatureState = ECreatureState.Idle;
        }
    }
    #endregion
}