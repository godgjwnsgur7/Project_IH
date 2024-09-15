using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
#if UNITY_EDITOR
    [Header("Test 여부 체크 & 테스트 Stage 입력")]
    [SerializeField] bool isTestStage = false;
    [SerializeField] int testStageId = 0;
#endif

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;

        return true;
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (isTestStage)
        {
            Managers.Game.TestStage(testStageId);
            return;    
        }
#endif

        Managers.Game.StartStage();
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
