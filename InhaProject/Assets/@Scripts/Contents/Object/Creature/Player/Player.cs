using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static Define;

public enum EPlayerType
{
    FemaleCharacter,
    MaleCharacter, // 아직 사용 불가능
}

public class Player : Creature, IHitEvent
{
    // 임시 데이터들
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float JumpPower;

    [SerializeField] BaseAttackObject attackObject;

    // 플레이어를 조작할 수 있는 경우
    [SerializeField, ReadOnly] private bool _isPlayerInputControll = false;
    public bool IsPlayerInputControll
    {
        get { return _isPlayerInputControll; }
        protected set
        {
            if (_isPlayerInputControll == value)
                return;

            _isPlayerInputControll = value;
            ConnectInputActions(value);

            if (_isPlayerInputControll && coPlayerStateController == null)
            {
                coPlayerStateController = StartCoroutine(CoPlayerStateController());
            }
            else if(_isPlayerInputControll == false && CreatureState != ECreatureState.Dead)
            {
                // 강제로 모션 변환
                PlayAnimation(ECreatureState.Idle);
                IdleStateEnter();
            }
        }
    }

    // 게임 매니저에서 처리 예정
    protected override void Start()
    {
        base.Start();

        IsPlayerInputControll = true; 
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // 임시
        MoveSpeed = 5.0f;
        JumpPower = 8.0f;

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        CreatureType = ECreatureType.Player;
        CreatureState = ECreatureState.Idle;

        attackObject.SetInfo(ETag.Monster, OnAttackTarget);
    }

    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {
        Managers.Input.OnArrowKeyEntered -= OnArrowKey;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;
        Managers.Input.OnFKeyEntered -= OnAttackKey;

        if (isConnect)
        {
            Managers.Input.OnArrowKeyEntered += OnArrowKey;
            Managers.Input.OnSpaceKeyEntered += OnJumpKey;
            Managers.Input.OnFKeyEntered += OnAttackKey;
        }
    }

    public void OnArrowKey(Vector2 value)
    {
        if (!IsPlayerInputControll)
            return;

        moveDirection = value;
        CreatureState = ECreatureState.Move;
    }

    public void OnJumpKey()
    {
        if (!IsPlayerInputControll)
            return;

        if(creatureFoot.IsLandingGround)
            CreatureState = ECreatureState.Jump;
        else
            CreatureState = ECreatureState.JumpAir;
    }

    public void OnAttackKey()
    {
        if (!IsPlayerInputControll)
            return;

        CreatureState = ECreatureState.Attack;
    }

    #endregion

    #region CreatureState Controll

    Coroutine coPlayerStateController = null;
    protected IEnumerator CoPlayerStateController()
    {
        while (IsPlayerInputControll)
        {
            switch (CreatureState)
            {
                case ECreatureState.Idle:
                    UpdateIdleState();
                    break;
                case ECreatureState.Move:
                    UpdateMoveState();
                    break;
                case ECreatureState.Jump:
                    UpdateJumpState();
                    break;
                case ECreatureState.JumpAir:
                    UpdateJumpAirState();
                    break;
                case ECreatureState.Fall:
                    UpdateFallState();
                    break;
                case ECreatureState.Land:
                    UpdateLandState();
                    break;
                case ECreatureState.Attack:
                    UpdateAttackState();
                    break;
                case ECreatureState.Hit:
                    UpdateHitState();
                    break;
            }

            yield return null;
        }

        coPlayerStateController = null;
    }

    #region Idle Motion
    protected override bool IdleStateCondition()
    {
        if (base.IdleStateCondition() == false)
            return false;

        if (moveDirection != Vector2.zero)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected override void IdleStateEnter()
    {
        base.IdleStateEnter();

        InitRigidVelocityX();
        isCreatureStateLock = false;
    }

    protected virtual void UpdateIdleState()
    {

    }

    #endregion

    #region Move Motion
    protected override bool MoveStateCondition() 
    {
        if (base.MoveStateCondition() == false)
            return false;

        if (moveDirection.x == 0)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected override void MoveStateEnter()
    {
        base.MoveStateEnter();

    }

    protected virtual void UpdateMoveState()
    {
        FallDownCheck();
        Movement();

        if (moveDirection.x == 0)
            CreatureState = ECreatureState.Idle;
    }

    private void Movement()
    {
        SetRigidVelocityX(moveDirection.x * MoveSpeed);

        if (moveDirection.x > 0)
            LookLeft = false;
        else if (moveDirection.x < 0)
            LookLeft = true;
    }
    #endregion

    #region Jump Motion
    protected override bool JumpStateCondition()
    {
        if (base.JumpStateCondition() == false)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected override void JumpStateEnter()
    {
        base.JumpStateEnter();

        InitRigidVelocityY();
        SetRigidVelocityY(JumpPower);
    }

    protected virtual void UpdateJumpState()
    {
        Movement();
        FallDownCheck();

        // 착지 확인
        if (creatureFoot.IsLandingGround)
        {
            CreatureState = ECreatureState.Move;
        }
    }
    #endregion

    #region JumpAir Motion

    bool isJumpAir = false;
    protected override bool JumpAirStateCondition()
    {
        if (base.JumpStateCondition() == false)
            return false;

        if (creatureFoot.IsLandingGround)
            return false;

        if (isJumpAir) return false;

        return true;
    }

    protected override void JumpAirStateEnter()
    {
        base.JumpStateEnter();

        isJumpAir = true;
        InitRigidVelocityY();
        SetRigidVelocityY(JumpPower);
    }

    protected virtual void UpdateJumpAirState()
    {
        Movement();
        FallDownCheck();

        // 착지 확인
        if (creatureFoot.IsLandingGround)
        {
            CreatureState = ECreatureState.Move;
        }
    }
    #endregion

    #region Fall Motion
    protected override bool FallStateCondition()
    {
        if (base.FallStateCondition() == false)
            return false;

        if (Rigid.velocity.y >= 0)
            return false;

        return true;
    }

    protected override void FallStateEnter()
    {
        base.FallStateEnter();
    }

    private void UpdateFallState()
    {
        Movement();

        // 낙하 속도 제한 해야 함

        // 착지 확인
        if (creatureFoot.IsLandingGround)
        {
            CreatureState = ECreatureState.Land;
            CreatureState = ECreatureState.Move;
        }
    }

    protected virtual void FallDownCheck()
    {
        if (creatureFoot.IsLandingGround == false && Rigid.velocity.y < 0)
            CreatureState = ECreatureState.Fall;
    }
    #endregion

    #region Land Motion
    protected override bool LandStateCondition()
    {
        if (base.LandStateCondition() == false)
            return false;

        if (Rigid.velocity.y >= 0.01f)
            return false;

        return true;
    }

    protected override void LandStateEnter()
    {
        base.LandStateEnter();

        isJumpAir = false;
    }

    protected virtual void UpdateLandState()
    {
        if (IsEndCurrentState(ECreatureState.Land))
        {
            CreatureState = ECreatureState.Move;
            CreatureState = ECreatureState.Idle;
        }
    }

    public void OnLand()
    {

    }
    #endregion

    #region Attack Motion
    protected override bool AttackStateCondition()
    {
        if(base.AttackStateCondition() == false)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected override void AttackStateEnter()
    {
        base.AttackStateEnter();

        isCreatureStateLock = true;
        // 현재 입력키에 따라 앞으로 조금 전진하면서 공격해야 함
        InitRigidVelocityX();
        attackObject.SetActiveAttackObject(true);
    }

    protected virtual void UpdateAttackState()
    {
        if(IsEndCurrentState(ECreatureState.Attack))
        {
            isCreatureStateLock = false;
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
        attackTarget.OnHit();
    }

    #endregion

    #region Hit Motion
    Vector3 hitForceDir = Vector3.zero;

    protected override bool HitStateCondition()
    {
        if (base.HitStateCondition() == false)
            return false;

        return true;
    }

    protected override void HitStateEnter()
    {
        base.HitStateEnter();

        isCreatureStateLock = true;
        InitRigidVelocityY();

        if (hitForceDir != Vector3.zero)
        {
            SetRigidVelocity(hitForceDir * 3f);
        }
    }

    protected virtual void UpdateHitState()
    {
        if (IsEndCurrentState(ECreatureState.Hit))
        {
            isCreatureStateLock = false;
            CreatureState = ECreatureState.Move;
            CreatureState = ECreatureState.Idle;
            CreatureState = ECreatureState.Fall;
        }
    }

    protected override void HitStateExit()
    {
        base.HitStateExit();

        hitForceDir = Vector3.zero;
    }

    public void OnHit(AttackParam param = null)
    {
        if (param == null)
            return;

        LookLeft = !param.isAttackerLeft;
        hitForceDir.x = (param.isAttackerLeft) ? -1 : 1;
        hitForceDir.y = 1;
        isCreatureStateLock = false;
        CreatureState = ECreatureState.Hit;
    }
    #endregion

    #region Dead Motion
    protected override void DeadStateEnter()
    {
        base.DeadStateEnter();
    }

    protected override bool DeadStateCondition()
    {
        return true;
    }
    #endregion
    
    #endregion
}
