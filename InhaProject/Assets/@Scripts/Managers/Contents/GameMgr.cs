using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{

    public BaseStage currentStage;

    public void Init()
    {
        if (currentStage != null)
        {
            currentStage.Init();
            currentStage.StartStage();
        }
    }

    public void Clear()
    {
        // 게임 클리어 처리 로직

    }

    public void UseKey(EItemType keyType)
    {
        // 현재 맵에서 키를 사용할 수 있는지 확인
        if (currentStage != null &&  keyType == EItemType.Key)
        {
            // 보스 맵 클리어 문을 열 수 있는 키 사용
            Debug.Log("Boss Map");
            currentStage.NextMap();
        }
        else if (keyType == EItemType.Key)
        {
            // 일반 맵에서 중간 보스 문을 여는 키 사용
            Debug.Log("Middle Boss Door Opened!");
            currentStage.NextMap();
        }
    }

}
  