using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static Define;

/// <summary>
/// 프리팹 이름과 같아야 함
/// </summary>
public enum EPlayerType
{
    Player = 0,
}

public enum EPlayerState
{
    None,
    Idle,
    Walk, // 일단 미사용
    Move, // Run으로 일단 사용
    Jump,
    Fall,
    Land,

    Attack,
    Skill1,
    Skill2,
    Skill3,
    Skill4,

    Guard,
    Block,
    Hit,

    Down, // 미구현
    GetUp, // 미구현

    Dead
}

[System.Serializable]
public class PlayerData
{
    public float CurrHp;
    public float CurrMp;
    public float StrikingPower;     // 공격력
    public float MoveSpeed;
    public float JumpPower;

    public PlayerData(JPlayerData jPlayerData)
    {
        CurrHp = jPlayerData.MaxHp;
        CurrMp = jPlayerData.MaxMp;
        StrikingPower = jPlayerData.StrikingPower;
        MoveSpeed = jPlayerData.MoveSpeed;
        JumpPower = jPlayerData.JumpPower;
    }
}

public class Player : Creature, IHitEvent
{
    [field: SerializeField, ReadOnly] public EPlayerType PlayerType { get; protected set; }
    [field: SerializeField] public PlayerData PlayerInfo { get; protected set; }

    [SerializeField, ReadOnly] AttackObject skillAttackObject;
    [SerializeField, ReadOnly] AttackObject attackObject = null;

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

            if (_isPlayerInputControll)
            {
                // 강제로 모션 변환
                PlayAnimation(EPlayerState.Idle);
                IdleStateEnter();

                if (coPlayerStateController == null)
                    coPlayerStateController = StartCoroutine(CoPlayerStateController());
            }
        }
    }

    [SerializeField, ReadOnly] protected bool isPlayerStateLock = false;
    [SerializeField, ReadOnly]
    protected EPlayerState _playerState = EPlayerState.None;
    public virtual EPlayerState PlayerState
    {
        get { return _playerState; }
        protected set
        {
            if (value != EPlayerState.Idle && isPlayerStateLock)
                return;

            if (_playerState == EPlayerState.Dead)
                return;

            if (_playerState == value)
                return;

            bool isChangeState = true;
            switch (value)
            {
                case EPlayerState.Idle:
                    isChangeState = IdleStateCondition();
                    break;
                case EPlayerState.Move:
                    isChangeState = MoveStateCondition();
                    break;
                case EPlayerState.Jump:
                    isChangeState = JumpStateCondition();
                    break;
                case EPlayerState.Fall:
                    isChangeState = FallStateCondition();
                    break;
                case EPlayerState.Land:
                    isChangeState = LandStateCondition();
                    break;
                case EPlayerState.Attack:
                    isChangeState = AttackStateCondition();
                    break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4:
                    isChangeState = SkillStateCondition();
                    break;
                case EPlayerState.Dead:
                    isChangeState = DeadStateCondition();
                    break;
            }

            if (isChangeState == false)
                return;

            switch (_playerState)
            {
                case EPlayerState.Idle:
                    IdleStateExit();
                    break;
                case EPlayerState.Move:
                    MoveStateExit();
                    break;
                case EPlayerState.Jump:
                    JumpStateExit();
                    break;
                case EPlayerState.Fall:
                    FallStateExit();
                    break;
                case EPlayerState.Land:
                    LandStateExit();
                    break;
                case EPlayerState.Attack:
                    AttackStateExit();
                    break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4:
                    SkillStateExit();
                    break;
                case EPlayerState.Guard:
                    GuardStateExit();
                    break;
                case EPlayerState.Block:
                    BlockStateExit();
                    break;
                case EPlayerState.Hit:
                    HitStateExit();
                    break;
                case EPlayerState.Dead:
                    DeadStateExit();
                    break;
            }

            _playerState = value;
            PlayAnimation(value);

            switch (value)
            {
                case EPlayerState.Idle:
                    IdleStateEnter();
                    break;
                case EPlayerState.Move:
                    MoveStateEnter();
                    break;
                case EPlayerState.Jump:
                    JumpStateEnter();
                    break;
                case EPlayerState.Fall:
                    FallStateEnter();
                    break;
                case EPlayerState.Land:
                    LandStateEnter();
                    break;
                case EPlayerState.Attack:
                    AttackStateEnter();
                    break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4:
                    SkillStateEnter();
                    break;
                case EPlayerState.Guard:
                    GuardStateEnter();
                    break;
                case EPlayerState.Block:
                    BlockStateEnter();
                    break;
                case EPlayerState.Hit:
                    HitStateEnter();
                    break;
                case EPlayerState.Dead:
                    DeadStateEnter();
                    break;
            }
        }
    }

    // 게임 매니저에서 처리 예정
    protected override void Start()
    {
        base.Start();

        IsPlayerInputControll = true; 
    }

    protected override void Reset()
    {
        base.Reset();

        skillAttackObject ??= Util.FindChild<AttackObject>(this.gameObject, "FX_Projectile1", true);
        attackObject = Managers.Object.SpawnAttackObject(EAttackObjectType.PlayerAttackObject, this.transform).GetComponent<AttackObject>();
        attackObject.SetActive(false);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Player;
        PlayerState = EPlayerState.Idle;

        this.gameObject.tag = ETag.Player.ToString();
        this.gameObject.layer = (int)ELayer.Player;

        Collider.excludeLayers += 1 << (int)ELayer.Monster;

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        PlayerType = Util.ParseEnum<EPlayerType>(gameObject.name); // 임시
        PlayerInfo = new PlayerData(Managers.Data.PlayerDict[(int)PlayerType]);

        skillAttackObject.SetInfo(ETag.Player, OnSkillAttackTarget);
        attackObject.SetInfo(ETag.Player, OnAttackTarget);
    }

    public override Vector3 GetCameraTargetPos()
    {
        Vector3 cameraTargetPos = base.GetCameraTargetPos();
        cameraTargetPos.y += (Collider.size.y * 1.5f);
        return cameraTargetPos;
    }

    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {
        Managers.Input.OnArrowKeyEntered -= OnArrowKey;
        Managers.Input.OnCKeyEntered -= OnJumpKey;
        Managers.Input.OnVKeyEntered -= OnGuardKey;
        Managers.Input.OnXKeyEntered -= OnAttackKey;
        Managers.Input.OnZKeyEntered += OnSkillKey1;
        Managers.Input.OnAKeyEntered -= OnSkillKey2;
        Managers.Input.OnSKeyEntered -= OnSkillKey3;
        Managers.Input.OnDKeyEntered -= OnSkillKey4;

        if (isConnect)
        {
            Managers.Input.OnArrowKeyEntered += OnArrowKey;
            Managers.Input.OnCKeyEntered += OnJumpKey;
            Managers.Input.OnVKeyEntered += OnGuardKey;
            Managers.Input.OnXKeyEntered += OnAttackKey;
            Managers.Input.OnZKeyEntered += OnSkillKey1;
            Managers.Input.OnAKeyEntered += OnSkillKey2;
            Managers.Input.OnSKeyEntered += OnSkillKey3;
            Managers.Input.OnDKeyEntered += OnSkillKey4;
        }
    }

    public void OnArrowKey(Vector2 value)
    {
        if (!IsPlayerInputControll)
            return;

        moveDirection = value;
        PlayerState = EPlayerState.Move;
    }

    public void OnGuardKey()
    {
        if(!IsPlayerInputControll) 
            return;

        PlayerState = EPlayerState.Guard;
    }

    public void OnJumpKey()
    {
        if (!IsPlayerInputControll)
            return;

        if(creatureFoot.IsLandingGround)
            PlayerState = EPlayerState.Jump;
    }

    public void OnAttackKey()
    {
        if (!IsPlayerInputControll)
            return;

        PlayerState = EPlayerState.Attack;
    }

    public void OnSkillKey1()
    {
        if (!IsPlayerInputControll)
            return;

        if (skillNum != 0)
            return;

        skillNum = 1;
        PlayerState = EPlayerState.Skill1;

        if (PlayerState != EPlayerState.Skill1)
            skillNum = 0;
    }

    public void OnSkillKey2()
    {
        if (!IsPlayerInputControll)
            return;

        if (skillNum != 0)
            return;

        skillNum = 2;
        PlayerState = EPlayerState.Skill2;

        if (PlayerState != EPlayerState.Skill2)
            skillNum = 0;
        else
            isSuperArmour = true;
    }

    public void OnSkillKey3()
    {
        if (!IsPlayerInputControll)
            return;

        if (skillNum != 0)
            return;

        skillNum = 3;
        PlayerState = EPlayerState.Skill3;

        if (PlayerState != EPlayerState.Skill3)
            skillNum = 0;
        else
            isSuperArmour = true;
    }

    public void OnSkillKey4()
    {
        if (!IsPlayerInputControll)
            return;

        if (skillNum != 0)
            return;

        skillNum = 4;
        PlayerState = EPlayerState.Skill4;

        if (PlayerState != EPlayerState.Skill4)
            skillNum = 0;
        else
            isInvincibility = true;
    }

    #endregion

    #region PlayerState Controll

    Coroutine coPlayerStateController = null;
    protected IEnumerator CoPlayerStateController()
    {
        while (IsPlayerInputControll)
        {
            switch (PlayerState)
            {
                case EPlayerState.Idle:
                    UpdateIdleState();
                    break;
                case EPlayerState.Move:
                    UpdateMoveState();
                    break;
                case EPlayerState.Jump:
                    UpdateJumpState();
                    break;
                case EPlayerState.Fall:
                    UpdateFallState();
                    break;
                case EPlayerState.Land:
                    UpdateLandState();
                    break;
                case EPlayerState.Attack:
                    UpdateAttackState();
                    break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4:
                    UpdateSkillState();
                    break;
                case EPlayerState.Guard:
                    UpdateGuardState();
                    break;
                case EPlayerState.Block:
                    UpdateBlockState();
                    break;
                case EPlayerState.Hit:
                    UpdateHitState();
                    break;
            }

            yield return null;
        }

        coPlayerStateController = null;
    }

    #region Idle Motion
    protected virtual bool IdleStateCondition()
    {
        if (moveDirection != Vector2.zero)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void IdleStateEnter()
    {
        InitRigidVelocityX();
        isPlayerStateLock = false;
    }

    protected virtual void UpdateIdleState()
    {

    }

    protected virtual void IdleStateExit()
    {

    }
    #endregion

    #region Move Motion
    protected virtual bool MoveStateCondition() 
    {
        if (moveDirection.x == 0)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void MoveStateEnter()
    {


    }

    protected virtual void UpdateMoveState()
    {
        FallDownCheck();
        Movement();

        if (moveDirection.x == 0)
            PlayerState = EPlayerState.Idle;
    }

    protected virtual void MoveStateExit()
    {

    }

    private void Movement()
    {
        SetRigidVelocityX(moveDirection.x * PlayerInfo.MoveSpeed);

        if (moveDirection.x > 0)
            LookLeft = false;
        else if (moveDirection.x < 0)
            LookLeft = true;
    }
    #endregion

    #region Jump Motion
    protected virtual bool JumpStateCondition()
    {
        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void JumpStateEnter()
    {
        InitRigidVelocityY();
        SetRigidVelocityY(PlayerInfo.JumpPower);
    }

    protected virtual void UpdateJumpState()
    {
        Movement();
        FallDownCheck();

        // 착지 확인
        if (creatureFoot.IsLandingGround)
        {
            PlayerState = EPlayerState.Move;
        }
    }

    protected virtual void JumpStateExit()
    {

    }
    #endregion

    #region Fall Motion
    protected virtual bool FallStateCondition()
    {
        if (Rigid.velocity.y >= 0)
            return false;

        return true;
    }

    protected virtual void FallStateEnter()
    {

    }

    private void UpdateFallState()
    {
        Movement();

        // 낙하 속도 제한 해야 함

        // 착지 확인
        if (creatureFoot.IsLandingGround)
        {
            PlayerState = EPlayerState.Land;
            PlayerState = EPlayerState.Move;
        }
    }

    protected virtual void FallStateExit()
    {

    }

    protected virtual void FallDownCheck()
    {
        if (creatureFoot.IsLandingGround == false && Rigid.velocity.y < 0)
            PlayerState = EPlayerState.Fall;
    }
    #endregion

    #region Land Motion
    protected virtual bool LandStateCondition()
    {
        if (Rigid.velocity.y >= 0.01f)
            return false;

        return true;
    }

    protected virtual void LandStateEnter()
    {

    }

    protected virtual void UpdateLandState()
    {
        if (IsEndCurrentState(EPlayerState.Land))
        {
            PlayerState = EPlayerState.Move;
            PlayerState = EPlayerState.Idle;
        }
    }

    protected virtual void LandStateExit()
    {

    }

    public void OnLand()
    {

    }
    #endregion

    #region Attack Motion

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        if (PlayerState == EPlayerState.Attack)
        {
            attackTarget.OnHit(new AttackParam(this, LookLeft, PlayerInfo.StrikingPower));
        }
    }
    protected virtual bool AttackStateCondition()
    {
        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void AttackStateEnter()
    {
        isPlayerStateLock = true;
        InitRigidVelocityX();

    }

    protected virtual void UpdateAttackState()
    {
        if(IsEndCurrentState(EPlayerState.Attack))
        {
            isPlayerStateLock = false;
            PlayerState = EPlayerState.Move;
            PlayerState = EPlayerState.Idle;
        }
    }

    protected virtual void AttackStateExit()
    {

    }
    #endregion

    #region Skill Motion
    [SerializeField, ReadOnly] int skillNum = 0;
    public void OnSkillAttackTarget(IHitEvent skillAttackTarget)
    {
        if (PlayerState == EPlayerState.Skill1 || PlayerState == EPlayerState.Skill2 
            || PlayerState == EPlayerState.Skill3 || PlayerState == EPlayerState.Skill4)
        {
            skillAttackTarget.OnHit(new AttackParam(this, LookLeft, PlayerInfo.StrikingPower * 2f));
        }
    }

    protected virtual bool SkillStateCondition()
    {
        if (skillNum == 0)
            return false;

        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void SkillStateEnter()
    {
        isPlayerStateLock = true;
        InitRigidVelocityX();
    }

    protected virtual void UpdateSkillState()
    {
        if (IsEndCurrentState(Util.ParseEnum<EPlayerState>($"Skill{skillNum}")))
        {
            isPlayerStateLock = false;
            PlayerState = EPlayerState.Move;
            PlayerState = EPlayerState.Idle;
        }
    }

    protected virtual void SkillStateExit()
    {
        skillNum = 0;
        isSuperArmour = false;
        isInvincibility = false;
    }

    // Animation Clip Event
    public void OnActiveAttackObject()
    {
        attackObject.SetActive(true);
    }

    // Animation Clip Event
    public void OnDeactiveAttackobject()
    {
        attackObject.SetActive(false);
    }

    // Animation Clip Event
    public void OnStartSlowEffect()
    {
        Time.timeScale = 0.5f; // 게임매니저로 옮길 예정? (임시)
    }

    // Animation Clip Event
    public void OnEndSlowEffect()
    {
        Time.timeScale = 1.0f; // 게임매니저로 옮길 예정? (임시)
    }
    #endregion

    #region Guard Motion
    protected virtual bool GuardStateCondition()
    {
        if (creatureFoot.IsLandingGround == false)
            return false;

        return true;
    }
    
    protected virtual void GuardStateEnter()
    {
        InitRigidVelocityX();
    }

    protected virtual void UpdateGuardState()
    {
        if (IsEndCurrentState(EPlayerState.Guard))
        {
            PlayerState = EPlayerState.Idle;
        }
    }    

    protected virtual void GuardStateExit()
    {

    }
    #endregion

    #region Block Motion
    protected virtual bool BlockStateCondition()
    {
        return true;
    }

    protected virtual void BlockStateEnter()
    {
        isPlayerStateLock = true;
        SetRigidVelocityX(2f * ((LookLeft) ? 1 : -1));
        Time.timeScale = 0.5f; // 게임매니저로 옮길 예정? (임시)
    }

    protected virtual void UpdateBlockState()
    {
        if (IsEndCurrentState(EPlayerState.Block))
        {
            isPlayerStateLock = false;
            PlayerState = EPlayerState.Move;
            PlayerState = EPlayerState.Idle;
            PlayerState = EPlayerState.Fall;
        }
    }

    protected virtual void BlockStateExit()
    {
        Time.timeScale = 1f; // 게임매니저로 옮길 예정? (임시)
    }
    #endregion

    #region Hit Motion
    Vector3 hitForceDir = Vector3.zero;
    [SerializeField, ReadOnly] bool isSuperArmour = false;
    [SerializeField, ReadOnly] bool isInvincibility = false;

    public void OnHit(AttackParam param = null)
    {
        if (isInvincibility || param == null)
            return;

        Vector3 subVec = new Vector3(0, Collider.size.y * 0.7f, 0);
        if (PlayerState == EPlayerState.Guard && param.isAttackerLeft == !LookLeft)
        {
            subVec.x += Collider.size.x * ((LookLeft) ? -1 : 1) * 2;
            Managers.Object.SpawnEffectObject(EEffectObjectType.PlayerHitEffect, this.transform.position + subVec);
            isPlayerStateLock = false;
            PlayerState = EPlayerState.Block;
            return;
        }
        
        if (isSuperArmour)
        {
            subVec.x += Collider.size.x * ((LookLeft) ? 1 : -1) * 2;
            subVec.y += Random.Range(-0.5f, 0.5f);
            Managers.Object.SpawnEffectObject(EEffectObjectType.PlayerHitEffect, this.transform.position + subVec);
            return;
        }

        LookLeft = !param.isAttackerLeft;
        hitForceDir.x = param.pushPower * ((param.isAttackerLeft) ? -1 : 1);
        Managers.Object.SpawnEffectObject(EEffectObjectType.PlayerHitEffect, this.transform.position + subVec);
        isPlayerStateLock = false;
        PlayerState = EPlayerState.Hit;
    }

    protected virtual bool HitStateCondition()
    {
        return true;
    }

    protected virtual void HitStateEnter()
    {
        isPlayerStateLock = true;
        InitRigidVelocityY();

        if (hitForceDir != Vector3.zero)
        {
            SetRigidVelocity(hitForceDir);
        }
    }

    protected virtual void UpdateHitState()
    {
        if (IsEndCurrentState(EPlayerState.Hit))
        {
            isPlayerStateLock = false;
            PlayerState = EPlayerState.Move;
            PlayerState = EPlayerState.Idle;
            PlayerState = EPlayerState.Fall;
        }
    }

    protected virtual void HitStateExit()
    {
        hitForceDir = Vector3.zero;
    }

    // Animation Clip Event
    public void OnInitHitForce()
    {
        if(creatureFoot.IsLandingGround)
            InitRigidVelocityX();
    }
    #endregion

    #region Dead Motion
    protected virtual bool DeadStateCondition()
    {
        if (PlayerInfo.CurrHp > 0)
            return false;

        return true;
    }

    protected virtual void DeadStateEnter()
    {
    }

    protected virtual void DeadStateExit()
    {

    }
    #endregion

    #endregion
}
