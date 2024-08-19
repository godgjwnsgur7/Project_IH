using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using Data;

/// <summary>
/// 애니메이션 클립 이름과 같아야 함
/// </summary>
public enum ECreatureState
{
    None,
    Idle,
    Walk, // 일단 미사용
    Move, // Run으로 일단 사용
    Jump,
    JumpAir,
    Fall,
    Land,
    Attack,
    Hit,

    Dead
}

public enum ECreatureType
{
    Player,
    Monster,
}

public class Creature : BaseObject
{
    [SerializeField, ReadOnly] protected bool isCreatureStateLock = false;
    [SerializeField, ReadOnly] protected ECreatureState _creatureState = ECreatureState.None;
    public virtual ECreatureState CreatureState
    {
        get { return _creatureState; }
        protected set
        {
            if (value != ECreatureState.Idle && isCreatureStateLock)
                return;

            if (_creatureState == ECreatureState.Dead)
                return;

            if (_creatureState == value)
                return;

            bool isChangeState = true;
            switch(value)
            {
                case ECreatureState.Idle:
                    isChangeState = IdleStateCondition();
                    break;
                case ECreatureState.Move:
                    isChangeState = MoveStateCondition();
                    break;
                case ECreatureState.Jump:
                    isChangeState = JumpStateCondition();
                    break;
                case ECreatureState.JumpAir:
                    isChangeState = JumpAirStateCondition();
                    break;
                case ECreatureState.Fall:
                    isChangeState = FallStateCondition();
                    break;
                case ECreatureState.Land:
                    isChangeState = LandStateCondition();
                    break;
                case ECreatureState.Attack:
                    isChangeState = AttackStateCondition();
                    break;
                case ECreatureState.Dead:
                    isChangeState = DeadStateCondition();
                    break;
            }

            if (isChangeState == false)
                return;

            switch(_creatureState)
            {
                case ECreatureState.Idle:
                    IdleStateExit();
                    break;
                case ECreatureState.Move:
                    MoveStateExit();
                    break;
                case ECreatureState.Jump:
                    JumpStateExit();
                    break;
                case ECreatureState.JumpAir:
                    JumpAirStateExit();
                    break;
                case ECreatureState.Fall:
                    FallStateExit();
                    break;
                case ECreatureState.Land:
                    LandStateExit();
                    break;
                case ECreatureState.Attack:
                    AttackStateExit();
                    break;
                case ECreatureState.Hit:
                    HitStateExit();
                    break;
                case ECreatureState.Dead:
                    DeadStateExit();
                    break;
            }

            _creatureState = value;
            PlayAnimation(value);

            switch (value)
            {
                case ECreatureState.Idle:
                    IdleStateEnter();
                    break;
                case ECreatureState.Move:
                    MoveStateEnter();
                    break;
                case ECreatureState.Jump:
                    JumpStateEnter();
                    break;
                case ECreatureState.JumpAir:
                    JumpAirStateEnter();
                    break;
                case ECreatureState.Fall:
                    FallStateEnter();
                    break;
                case ECreatureState.Land:
                    LandStateEnter();
                    break;
                case ECreatureState.Attack:
                    AttackStateEnter();
                    break;
                case ECreatureState.Hit:
                    HitStateEnter();
                    break;
                case ECreatureState.Dead:
                    DeadStateEnter();
                    break;
            }
        }
    }

    public ECreatureType CreatureType { get; protected set; }

    public CreatureFoot creatureFoot { get; protected set; }

    protected Rigidbody Rigid { get; private set; }
    [SerializeField] public BoxCollider Collider { get; private set; }
    protected Animator animator;

    private bool _lookLeft = false;
    public bool LookLeft
    {
        get { return _lookLeft; }
        set
        {
            if (_lookLeft == value)
                return;

            _lookLeft = value;
            FlipX(value);
        }
    }  

    protected float LookDirX;

    protected virtual void Start()
    {
        SetInfo(); // 임시?
    }

    protected virtual void Reset()
    {
        creatureFoot ??= Util.FindChild<CreatureFoot>(this.gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Reset();
        Collider = GetComponent<BoxCollider>();
        Rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        CreatureState = ECreatureState.Idle;
    }

    protected override void FlipX(bool isLeft)
    {
        base.FlipX(isLeft);

        LookDirX = (isLeft) ? -1 : 1;
    }

    #region Rigid

    protected void SetRigidVelocity(Vector3 velocity)
    {
        Rigid.velocity = velocity;
    }

    protected void SetRigidVelocityX(float x)
    {
        Vector3 veloctiy = new Vector3(x, Rigid.velocity.y, 0);
        Rigid.velocity = veloctiy; 
    }

    protected void SetRigidVelocityY(float y)
    {
        Vector3 veloctiy = new Vector3(Rigid.velocity.x, y, 0);
        Rigid.velocity = veloctiy;
    }

    protected void InitRigidVelocityY()
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, 0, 0);
    }

    protected void InitRigidVelocityX()
    {
        Rigid.velocity = new Vector3(0, Rigid.velocity.y, 0);
    }

    protected void InitRigidVelocity()
    {
        Rigid.velocity = Vector3.zero;
    }
    #endregion

    #region State Condition
    protected virtual bool IdleStateCondition() { return true; }
    protected virtual bool MoveStateCondition() { return true; }
    protected virtual bool JumpStateCondition() { return true; }
    protected virtual bool JumpAirStateCondition() { return true; }
    protected virtual bool FallStateCondition() { return true; }
    protected virtual bool LandStateCondition() { return true; }
    protected virtual bool AttackStateCondition() { return true; }
    protected virtual bool HitStateCondition() { return true; }
    protected virtual bool DeadStateCondition() { return true; }
    #endregion

    #region State Enter
    protected virtual void IdleStateEnter() { }
    protected virtual void MoveStateEnter() { }
    protected virtual void JumpStateEnter() { }
    protected virtual void JumpAirStateEnter() { }
    protected virtual void FallStateEnter() { }
    protected virtual void LandStateEnter() { }
    protected virtual void AttackStateEnter() { }
    protected virtual void HitStateEnter() { }
    protected virtual void DeadStateEnter() { }
    #endregion

    #region State Exit
    protected virtual void IdleStateExit() { }
    protected virtual void MoveStateExit() { }
    protected virtual void JumpStateExit() { }
    protected virtual void JumpAirStateExit() { }
    protected virtual void FallStateExit() { }
    protected virtual void LandStateExit() { }
    protected virtual void AttackStateExit() { }
    protected virtual void HitStateExit() { }
    protected virtual void DeadStateExit() { }
    #endregion

    #region Animation
    protected void PlayAnimation(ECreatureState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString());
    }

    public bool IsState(ECreatureState state)
    {
        if (animator == null)
            return false;

        return IsState(animator.GetCurrentAnimatorStateInfo(0), state);
    }

    private bool IsState(AnimatorStateInfo stateInfo, ECreatureState state)
    {
        return stateInfo.IsName(state.ToString());
    }

    public bool IsEndCurrentState(ECreatureState state)
    {
        if (animator == null)
        {
            Debug.LogWarning("animator is Null");
            return false;
        }

        // 다른 애니메이션이 재생 중
        if(!IsState(state))
            return false;

        return IsEndState(animator.GetCurrentAnimatorStateInfo(0));
    }

    private bool IsEndState(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime >= 1.0f;
    }
    #endregion
}
