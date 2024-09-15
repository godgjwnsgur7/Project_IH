using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFixedBossMonsterState
{
    None,
    Idle,
    Pattern1,
    Pattern2,
    Pattern3,
    Pattern4,
}

public enum EFixedBossMonsterType
{
    SkeletonWizardBoss = 0,
    Max,
}

public class FixedBossMonster : Creature
{
    [field: SerializeField, ReadOnly] private List<BossGimmickPoint> gimmickPointList;
    [field: SerializeField, ReadOnly] private List<MonsterSpawnPoint> monsterSpawnPointList;

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
                    UpdateAITick = 5.0f;
                    break;
                default:
                    UpdateAITick = 0.0f;
                    break;
            }

            _monsterState = value;
            PlayAnimation(value);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(List<BossGimmickPoint> gimmickPointList, List<MonsterSpawnPoint> monsterSpawnPointList)
    {
        this.gimmickPointList = gimmickPointList;
        this.monsterSpawnPointList = monsterSpawnPointList;

        StartCoroutine(CoUpdateAI());
    }

    protected override void FlipX(bool isLeft)
    {
        base.FlipX(false);
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
                case EFixedBossMonsterState.Idle: UpdateIdleState(); break;
                case EFixedBossMonsterState.Pattern1:
                case EFixedBossMonsterState.Pattern2:
                case EFixedBossMonsterState.Pattern3:
                case EFixedBossMonsterState.Pattern4:
                    UpdatePattern(MonsterState);
                    break;
            }

            if (UpdateAITick > 0)
                yield return new WaitForSeconds(UpdateAITick);
            else
                yield return null;
        }
    }

    protected void UpdateIdleState()
    {

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
            int randomNum = (Random.Range(0, (int)ENormalMonsterType.Max));
            ENormalMonsterType spawnMonsterType = (ENormalMonsterType)randomNum;
            Managers.Object.SpawnNormalMonster(spawnMonsterType, point.transform.position);
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
    #endregion
}
