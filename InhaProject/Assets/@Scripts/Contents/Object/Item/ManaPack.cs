using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPack : BaseItem
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 Player 태그를 가진 경우
        {
            //heal 코드
        }
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;
        ItemType = EItemType.ManaPack;

        return true;
    }
}
