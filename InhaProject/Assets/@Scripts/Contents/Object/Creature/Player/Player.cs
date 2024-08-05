using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static Define;

public class Player : Creature
{
    // 임시 데이터들
    protected float MoveSpeed = 3.0f;
    protected float JumpPower = 5.0f;

    // 플레이어를 조작할 수 있는 경우
    [SerializeField] private bool _isPlayerInputControll = false;
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
                IdleStateOperate();
            }
        }
    }

    private void Start()
    {
        // 임시
        IsPlayerInputControll = true; // 게임 매니저에서 할 것
        SetInfo(0);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SetInfo();
        
        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        CreatureType = ECreatureType.Player;

        Camera.main.GetOrAddComponent<CameraController>().Target = this;
    }

    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {
        Managers.Input.OnArrowKeyEntered -= OnArrowKey;
        Managers.Input.OnSpaceKeyEntered -= OnJumpKey;

        if (isConnect)
        {
            Managers.Input.OnArrowKeyEntered += OnArrowKey;
            Managers.Input.OnSpaceKeyEntered += OnJumpKey;
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

        CreatureState = ECreatureState.Jump;
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
                case ECreatureState.Fall:
                    UpdateFallState();
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

        return true;
    }

    private void UpdateIdleState()
    {

    }

    protected override void IdleStateOperate()
    {
        base.IdleStateOperate();
    }
    #endregion

    #region Move Motion
    protected override bool MoveStateCondition()
    {
        if (base.MoveStateCondition() == false)
            return false;

        if (moveDirection.x == 0 && moveDirection.y == 0)
            return false;



        return true;
    }

    private void UpdateMoveState()
    {
        // 이동할 방향 쳐다봐야 함

        SetRigidVelocity(moveDirection * MoveSpeed);

        CreatureState = ECreatureState.Idle;
    }

    protected override void MoveStateOperate()
    {
        base.MoveStateOperate();


    }
    #endregion

    #region Jump Motion
    protected override bool JumpStateCondition()
    {
        if (base.JumpStateCondition() == false)
            return false;

        return true;
    }

    private void UpdateJumpState()
    {
        // 착지 확인
        if (creatureFoot.IsLandingGround)
        {
            CreatureState = ECreatureState.Move;
            CreatureState = ECreatureState.Idle;
            return;
        }
    }

    protected override void JumpStateOperate()
    {
        base.JumpStateOperate();

        // SetRigidVelocityY(JumpPower);
    }
    #endregion

    #region Fall Motion
    protected override bool FallStateCondition()
    {

        return true;
    }

    private void UpdateFallState()
    {

    }

    protected override void FallStateOperate()
    {
        base.FallStateOperate();
    }
    #endregion

    #region Attack Motion
    protected override bool AttackStateCondition()
    {

        return true;
    }

    private void UpdateAttackState()
    {

    }

    protected override void AttackStateOperate()
    {
        base.JumpStateOperate();
    }
    #endregion

    #region Dead Motion
    protected override bool DeadStateCondition()
    {
        return true;
    }

    protected override void DeadStateOperate()
    {
        base.DeadStateOperate();
    }
    #endregion
    
    #endregion
}
