using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이펙트가 무언가를 소환한다는 게 찜찜하지만, 일단 고. (임시)
public class MonsterSpawnEffect : BaseEffectObject
{
    ENormalMonsterType monsterType = ENormalMonsterType.Max;

    protected override void OnDisable()
    {
        base.OnDisable();

        monsterType = ENormalMonsterType.Max;
    }

    public override bool Init()
    {
        if (base.Init())
            return false;

        SubPos = new Vector3(0, -2.5f, -0.1f);

        return true;
    }

    public override void SetInfo(EffectParam param = null)
    {
        base.SetInfo(param);

        if(param is SpawnMonsterEffectParam spawnParam)
        {
            monsterType = spawnParam.spawnType;
        }
    }

    protected override IEnumerator CoDestroySelf(float subTime = 1.5f)
    {
        yield return new WaitForSeconds(subTime);

        Vector3 spawnPosVec = this.transform.position - (Vector3.up * SubPos.y);
        spawnPosVec.z = 0;
        
        if(monsterType == ENormalMonsterType.Max)
            monsterType = (ENormalMonsterType)Random.Range(0, (int)ENormalMonsterType.Max);
        
        Managers.Object.SpawnNormalMonster(monsterType, spawnPosVec);

        yield return StartCoroutine(base.CoDestroySelf(subTime));
    }
}
