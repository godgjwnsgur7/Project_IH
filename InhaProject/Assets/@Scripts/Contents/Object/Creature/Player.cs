using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static Define;

#region Enum Group
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
    JumpAir,
    Fall,
    Land,
    Dash,

    Attack,
    Skill1,
    Skill2,
    Skill3,
    Skill4,

    Guard,
    Block,
    
    Hit,
    Down,
    DownLand,
    GetUp,

    Dead
}

public enum EPlayerSkillType
{
    Default = 0,
    Skill1 = 1,
    Skill2 = 2,
    Skill3 = 3,
    Skill4 = 4,
    Guard = 5,
    Dash = 6,
    Max = 7,
}
#endregion

#region PlayerData
[Serializable]
public class PlayerData
{
    public float MaxHp;
    public float CurrHp;
    public float MaxMp;
    public float CurrMp;
    public float StrikingPower;     // 공격력
    public float MoveSpeed;
    public float DashSpeed;
    public float JumpPower;

    public PlayerData(JPlayerData jPlayerData)
    {
        MaxHp = jPlayerData.MaxHp;
        CurrHp = jPlayerData.MaxHp;
        MaxMp = jPlayerData.MaxMp;
        CurrMp = jPlayerData.MaxMp;
        StrikingPower = jPlayerData.StrikingPower;
        MoveSpeed = jPlayerData.MoveSpeed;
        DashSpeed = jPlayerData.DashSpeed;
        JumpPower = jPlayerData.JumpPower;
    }
}

[Serializable]
public class PlayerSkill
{
    public EPlayerSkillType skillType;
    public float coolTime;
    public float mpAmount;
    public bool isAvailable;

    public PlayerSkill(EPlayerSkillType skillType, float coolTime, float mpAmount,
        bool isAvailable = true)
    {
        this.skillType = skillType;
        this.coolTime = coolTime;
        this.mpAmount = mpAmount;
        this.isAvailable = isAvailable;
    }
}

[Serializable]
public class PlayerAttackSkill : PlayerSkill
{
    public List<float> damageRatioList;

    public PlayerAttackSkill(EPlayerSkillType skillType, List<float> damageRatioList, float coolTime, float mpAmount, 
        bool isAvailable = true) : base(skillType, coolTime, mpAmount, isAvailable)
    {
        this.damageRatioList = damageRatioList;
    }
}
#endregion

public class Player : Creature, IHitEvent
{
    [SerializeReference, ReadOnly] PlayerCameraController playerCameraController = null;

    /// <summary> Type, CoolTime </summary>
    public event Action<EPlayerSkillType, float> OnUseSkill = null;

    public event Action<float> OnChangedHp = null;
    public event Action<float> OnChangedMp = null;

    public Dictionary<EPlayerSkillType, PlayerSkill> PlayerSkillDict { get; protected set; }
    [field: SerializeField, ReadOnly] public EPlayerType PlayerType { get; protected set; }
    [field: SerializeField, ReadOnly] private PlayerData playerData = null;

    [SerializeField, ReadOnly] BaseAttackObject skillAttackObject;
    [SerializeField, ReadOnly] BaseAttackObject attackObject = null;

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
                case EPlayerState.Idle: isChangeState = IdleStateCondition(); break;
                case EPlayerState.Move: isChangeState = MoveStateCondition(); break;
                case EPlayerState.Jump: isChangeState = JumpStateCondition(); break;
                case EPlayerState.JumpAir: isChangeState = JumpAirStateCondition(); break;
                case EPlayerState.Fall: isChangeState = FallStateCondition(); break;
                case EPlayerState.Land: isChangeState = LandStateCondition(); break;
                case EPlayerState.Dash: isChangeState = DashStateCondition(); break;
                case EPlayerState.Attack: isChangeState = AttackStateCondition(); break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4: isChangeState = SkillStateCondition(); break;
                case EPlayerState.Guard: isChangeState = GuardStateCondition(); break;
                case EPlayerState.Hit: isChangeState = HitStateCondition(); break;
                case EPlayerState.Down: isChangeState = DownStateCondition(); break;
                case EPlayerState.DownLand: isChangeState = DownLandStateCondition(); break;
                case EPlayerState.GetUp: isChangeState = GetUpStateCondition(); break;
                case EPlayerState.Dead: isChangeState = DeadStateCondition(); break;
            }

            if (isChangeState == false)
                return;

            switch (_playerState)
            {
                case EPlayerState.Idle: IdleStateExit(); break;
                case EPlayerState.Move: MoveStateExit(); break;
                case EPlayerState.Jump: JumpStateExit(); break;
                case EPlayerState.JumpAir: JumpAirStateExit(); break;
                case EPlayerState.Fall: FallStateExit(); break;
                case EPlayerState.Land: LandStateExit(); break;
                case EPlayerState.Dash: DashStateExit(); break;
                case EPlayerState.Attack: AttackStateExit(); break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4: SkillStateExit(); break;
                case EPlayerState.Guard: GuardStateExit(); break;
                case EPlayerState.Block: BlockStateExit(); break;
                case EPlayerState.Hit: HitStateExit(); break;
                case EPlayerState.Down: DownStateExit(); break;
                case EPlayerState.DownLand: DownLandStateExit(); break;
                case EPlayerState.GetUp: GetUpStateExit(); break;
            }

            _playerState = value;
            PlayAnimation(value);

            switch (value)
            {
                case EPlayerState.Idle: IdleStateEnter(); break;
                case EPlayerState.Move: MoveStateEnter(); break;
                case EPlayerState.Jump: JumpStateEnter(); break;
                case EPlayerState.JumpAir: JumpAirStateEnter(); break;
                case EPlayerState.Fall: FallStateEnter(); break;
                case EPlayerState.Land: LandStateEnter(); break;
                case EPlayerState.Dash: DashStateEnter(); break;
                case EPlayerState.Attack: AttackStateEnter(); break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4: SkillStateEnter(); break;
                case EPlayerState.Guard: GuardStateEnter(); break;
                case EPlayerState.Block: BlockStateEnter(); break;
                case EPlayerState.Hit: HitStateEnter(); break;
                case EPlayerState.Down: DownStateEnter(); break;
                case EPlayerState.DownLand: DownLandStateEnter(); break;
                case EPlayerState.GetUp: GetUpStateEnter(); break;
                case EPlayerState.Dead: DeadStateEnter(); break;
            }
        }
    }

    private void OnDestroy()
    {
        ConnectInputActions(false);
    }

    protected override void Reset()
    {
        base.Reset();

        playerCameraController = Util.FindChild<PlayerCameraController>(gameObject, "PlayerCamera", true);
        skillAttackObject ??= Util.FindChild<BaseAttackObject>(this.gameObject, "FX_Projectile1", true);
        attackObject ??= Util.FindChild<BoxAttackObject>(this.gameObject, "PlayerAttackObject");
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        attackObject.SetActive(false);

        CreatureType = ECreatureType.Player;
        PlayerState = EPlayerState.Idle;

        this.gameObject.tag = ETag.Player.ToString();
        this.gameObject.layer = (int)ELayer.Player;

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);

        PlayerType = Util.ParseEnum<EPlayerType>(gameObject.name); // 임시
        playerData = new PlayerData(Managers.Data.PlayerDict[(int)PlayerType]);
        playerCameraController.SetEnabled(false);

        SetSkillInfo();

        skillAttackObject.SetActive(true);
        skillAttackObject.SetActiveCollider(false);
        skillAttackObject.SetInfo(ETag.Player, OnSkillAttackTarget);
        attackObject.SetInfo(ETag.Player, OnAttackTarget);

        UI_GameScene gameSceneUI = Managers.UI.SceneUI.GetComponent<UI_GameScene>();
        if(gameSceneUI != null)
        {
            gameSceneUI.SetInfo(OnReadyToSkill, playerData.MaxHp, playerData.MaxMp);
            inventory = gameSceneUI.uiInventory.inventory;
        }

        IsPlayerInputControll = true;
    }

    private void SetSkillInfo()
    {
        // 플레이어 스킬 데이터로 분리 예정 (임시)
        EPlayerSkillType skillType;
        PlayerSkillDict = new Dictionary<EPlayerSkillType, PlayerSkill>();

        skillType = EPlayerSkillType.Dash;
        PlayerSkillDict.Add(skillType, new PlayerSkill(skillType, 3, 0));

        skillType = EPlayerSkillType.Guard;
        PlayerSkillDict.Add(skillType, new PlayerSkill(skillType, 4, 0));

        skillType = EPlayerSkillType.Skill1;
        PlayerSkillDict.Add(skillType, new PlayerAttackSkill(skillType, new List<float> { 1.0f }, 0, 30));

        skillType = EPlayerSkillType.Skill2;
        PlayerSkillDict.Add(skillType, new PlayerAttackSkill(skillType, new List<float> { 1.5f }, 7, 100));

        skillType = EPlayerSkillType.Skill3;
        PlayerSkillDict.Add(skillType, new PlayerAttackSkill(skillType, new List<float> { 2.0f }, 10, 150));

        skillType = EPlayerSkillType.Skill4;
        PlayerSkillDict.Add(skillType, new PlayerAttackSkill(skillType, new List<float> { 0.3f, 0.4f, 0.5f, 0.6f, 2.5f }, 20, 350));
    }

    public override Vector3 GetCameraTargetPos()
    {
        Vector3 cameraTargetPos = base.GetCameraTargetPos();
        cameraTargetPos.y += (Collider.size.y * 1.5f);
        return cameraTargetPos;
    }

    public void AddPosition(Vector3 addVec)
    {
        this.transform.position += addVec;
    }

    #region Inventory
    [SerializeField, ReadOnly] Inventory inventory;

    public void OnGetInventroyItem(IInventoryItem item)
    {
        inventory.AddItem(item);
    }

    public void OnGetApplyItme(ItemParam param)
    {
        if(param is ApplyItemParam applyItemParam)
        {
            if (applyItemParam.IsHp)
            {
                RecoveryHp(applyItemParam.Heal);
            }
            else
            {
                RecoveryMp(applyItemParam.Heal);
            }
        }
    }

    public void OnUseItemKey(int slotId)
    {
        InventoryItemData itemData = inventory.GetItem(slotId);
        bool isUse = inventory.RemoveItem(itemData);

        if (isUse)
        {
            if (itemData.param is PotionItemParam potion)
            {
                if (potion.IsHp)
                {
                    RecoveryHp(potion.Heal);
                }
                else
                {
                    RecoveryMp(potion.Heal);
                }
            }
        }
    }
    #endregion

    #region Input
    private Vector2 moveDirection = Vector2.zero;

    private void ConnectInputActions(bool isConnect)
    {
        Managers.Input.OnArrowKeyEntered -= OnArrowKey;
        Managers.Input.OnSpaceKeyEntered -= OnDashKey;
        Managers.Input.OnCKeyEntered -= OnJumpKey;
        Managers.Input.OnVKeyEntered -= OnGuardKey;
        Managers.Input.OnXKeyEntered -= OnAttackKey;
        Managers.Input.OnZKeyEntered -= OnSkillKey1;
        Managers.Input.OnAKeyEntered -= OnSkillKey2;
        Managers.Input.OnSKeyEntered -= OnSkillKey3;
        Managers.Input.OnDKeyEntered -= OnSkillKey4;
        Managers.Input.OnNum1KeyEntered -= OnUseItemSlot1;
        Managers.Input.OnNum2KeyEntered -= OnUseItemSlot2;

        if (isConnect)
        {
            Managers.Input.OnArrowKeyEntered += OnArrowKey;
            Managers.Input.OnSpaceKeyEntered += OnDashKey;
            Managers.Input.OnCKeyEntered += OnJumpKey;
            Managers.Input.OnVKeyEntered += OnGuardKey;
            Managers.Input.OnXKeyEntered += OnAttackKey;
            Managers.Input.OnZKeyEntered += OnSkillKey1;
            Managers.Input.OnAKeyEntered += OnSkillKey2;
            Managers.Input.OnSKeyEntered += OnSkillKey3;
            Managers.Input.OnDKeyEntered += OnSkillKey4;
            Managers.Input.OnNum1KeyEntered += OnUseItemSlot1;
            Managers.Input.OnNum2KeyEntered += OnUseItemSlot2;
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

        if (!CreatureFoot.IsLandingGround)
            PlayerState = EPlayerState.JumpAir;
        else
            PlayerState = EPlayerState.Jump;
    }

    public void OnDashKey()
    {
        if (!IsPlayerInputControll)
            return;

        if (!CreatureFoot.IsLandingGround)
            return;

        PlayerState = EPlayerState.Dash;
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
            IsInvincibility = true;
    }

    public void OnUseItemSlot1()
    {
        OnUseItemKey(1);
    }

    public void OnUseItemSlot2()
    {
        OnUseItemKey(2);
    }

    #endregion

    #region PlayerState Controll

    Coroutine coPlayerStateController = null;
    protected IEnumerator CoPlayerStateController()
    {
        float timer = 0.0f;
        while (IsPlayerInputControll)
        {
            if(!isDead)
            {
                timer += Time.deltaTime;
                if (timer >= 1.0f)
                {
                    timer -= 1.0f;
                    NaturalRecovery();
                }
            }
            
            switch (PlayerState)
            {
                case EPlayerState.Idle: UpdateIdleState(); break;
                case EPlayerState.Move: UpdateMoveState(); break;
                case EPlayerState.Jump: UpdateJumpState(); break;
                case EPlayerState.JumpAir: UpdateJumpAirState(); break;
                case EPlayerState.Fall: UpdateFallState(); break;
                case EPlayerState.Land: UpdateLandState(); break;
                case EPlayerState.Dash: UpdateDashState(); break;
                case EPlayerState.Attack: UpdateAttackState(); break;
                case EPlayerState.Skill1:
                case EPlayerState.Skill2:
                case EPlayerState.Skill3:
                case EPlayerState.Skill4: UpdateSkillState(); break;
                case EPlayerState.Guard: UpdateGuardState(); break;
                case EPlayerState.Block: UpdateBlockState(); break;
                case EPlayerState.Hit: UpdateHitState(); break;
                case EPlayerState.Down: UpdateDownState(); break;
                case EPlayerState.DownLand: UpdateDownLandState(); break;
                case EPlayerState.GetUp: UpdateGetUpState(); break;
            }

            yield return null;
        }

        coPlayerStateController = null;
    }

    private void NaturalRecovery()
    {
        RecoveryHp(10, false);
        RecoveryMp(5, false);
    }
    
    private void RecoveryHp(float hp, bool isNotifyUI = true)
    {
        playerData.CurrHp += hp;

        if(isNotifyUI)
            Managers.UI.SpawnObjectUI<UI_Heal>(EUIObjectType.UI_Heal, new UIHealParam((int)hp, true));

        if (playerData.CurrHp > playerData.MaxHp)
            playerData.CurrHp = playerData.MaxHp;

        OnChangedHp?.Invoke(playerData.CurrHp);
    }

    private void RecoveryMp(float mp, bool isNotifyUI = true)
    {
        playerData.CurrMp += mp;
        
        if(isNotifyUI)
            Managers.UI.SpawnObjectUI<UI_Heal>(EUIObjectType.UI_Heal, new UIHealParam((int)mp, false));

        if (playerData.CurrMp > playerData.MaxMp) 
            playerData.CurrMp = playerData.MaxMp;

        OnChangedMp?.Invoke(playerData.CurrMp);
    }

    #region Idle Motion
    protected virtual bool IdleStateCondition()
    {
        if (moveDirection != Vector2.zero)
            return false;

        if (CreatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void IdleStateEnter()
    {
        InitRigidVelocityX();
        isPlayerStateLock = false;
        isJumpAir = false;
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

        if (Rigid.velocity.y > 0)
            return false;

        if (CreatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void MoveStateEnter()
    {
        isJumpAir = false;
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
        SetRigidVelocityX(moveDirection.x * playerData.MoveSpeed);

        if (moveDirection.x > 0)
            LookLeft = false;
        else if (moveDirection.x < 0)
            LookLeft = true;
    }
    #endregion

    #region Jump Motion
    protected virtual bool JumpStateCondition()
    {
        if (CreatureFoot.IsLandingGround == false)
            return false;

        return true;
    }

    protected virtual void JumpStateEnter()
    {
        InitRigidVelocityY();
        SetRigidVelocityY(playerData.JumpPower);
    }

    protected virtual void UpdateJumpState()
    {
        Movement();
        FallDownCheck();
    }

    protected virtual void JumpStateExit()
    {

    }
    #endregion

    #region JumpAir Motion
    bool isJumpAir = false;
    protected virtual bool JumpAirStateCondition()
    {
        if (isJumpAir)
            return false;

        if (CreatureFoot.IsLandingGround)
            return false;

        return true;
    }

    protected virtual void JumpAirStateEnter()
    {
        InitRigidVelocityY();
        SetRigidVelocityY(playerData.JumpPower);

        isJumpAir = true;

        // 캐릭터 밑에 이펙트 추가하면 좋을 듯한데 (에셋)
    }

    protected virtual void UpdateJumpAirState()
    {
        Movement();
        FallDownCheck();

        // 착지 확인
        if (CreatureFoot.IsLandingGround)
        {
            PlayerState = EPlayerState.Move;
        }
    }

    protected virtual void JumpAirStateExit()
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
        if (CreatureFoot.IsLandingGround)
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
        if (CreatureFoot.IsLandingGround == false && Rigid.velocity.y < 0)
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
        isJumpAir = false;
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

    #region Dash Motion
    protected virtual bool DashStateCondition()
    {
        if (PlayerSkillDict[EPlayerSkillType.Dash].isAvailable == false)
        {
            // 재사용 대기시간입니다.
            return false;
        }

        return true;
    }

    protected virtual void DashStateEnter()
    {
        SetRigidVelocityX(playerData.DashSpeed * ((LookLeft) ? -1 : 1));
        isPlayerStateLock = true;
        IsInvincibility = true;

        PlayerSkillDict[EPlayerSkillType.Dash].isAvailable = false;
        OnUseSkill(EPlayerSkillType.Dash, PlayerSkillDict[EPlayerSkillType.Dash].coolTime);
    }

    protected virtual void UpdateDashState()
    {
        if (IsEndCurrentState(EPlayerState.Dash))
        {
            isPlayerStateLock = false;
            PlayerState = EPlayerState.Fall;
            PlayerState = EPlayerState.Move;
            PlayerState = EPlayerState.Idle;
        }
    }

    protected virtual void DashStateExit()
    {
        IsInvincibility = false;
    }
    #endregion

    #region Attack Motion

    public void OnAttackTarget(IHitEvent attackTarget)
    {
        if (PlayerState == EPlayerState.Attack)
        {
            attackTarget.OnHit(new AttackParam(this, LookLeft, playerData.StrikingPower));
        }
    }
    protected virtual bool AttackStateCondition()
    {
        if (CreatureFoot.IsLandingGround == false)
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
    [SerializeField, ReadOnly] int skillCount = 0;
    public void OnSkillAttackTarget(IHitEvent skillAttackTarget)
    {
        if (PlayerState == EPlayerState.Skill1 || PlayerState == EPlayerState.Skill2 
            || PlayerState == EPlayerState.Skill3 || PlayerState == EPlayerState.Skill4)
        {
            if(PlayerSkillDict.TryGetValue((EPlayerSkillType)skillNum, out PlayerSkill d)
                && d is PlayerAttackSkill data)
            {
                float damageRatio = data.damageRatioList[skillCount];
                skillAttackTarget.OnHit(new AttackParam(this, LookLeft, playerData.StrikingPower * damageRatio));
            }
            else
            {
                Debug.LogWarning($"스킬 데이터가 없습니다 : {skillNum} 번");
                return;
            }
        }
    }

    public void OnReadyToSkill(EPlayerSkillType skillType)
    {
        if (PlayerSkillDict.TryGetValue(skillType, out PlayerSkill skillData))
            skillData.isAvailable = true;
    }

    protected virtual bool SkillStateCondition()
    {
        if (skillNum == 0)
            return false;

        if (CreatureFoot.IsLandingGround == false)
            return false;

        if (PlayerSkillDict.TryGetValue((EPlayerSkillType)skillNum, out PlayerSkill skillData))
        {
            if (skillData.isAvailable == false)
            {
                Managers.UI.SpawnObjectUI<UI_TextObject>(EUIObjectType.UI_TextObject,
                    new UITextParam("스킬이 준비되지 않았습니다."));
                return false;
            }

            if (skillData.mpAmount > playerData.CurrMp)
            {
                Managers.UI.SpawnObjectUI<UI_TextObject>(EUIObjectType.UI_TextObject,
                    new UITextParam("마나가 부족합니다."));
                return false;
            }
        }
        else
            return false;

        return true;
    }

    protected virtual void SkillStateEnter()
    {
        if(skillNum == 4)
            playerCameraController.SetEnabled(true);

        EPlayerSkillType type = (EPlayerSkillType)skillNum;
        PlayerSkill skillData = PlayerSkillDict[type];
        skillData.isAvailable = false;

        playerData.CurrMp -= skillData.mpAmount;
        OnChangedMp?.Invoke(playerData.CurrMp);
        OnUseSkill?.Invoke(type, skillData.coolTime);

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
        playerCameraController.SetEnabled(false);
        OnDeactiveSkillAttackObject();
        skillNum = 0;
        skillCount = 0;
        isSuperArmour = false;
        IsInvincibility = false;
    }
    #endregion

    #region Guard Motion
    protected virtual bool GuardStateCondition()
    {
        if (CreatureFoot.IsLandingGround == false)
            return false;

        if (PlayerSkillDict[EPlayerSkillType.Guard].isAvailable == false)
        {
            // 재사용 대기시간입니다.
            return false;
        }

        return true;
    }
    
    protected virtual void GuardStateEnter()
    {
        InitRigidVelocityX();

        PlayerSkillDict[EPlayerSkillType.Guard].isAvailable = false;
        OnUseSkill(EPlayerSkillType.Guard, PlayerSkillDict[EPlayerSkillType.Guard].coolTime);
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
        RecoveryMp(100);
        isPlayerStateLock = true;
        SetRigidVelocityX(2f * ((LookLeft) ? 1 : -1));
        Managers.Game.GameTimeScaleSlowEffect(0.5f, 0.5f);
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

    }
    #endregion

    #region Hit Motion
    Vector3 hitForceVec = Vector3.zero;
    [SerializeField, ReadOnly] bool isSuperArmour = false;

    [SerializeField, ReadOnly] bool _isInvincibility;
    bool IsInvincibility
    {
        get { return _isInvincibility; }
        set
        {
            if (value == _isInvincibility)
                return;

            _isInvincibility = value;

            if (_isInvincibility)
                Rigid.excludeLayers += 1 << (int)ELayer.Monster;
            else
                Rigid.excludeLayers -= 1 << (int)ELayer.Monster;
        }
    }

    private void OnDamaged(float damage)
    {
        playerData.CurrHp -= damage;

        if (playerData.CurrHp < 0)
        {
            isDead = true;
            playerData.CurrHp = 0;
        }

        OnChangedHp?.Invoke(playerData.CurrHp);
    }

    public void OnHit(AttackParam param = null)
    {
        if (IsInvincibility || param == null)
            return;

        if (coHitDelayCheck != null)
            return;

        coHitDelayCheck = StartCoroutine(CoHitDelayCheck(0.3f));

        UIDamageParam damageParam;
        Vector3 subVec = new Vector3(0, Collider.size.y * 0.7f, 0);

        if (PlayerState == EPlayerState.Guard && param.isAttackerLeft == !LookLeft)
        {
            subVec.x += Collider.size.x * ((LookLeft) ? -1 : 1) * 2;
            Managers.Object.SpawnEffectObject(EEffectObjectType.PlayerHitBlockEffect, this.transform.position + subVec);
            isPlayerStateLock = false;

            damageParam = new((int)param.damage / 10
            , transform.position + (Collider.size.y * Vector3.up * 1.2f));
            Managers.UI.SpawnObjectUI<UI_Damage>(EUIObjectType.UI_Damage, damageParam);
            OnDamaged(damageParam.damage);

            PlayerState = EPlayerState.Block;
            return;
        }

        damageParam = new((int)param.damage
            , transform.position + (Collider.size.y * Vector3.up * 1.2f));
        Managers.UI.SpawnObjectUI<UI_Damage>(EUIObjectType.UI_Damage, damageParam);
        OnDamaged(damageParam.damage);

        if (isSuperArmour)
        {
            subVec.x += Collider.size.x * ((LookLeft) ? 1 : -1) * 2;
            subVec.y += UnityEngine.Random.Range(-0.5f, 0.5f);
            Managers.Object.SpawnEffectObject(EEffectObjectType.PlayerHitEffect, this.transform.position + subVec);
            return;
        }

        LookLeft = !param.isAttackerLeft;
        hitForceVec.x = param.pushPower * ((param.isAttackerLeft) ? -1 : 1);
        Managers.Object.SpawnEffectObject(EEffectObjectType.PlayerHitEffect, this.transform.position + subVec);
        isPlayerStateLock = false;

        if(CreatureFoot.IsLandingGround == false)
        {
            hitForceVec.y = param.pushPower;
            PlayerState = EPlayerState.Down;
        }
        else if(playerData.CurrHp <= 0)
        {
            PlayerState = EPlayerState.Dead;
        }
        else
        {
            PlayerState = EPlayerState.Hit;
        }
    }

    Coroutine coHitDelayCheck;
    private IEnumerator CoHitDelayCheck(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        coHitDelayCheck = null;
    }

    protected virtual bool HitStateCondition()
    {
        return true;
    }

    protected virtual void HitStateEnter()
    {
        isPlayerStateLock = true;
        InitRigidVelocityY();

        if (hitForceVec != Vector3.zero)
        {
            SetRigidVelocity(hitForceVec);
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
        hitForceVec = Vector3.zero;
    }
    #endregion

    #region Down Motion
    protected virtual bool DownStateCondition()
    {
        return true;
    }

    protected virtual void DownStateEnter()
    {
        isPlayerStateLock = true;
        InitRigidVelocityY();

        if (hitForceVec != Vector3.zero)
        {
            SetRigidVelocity(hitForceVec);
        }
    }

    protected virtual void UpdateDownState()
    {
        if (CreatureFoot.IsLandingGround)
        {
            isPlayerStateLock = false;
            if (playerData.CurrHp <= 0)
                PlayerState = EPlayerState.Dead;
            else
                PlayerState = EPlayerState.DownLand;
        }
    }

    protected virtual void DownStateExit()
    {
        hitForceVec = Vector3.zero;
        isPlayerStateLock = true;
    }
    #endregion

    #region DownLand Motion
    protected virtual bool DownLandStateCondition()
    {
        return true;
    }

    protected virtual void DownLandStateEnter()
    {
        isPlayerStateLock = true;
        IsInvincibility = true;
        InitRigidVelocityX();
    }

    protected virtual void UpdateDownLandState()
    {
       if(IsEndCurrentState(EPlayerState.DownLand))
       {
            isPlayerStateLock = false;
            PlayerState = EPlayerState.GetUp;
        }
    }

    protected virtual void DownLandStateExit()
    {
        isPlayerStateLock = true;
    }
    #endregion

    #region GetUp Motion
    protected virtual bool GetUpStateCondition()
    {
        return true;
    }

    protected virtual void GetUpStateEnter()
    {
        isPlayerStateLock = true;
    }

    protected virtual void UpdateGetUpState()
    {
        if (IsEndCurrentState(EPlayerState.GetUp))
        {
            isPlayerStateLock = false;
            PlayerState = EPlayerState.Move;
            PlayerState = EPlayerState.Idle;
        }
    }

    protected virtual void GetUpStateExit()
    {
        if (coDelayInvincibilityTime == null)
            coDelayInvincibilityTime = StartCoroutine(CoDelayInvincibilityTime(0.3f));
        else
            IsInvincibility = false;
    }

    Coroutine coDelayInvincibilityTime = null;
    private IEnumerator CoDelayInvincibilityTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        IsInvincibility = false;
        coDelayInvincibilityTime = null;
    }
    #endregion

    #region Dead Motion
    bool isDead = false;
    protected virtual bool DeadStateCondition()
    {
        if (playerData.CurrHp > 0)
            return false;

        return true;
    }

    protected virtual void DeadStateEnter()
    {
        IsInvincibility = true;
        InitRigidVelocityX();
        Managers.Game.ClearFailedStage();
    }
    #endregion

    #endregion

    #region Animation
    protected void PlayAnimation(EPlayerState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString());
    }

    protected bool IsState(AnimatorStateInfo stateInfo, EPlayerState state)
    {
        return stateInfo.IsName(state.ToString());
    }

    public bool IsState(EPlayerState state)
    {
        if (animator == null)
            return false;

        return IsState(animator.GetCurrentAnimatorStateInfo(0), state);
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
    #endregion

    #region Animation Clip Event
    public void OnInitHitForce()
    {
        if (CreatureFoot.IsLandingGround)
            InitRigidVelocityX();
    }

    public void OnActiveAttackObject()
    {
        attackObject.SetActive(true);
    }

    public void OnDeactiveAttackobject()
    {
        attackObject.SetActive(false);
    }

    public void OnActiveSkillAttackObject()
    {
        skillAttackObject.SetActiveCollider(true);
    }

    public void OnDeactiveSkillAttackObject()
    {
        skillAttackObject.SetActiveCollider(false);
    }

    public void OnAddSkillCount()
    {
        skillCount++;
    }
    #endregion
}
