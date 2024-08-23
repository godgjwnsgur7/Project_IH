using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static bool IsValid(this BaseObject bo)
    {
        if (bo == null || bo.isActiveAndEnabled == false)
            return false;

        Player player = bo as Player;
        if (player != null)
            return player.PlayerState != EPlayerState.Dead;

        Monster monster = bo as Monster;
        if (monster != null)
            return monster.MonsterState != EMonsterState.Dead;

        return true;
    }
}
