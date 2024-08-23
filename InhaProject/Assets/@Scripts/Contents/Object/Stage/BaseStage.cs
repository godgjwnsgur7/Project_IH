using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStage : BaseObject
{
    public string stageName;

    public List<BaseMap> maps = new List<BaseMap>();
    public int currentMapIndex { get; set; } = 0;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        foreach (BaseMap map in maps)
        {
            map.Init(); // 각 맵을 초기화
        }
        /*

        NormalMap normalMapInstance = Instantiate(normalMapPrefab);
        BossMap bossMapInstance = Instantiate(bossMapPrefab);


        maps.Add(normalMapInstance);
        maps.Add(bossMapInstance);
        약간 이런식으로 동적으로 추가할까 흠...
         */

        return true;
    }

    public void StartStage()
    {
        if (!_init)
        {
            return;
        }

        if (maps.Count > 0)
        {
            LoadCurrentMap();
        }
    }
    public void NextMap()
    {
        // 현재 맵 인덱스가 맵 리스트의 개수보다 작은지 확인
        if (currentMapIndex < maps.Count)
        {
            // 현재 맵을 언로드 (비활성화 및 상태 저장 등)
            maps[currentMapIndex].UnloadMap();
            currentMapIndex++; // 다음 맵으로 인덱스 증가

            // 다음 맵이 존재하는지 확인
            if (currentMapIndex < maps.Count)
            {
                // 다음 맵을 로드 (활성화 및 상태 복구 등)
                LoadCurrentMap();
            }
            else
            {
                // 모든 맵을 완료한 경우 스테이지 완료 처리
                OnStageCompleted(); // 스테이지 완료 시 수행할 추가 로직
            }
        }
    }

    protected virtual void OnStageCompleted()
    {
        // 스테이지가 완료될 때 수행할 로직을 정의하는 가상 메서드
    }

    private void LoadCurrentMap()
    {
        // 현재 맵 인덱스에 해당하는 맵을 로드 (활성화 및 상태 복구 등)
        maps[currentMapIndex].LoadMap();
    }
}