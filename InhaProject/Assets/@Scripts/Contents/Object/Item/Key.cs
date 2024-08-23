using UnityEngine;

public class Key : BaseItem
{
    public override bool Init()
    {
        if (!base.Init())
            return false;

        // 키 초기화 로직
        ItemType = EItemType.Key; // 예시: 중간 보스 키
        return true;
    }

    protected override bool UseStateCondition()
    {
        // 키 사용 조건을 확인 (예: 플레이어가 문 앞에 있는지)
        return true;
    }

    protected override void UseStateEnter()
    {
        base.UseStateEnter();
        // 키 사용시 로직 (예: 문 열기)
        Managers.Game.UseKey(ItemType);    
        DestroyItem();
    }
}