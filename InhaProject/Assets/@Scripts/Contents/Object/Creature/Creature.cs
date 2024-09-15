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
    public CreatureFoot CreatureFoot { get; protected set; }

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

    protected virtual void Reset()
    {
        CreatureFoot ??= Util.FindChild<CreatureFoot>(this.gameObject);
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

    public virtual Vector3 GetTopPosition()
    {
        return this.transform.position + (Vector3.up * Collider.size.y * transform.localScale.y);
    }
    
    public virtual float GetSizeX()
    {
        return Collider.size.x * transform.localScale.x;
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
    protected bool IsEndState(AnimatorStateInfo stateInfo)
    {
        return stateInfo.normalizedTime >= 1.0f;
    }
    #endregion
}
