using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;

        GameObject go = Managers.Resource.Instantiate(PrefabPath.OBJECT_PLAYER_PATH + $"/{EPlayerType.Player.ToString()}");
        go.transform.position = new Vector3(19, 3, 0);
        Camera.main.GetComponent<CameraController>().SetTarget(go.GetComponent<Player>());
        return true;
    }

    private void Start()
    {
        // TestSpawnMonster();
    }

    // 스폰 몬스터 테스트 코드
    private void TestSpawnMonster()
    {
        Managers.Object.SpawnEffectObject(EEffectObjectType.MonsterSpawnEffect, new Vector3(19, 2.5f, 0),
            new SpawnMonsterEffectParam(ENormalMonsterType.SkeletonWarrior));
        Managers.Object.SpawnEffectObject(EEffectObjectType.MonsterSpawnEffect, new Vector3(-2.5f, 5.5f, 0),
            new SpawnMonsterEffectParam(ENormalMonsterType.SkeletonWizard));
    }

    public override void Clear()
    {

    }
}
