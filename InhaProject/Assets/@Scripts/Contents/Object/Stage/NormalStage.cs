using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStage : BaseStage
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.BossStage;

        return true;
    }
}