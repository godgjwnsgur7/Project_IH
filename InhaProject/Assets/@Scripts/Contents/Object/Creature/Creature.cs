using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    [SerializeField] // 확인용
    protected ECreatureState _creatureState = ECreatureState.None;
    public virtual ECreatureState CreatureState
    {
        get { return _creatureState; }
        protected set
        {
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

            _creatureState = value;
            PlayAnimation(value);

            switch (value)
            {
                case ECreatureState.Idle:
                    IdleStateOperate();
                    break;
                case ECreatureState.Move:
                    MoveStateOperate();
                    break;
                case ECreatureState.Jump:
                    JumpStateOperate();
                    break;
                case ECreatureState.Fall:
                    FallStateOperate();
                    break;
                case ECreatureState.Land:
                    LandStateOperate();
                    break;
                case ECreatureState.Attack:
                    AttackStateOperate();
                    break;
                case ECreatureState.Dead:
                    DeadStateOperate();
                    break;
            }
        }
    }

    public ECreatureType CreatureType { get; protected set; }

    public CreatureFoot creatureFoot { get; protected set; }

    protected Rigidbody Rigid { get; private set; }
    [SerializeField] public CapsuleCollider Collider { get; private set; }
    protected Animator animator;

    protected virtual void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<CapsuleCollider>();
        Rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        creatureFoot ??= Util.FindChild<CreatureFoot>(gameObject);

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        CreatureState = ECreatureState.Idle;
    }

    #region Rigid
    protected void SetRigidVelocity(Vector2 vec)
    {
        Vector3 dir = Quaternion.AngleAxis(-transform.rotation.eulerAngles.y, Vector3.forward) * vec;
        
        Rigid.AddForce(new Vector3(dir.x, 0, dir.y), ForceMode.Impulse);
    }

    protected void PushRigidVelocity(float y)
    {
        Rigid.AddForce(new Vector3(0, y, 0), ForceMode.Impulse);
    }

    protected void SetRigidVelocityZero()
    {
        Rigid.velocity = new Vector3(0, Rigid.velocity.y, 0);
    }
    #endregion

    #region State Condition
    protected virtual bool IdleStateCondition() { return true; }
    protected virtual bool MoveStateCondition() { return true; }
    protected virtual bool JumpStateCondition() { return true; }
    protected virtual bool FallStateCondition() { return true; }
    protected virtual bool LandStateCondition() { return true; }
    protected virtual bool AttackStateCondition() { return true; }
    protected virtual bool DeadStateCondition() { return true; }
    #endregion

    #region State Operate
    protected virtual void IdleStateOperate() { }
    protected virtual void MoveStateOperate() { }
    protected virtual void JumpStateOperate() { }
    protected virtual void FallStateOperate() { }
    protected virtual void LandStateOperate() { }
    protected virtual void AttackStateOperate() { }
    protected virtual void DeadStateOperate() { }
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
