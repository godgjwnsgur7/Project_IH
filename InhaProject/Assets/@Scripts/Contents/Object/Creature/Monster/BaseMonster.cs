using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseMonster : Creature
{
    // 임시 데이터들
    protected float MoveSpeed = 1;
    [SerializeField] protected float AttackDistance;

    // 임시
    [SerializeField] BaseObject _target = null;

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

    private void RaycastTarget()
    {
        Vector3 subVec = new Vector3(0, Collider.center.y, 0);

        Debug.DrawRay(transform.position + subVec, Vector3.right * AttackDistance, Color.red, 0.1f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + subVec, Vector3.right, out hit, AttackDistance))
        {
            CreatureState = ECreatureState.Attack;
        }
    }

    protected virtual void UpdateIdle()
    {
        RaycastTarget();
    }

    protected virtual void UpdateMove()
    {
        RaycastTarget();

        // 1. 지금 앞으로 갈 수 있는지 확인


        Vector3 subVec = new Vector3(0, Collider.center.y, 0);
        subVec *= (LookLeft) ? 1 : -1;

        Debug.DrawRay(transform.position + subVec, Vector3.right, Color.red, 0.1f);
    }

    protected virtual void UpdateAttack()
    {
        if (IsEndCurrentState(ECreatureState.Attack))
        {
            CreatureState = ECreatureState.Idle;
        }
    }
    #endregion
}