using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using Data;

public enum ECreatureType
{
    Player,
    Monster,
}

public class Creature : BaseObject
{
    public ECreatureType CreatureType { get; protected set; }
    public CreatureFoot creatureFoot { get; protected set; }

    protected Rigidbody Rigid { get; private set; }
    [SerializeField] public BoxCollider Collider { get; private set; }

    protected Animator animator;

    protected float lookDirX;
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
    }

    protected override void FlipX(bool isLeft)
    {
        base.FlipX(isLeft);

        lookDirX = (isLeft) ? -1 : 1;
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

    #region Animation
    protected void PlayAnimation(EPlayerState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString());
    }

    public bool IsState(EPlayerState state)
    {
        if (animator == null)
            return false;

        return IsState(animator.GetCurrentAnimatorStateInfo(0), state);
    }

    protected bool IsState(AnimatorStateInfo stateInfo, EPlayerState state)
    {
        return stateInfo.IsName(state.ToString());
    }

    public bool IsEndCurrentState(EPlayerState state)
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

    protected void PlayAnimation(EMonsterState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString());
    }

    protected void ReplayAnimation(EMonsterState state)
    {
        if (IsState(state))
            return;

        animator.Play(state.ToString(), -1, 0f);
    }

    public bool IsState(EMonsterState state)
    {
        if (animator == null)
            return false;

        return IsState(animator.GetCurrentAnimatorStateInfo(0), state);
    }

    protected bool IsState(AnimatorStateInfo stateInfo, EMonsterState state)
    {
        return stateInfo.IsName(state.ToString());
    }

    public bool IsEndCurrentState(EMonsterState state)
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

    protected bool IsEndState(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime >= 1.0f;
    }
    #endregion
}
