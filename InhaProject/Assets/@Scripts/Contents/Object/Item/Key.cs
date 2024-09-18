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

  

}