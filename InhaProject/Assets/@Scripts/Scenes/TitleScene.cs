using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.TitleScene;

        return true;
    }

    private void Start()
    {
        Managers.Sound.PlayBgm(EBgmSoundType.BossMap);
    }

    public override void Clear()
    {
        
    }
}