using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMap : BaseMap
{
    public override void LoadMap()
    {
        base.LoadMap();
        // 보스 맵 특화 로직 (예: 보스 스폰)
    }

    public override void UnloadMap()
    {
        base.UnloadMap();
        // 보스 맵 특화 로직 (예: 보스 제거)
    }
}