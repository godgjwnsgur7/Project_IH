using UnityEngine;
using System.Collections.Generic;

public class BaseStage : BaseObject
{
    public string stageName; // 스테이지 이름
    public List<BaseMap> maps; // 이 스테이지의 모든 맵 리스트
    private int currentMapIndex;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        currentMapIndex = 0;
        foreach (var map in maps)
        {
            map.Init();
        }

        return true;
    }

    public void OnMapCleared(BaseMap clearedMap)
    {
        currentMapIndex++;

        if (currentMapIndex >= maps.Count)
        {
            Managers.Game.OnStageCleared(this); // GameMgr에 스테이지 클리어 알림
        }
        else
        {
            Managers.Game.LoadNextMap(maps[currentMapIndex]); // 다음 맵 로드
        }
    }
}
