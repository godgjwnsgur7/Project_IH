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

        // �� �ҷ����� Init

        // ĳ���� ��ȯ�ϰ� Init
        GameObject go = Managers.Resource.Instantiate(PrefabPath.OBJECT_PLAYER_PATH + $"/{Define.EPlayerType.FemaleCharacter}");
        
        // ī�޶� ĳ���Ϳ� �ٿ��ֱ�

        return true;
    }

    public override void Clear()
    {

    }
}
