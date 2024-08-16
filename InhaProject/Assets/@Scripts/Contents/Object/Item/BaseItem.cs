using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class BaseItem : BaseObject
{
    [SerializeField]
    private EItemState _itemState = EItemState.None;

    protected ObjectMgr objectMgr; // ObjectMgr를 참조할 변수
    // 아이템 상태를 설정하는 프로퍼티
    public virtual EItemState ItemState
    {
        get { return _itemState; }
        protected set
        {
            if (_itemState == EItemState.Used) // 아이템이 이미 사용된 상태인 경우 변경하지 않음
                return;

            if (_itemState == value) // 현재 상태와 동일하면 변경하지 않음
                return;

            bool isChangeState = true;
            switch (value)
            {
                case EItemState.Use:
                    isChangeState = UseStateCondition();
                    break;
                case EItemState.Used:
                    isChangeState = UsedStateCondition();
                    break;
            }

            if (!isChangeState) // 상태 변경 조건이 충족되지 않으면 아무것도 하지 않음
                return;

            switch (_itemState)
            {
                case EItemState.Use:
                    UseStateExit();
                    break;
                case EItemState.Used:
                    UsedStateExit();
                    break;
            }

            _itemState = value; // 상태 업데이트

            switch (value)
            {
                case EItemState.Use:
                    UseStateEnter();
                    break;
                case EItemState.Used:
                    UsedStateEnter();
                    break;
            }
        }
    }

    public EItemType ItemType { get; protected set; } // 아이템 타입

    private Animator animator; // 애니메이터 컴포넌트
    
    protected virtual void Start()
    {
        SetInfo(); // 초기화용 또는 불필요한 경우 제거 가능
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
                                             // 필요한 경우 다른 컴포넌트 초기화

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);
        // templateID에 따라 아이템 타입 및 상태 설정
        ItemType = (EItemType)templateID;
        ItemState = EItemState.Standby; // 기본 상태
    }

    #region 상태 조건
    protected virtual bool UseStateCondition() { return false; }
    protected virtual bool UsedStateCondition() { return true; }
    #endregion

    #region 상태 진입
    protected virtual void UseStateEnter() { }
    protected virtual void UsedStateEnter() { }
    #endregion

    #region 상태 종료
    protected virtual void UseStateExit() { }
    protected virtual void UsedStateExit() { }
    #endregion

    #region 애니메이션
    protected void PlayAnimation(EItemState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString()); // 상태에 맞는 애니메이션 재생
    }
    #endregion

    protected virtual void DestroyItem()
    {
        Debug.Log("Destroy");
        ItemState = EItemState.Used;
        // 자신을 파괴
        Managers.Object.DespawnObject(gameObject);
        
    }

    protected virtual void SpawnItems(EItemType spawnItemType)
    {
        Vector3 spawnPosition = transform.position; // 현재 아이템 위치에 생성
        Quaternion spawnRotation = Quaternion.identity;
        Managers.Object.SpawnObject(spawnItemType, spawnPosition,spawnRotation);

    }
}


