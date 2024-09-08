using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParam { }

public class SpawnMonsterEffectParam : EffectParam
{
    public ENormalMonsterType spawnType;

    public SpawnMonsterEffectParam(ENormalMonsterType spawnType)
    {
        this.spawnType = spawnType;
    }
}