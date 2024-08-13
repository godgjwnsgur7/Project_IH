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

        // 맵 불러오고 Init

        // 캐릭터 소환하고 Init
        GameObject go = Managers.Resource.Instantiate(PrefabPath.OBJECT_PLAYER_PATH + $"/{Define.EPlayerType.FemaleCharacter}");
        
        // 카메라 캐릭터에 붙여주기

        return true;
    }

    public override void Clear()
    {

    }
}
