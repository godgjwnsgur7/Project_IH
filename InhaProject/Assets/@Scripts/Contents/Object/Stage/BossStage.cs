using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossStage : BaseStage
{
    [field: SerializeField, ReadOnly] private List<BossGimmickPoint> gimmickPointList = new List<BossGimmickPoint>();
    [field: SerializeField, ReadOnly] private List<MonsterSpawnPoint> monsterSpawnPoints = new List<MonsterSpawnPoint>();

    protected override void Reset()
    {
        base.Reset();

        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.TryGetComponent<BossGimmickPoint>(out BossGimmickPoint gimmickPoint))
                gimmickPointList.Add(gimmickPoint);
            else if(child.TryGetComponent<MonsterSpawnPoint>(out MonsterSpawnPoint monsterSpawnPoint))
                monsterSpawnPoints.Add(monsterSpawnPoint);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.BossStage;

        return true;
    }

    public List<BossGimmickPoint> GetGimmickPointList() => gimmickPointList;

}
