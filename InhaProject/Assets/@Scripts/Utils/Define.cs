using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    /// <summary>
    /// 씬 이름과 같아야 함
    /// </summary>
    public enum EScene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
    }
    public enum ELayer
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,

        Water = 4,
        UI = 5,
    }

    public enum ETag
    {
        Untagged,
        
        MainCamera,
        Player,
        GameController,

        Ground,
        Interaction,
    }

    public enum ECreatureType
    {
        Player,
        Enemy,
    }

    /// <summary>
    /// 애니메이션 클립 이름과 같아야 함
    /// </summary>
    public enum ECreatureState
    {
        None,
        Idle,
        Walk, // 일단 미사용
        Move, // Run으로 일단 사용
        Jump,
        Fall,
        Land,
        Attack,

        Dead
    }
}