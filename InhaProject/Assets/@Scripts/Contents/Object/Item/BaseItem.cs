using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class BaseItem : BaseObject
{
    [SerializeField]
    private EItemState _itemState = EItemState.None;

    protected ObjectMgr objectMgr; // ObjectMgr�� ������ ����
    // ������ ���¸� �����ϴ� ������Ƽ
    public virtual EItemState ItemState
    {
        get { return _itemState; }
        protected set
        {
            if (_itemState == EItemState.Used) // �������� �̹� ���� ������ ��� �������� ����
                return;

            if (_itemState == value) // ���� ���¿� �����ϸ� �������� ����
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

            if (!isChangeState) // ���� ���� ������ �������� ������ �ƹ��͵� ���� ����
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

            _itemState = value; // ���� ������Ʈ

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

    public EItemType ItemType { get; protected set; } // ������ Ÿ��

    private Animator animator; // �ִϸ����� ������Ʈ
    
    protected virtual void Start()
    {
        SetInfo(); // �ʱ�ȭ�� �Ǵ� ���ʿ��� ��� ���� ����
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
                                             // �ʿ��� ��� �ٸ� ������Ʈ �ʱ�ȭ

        return true;
    }

    public override void SetInfo(int templateID = 0)
    {
        base.SetInfo(templateID);
        // templateID�� ���� ������ Ÿ�� �� ���� ����
        ItemType = (EItemType)templateID;
        ItemState = EItemState.Standby; // �⺻ ����
    }

    #region ���� ����
    protected virtual bool UseStateCondition() { return false; }
    protected virtual bool UsedStateCondition() { return true; }
    #endregion

    #region ���� ����
    protected virtual void UseStateEnter() { }
    protected virtual void UsedStateEnter() { }
    #endregion

    #region ���� ����
    protected virtual void UseStateExit() { }
    protected virtual void UsedStateExit() { }
    #endregion

    #region �ִϸ��̼�
    protected void PlayAnimation(EItemState state)
    {
        if (animator == null)
            return;

        animator.Play(state.ToString()); // ���¿� �´� �ִϸ��̼� ���
    }
    #endregion

    protected virtual void DestroyItem()
    {
        Debug.Log("Destroy");
        ItemState = EItemState.Used;
        // �ڽ��� �ı�
        Managers.Object.DespawnObject(gameObject);
        
    }

    protected virtual void SpawnItems(EItemType spawnItemType)
    {
        Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
        Quaternion spawnRotation = Quaternion.identity;
        Managers.Object.SpawnObject(spawnItemType, spawnPosition,spawnRotation);

    }
}


