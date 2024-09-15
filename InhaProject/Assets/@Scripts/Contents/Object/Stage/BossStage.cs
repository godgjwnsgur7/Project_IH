using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossStage : BaseStage
{
    [field: SerializeField, ReadOnly] private List<BossGimmickPoint> gimmickPointList = new List<BossGimmickPoint>();
    [field: SerializeField, ReadOnly] private List<MonsterSpawnPoint> monsterSpawnPointList = new List<MonsterSpawnPoint>();

    [SerializeField, ReadOnly] FixedBossMonster bossMonster;

    protected override void Reset()
    {
        base.Reset();

        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.TryGetComponent<BossGimmickPoint>(out BossGimmickPoint gimmickPoint))
                gimmickPointList.Add(gimmickPoint);
            else if(child.TryGetComponent<MonsterSpawnPoint>(out MonsterSpawnPoint monsterSpawnPoint))
                monsterSpawnPointList.Add(monsterSpawnPoint);
        }

        bossMonster = Util.FindChild<FixedBossMonster>(this.gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.BossStage;

        return true;
    }
}
