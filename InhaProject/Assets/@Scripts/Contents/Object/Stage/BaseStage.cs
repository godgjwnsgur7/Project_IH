using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStageType
{
    None,
    NormalStage,
    BossStage,
    Max
}

public class BaseStage : InitBase
{
    [field: SerializeField, ReadOnly] public EStageType StageType { get; protected set; } = EStageType.None;
    [field: SerializeField, ReadOnly] public Transform PlayerStartingPoint { get; protected set; } = null;

    protected virtual void Reset()
    {
        PlayerStartingPoint = Util.FindChild(this.gameObject, "PlayerStartingPoint").transform;
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }

    
}
