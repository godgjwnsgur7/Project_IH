using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static Define;

public class Player : Creature, IHitEvent
{
    // 임시 데이터들
    protected float MoveSpeed = 5.0f;
    protected float JumpPower = 8.0f;

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

    private void UpdateIdleState()
    {

    }

    protected override void IdleStateEnter()
    {
        base.IdleStateEnter();

        SetRigidVelocityZeroToX();
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

    private void UpdateMoveState()
    {
        FallDownCheck();
        Movement();

        if (moveDirection.x == 0)
            CreatureState = ECreatureState.Idle;
    }

    private void Movement()
    {
        PushRigidVelocityX(moveDirection.x * MoveSpeed * 0.1f);

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
        PushRigidVelocityY(JumpPower);
    }

    private void UpdateJumpState()
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
        PushRigidVelocityY(JumpPower);
    }

    private void UpdateJumpAirState()
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

    private void FallDownCheck()
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

    private void UpdateLandState()
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

        attackObject.SetActiveWeapon(true);
    }

    private void UpdateAttackState()
    {
        if(IsEndCurrentState(ECreatureState.Attack))
        {
            CreatureState = ECreatureState.Move;
            CreatureState = ECreatureState.Idle;
        }
    }

    protected override void AttackStateExit()
    {
        base.AttackStateExit();
        attackObject.SetActiveWeapon(false);
    }

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        attackTarget.OnHit();
    }

    #endregion

    #region Hit Motion
    public void OnHit(AttackParam param = null)
    {
        Debug.Log("플레이어 히트당함");
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
