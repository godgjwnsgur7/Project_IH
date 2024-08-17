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

    /// <summary>
    /// 유니티 에디터의 레이어와 같아야 함
    /// </summary>
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

    /// <summary>
    /// 유니티 에디터에 태그가 있어야 함
    /// </summary>
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

    public enum EItemState
    {
        None,
        Standby,
        Use,
        Used
    }

    // 아이템 프리펩이랑 이름 같을것
    public enum EItemType
    {
        None,
        ItemBox,
        Item2, //  아이템 정해지면 수정예정
        Item3,
        Item4,
        Item5,
        Item6,
        Item7,
        Item8,
        Item9,
        Item10,
        Max
    }
}