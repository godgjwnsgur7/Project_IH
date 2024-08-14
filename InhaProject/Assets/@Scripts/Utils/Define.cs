//using System.Collections;
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
        IntroScene,
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
        Platform = 6,
        Player = 7,
        Monster = 8,
    }

    public enum ETag
    {
        Untagged,
        
        MainCamera,
        Player,
        GameController,

        Ground,
        Interaction,
        Monster,
    }

    public enum EObjectType
    {
        None,
        Creature,
        GimmickObject,

    }

    public enum ECreatureType
    {
        Player,
        Monster,
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
        JumpAir,
        Fall,
        Land,
        Attack,

        Dead
    }

    public enum EPlayerType
    {
        FemaleCharacter,
        MaleCharacter, // 아직 사용 불가능
    }

    public enum EMonsterType
    {
        Skeleton1,
        Skeleton2,
        Skeleton3,
    }
}