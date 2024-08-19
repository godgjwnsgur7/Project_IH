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

        GameObject go = Managers.Resource.Instantiate(PrefabPath.OBJECT_PLAYER_PATH + $"/{EPlayerType.FemaleCharacter.ToString()}");
        Camera.main.GetComponent<PlayerCamera>().SetTarget(go.GetComponent<Player>());

        return true;
    }

    public override void Clear()
    {

    }
}
