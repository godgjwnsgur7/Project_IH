using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMap : BaseMap
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        isCleared = false;
        // 맵 초기화 로직 추가

        return true;
    }
}