using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameMgr 
{
    public static GameMgr Instance { get; private set; }

    public List<BaseStage> stages; // 전체 스테이지 리스트
    private int currentStageIndex;
    private BaseStage currentStage;

    public void Init()
    {
         if (Instance == null)
        {
            Instance = this;

        }
        else
        {

        }

        LoadStage(0); // 첫 스테이지 로드
    }



    public void LoadStage(int stageIndex)
    {
        if (stageIndex < stages.Count)
        {
            currentStageIndex = stageIndex;
            currentStage = stages[stageIndex];
            currentStage.Init();
            LoadNextMap(currentStage.maps[0]);
        }
        else
        {
            Debug.Log("모든 스테이지 클리어!");
            // 게임 클리어 로직 추가
        }
    }

    public void LoadNextMap(BaseMap map)
    {
        // 맵을 활성화하고, 필요한 초기화 로직 수행
        map.gameObject.SetActive(true);
        map.Init();
    }

    public void OnMapCleared(BaseMap clearedMap)
    {
        currentStage.OnMapCleared(clearedMap); // 현재 스테이지에 맵 클리어 알림
    }

    public void OnStageCleared(BaseStage clearedStage)
    {
        currentStageIndex++;
        LoadStage(currentStageIndex); // 다음 스테이지 로드
    }
}
