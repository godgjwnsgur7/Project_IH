using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이펙트가 무언가를 소환한다는 게 찜찜하지만, 일단 고. (임시)
public class MonsterSpawnEffect : BaseEffectObject
{
    ENormalMonsterType monsterType = ENormalMonsterType.Max;
    float subPosY = 2.5f;

    protected override void OnDisable()
    {
        base.OnDisable();

        monsterType = ENormalMonsterType.Max;
    }

    public override bool Init()
    {
        if (base.Init())
            return false;

        return true;
    }

    public override void SetInfo(EffectParam param = null)
    {
        base.SetInfo(param);

        transform.position += Vector3.down * subPosY;

        if(param is SpawnMonsterEffectParam spawnParam)
        {
            monsterType = spawnParam.spawnType;
        }

        if (coSpawnMonster != null)
            StopCoroutine(coSpawnMonster);

        coSpawnMonster = CoroutineHelper.StartCoroutine(CoSpawnMonster());
    }

    Coroutine coSpawnMonster = null;
    private IEnumerator CoSpawnMonster(float waitTime = 0.5f)
    {
        yield return new WaitForSeconds(waitTime);

        if (monsterType == ENormalMonsterType.Max)
            monsterType = (ENormalMonsterType)Random.Range(0, (int)ENormalMonsterType.Max);

        Vector3 spawnPosVec = this.transform.position + (Vector3.up * subPosY);
        spawnPosVec.z = 0;
        
        Managers.Object.SpawnNormalMonster(monsterType, spawnPosVec);
        coSpawnMonster = null;
    }
}
