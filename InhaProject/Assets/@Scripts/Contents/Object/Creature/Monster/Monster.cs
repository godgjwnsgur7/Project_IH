using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    // 임시 데이터들
    protected float MoveSpeed = 1;
    [SerializeField] protected float SearchDistance;
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
        _initPos = this.transform.position;

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
    [SerializeField] Vector3 _destPos, _initPos;
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

    protected virtual void UpdateIdle()
    {
        // Patrol
        {
            int patrolPercent = 10;
            int rand = Random.Range(0, 100);
            if (rand <= patrolPercent)
            {
                _destPos = _initPos + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                CreatureState = ECreatureState.Move;
                return;
            }
        }
    }

    protected virtual void UpdateMove()
    {
        if (_target == null)
        {
            Vector3 dir = (_destPos - transform.position);
            this.transform.rotation = Quaternion.LookRotation(dir.normalized);
            // SetRigidVelocity(Vector2.up * MoveSpeed);

            if (dir.sqrMagnitude <= 0.1f)
            {
                CreatureState = ECreatureState.Idle;
            }
        }
        else
        {
            // Chase
            Vector3 dir = (_target.transform.position - transform.position);
            this.transform.rotation = Quaternion.LookRotation(dir.normalized);
            float distToTargetSqr = dir.sqrMagnitude;
            float attackDistanceSqr = AttackDistance * AttackDistance;

            if (distToTargetSqr < attackDistanceSqr)
            {
                // 공격 범위 이내로 들어왔으면 공격.
                CreatureState = ECreatureState.Attack;
            }
            else
            {
                // 공격 범위 밖이라면 추적.
                float moveDist = Mathf.Min(dir.magnitude, Time.deltaTime * MoveSpeed);
                // SetRigidVelocity(Vector2.up * MoveSpeed);
            }
        }
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