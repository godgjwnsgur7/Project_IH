using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMap : BaseMap
{
    public override void LoadMap()
    {
        base.LoadMap();
        // 추가적인 로드 로직 (예: 적 스폰)
    }

    public override void UnloadMap()
    {
        base.UnloadMap();
        // 추가적인 언로드 로직
    }
}