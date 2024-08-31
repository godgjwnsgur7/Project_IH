using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameMgr 
{
    public List<BaseStage> stages = new List<BaseStage>(); // ��ü �������� ����Ʈ
    private int currentStageIndex;
    private BaseStage currentStage;

    public void Init()
    {
        LoadStage(0); // ù �������� �ε�
    }

    public void Clear()
    {

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
            Debug.Log("��� �������� Ŭ����!");
            // ���� Ŭ���� ���� �߰�
        }
    }

    public void LoadNextMap(BaseMap map)
    {
        // ���� Ȱ��ȭ�ϰ�, �ʿ��� �ʱ�ȭ ���� ����
        map.gameObject.SetActive(true);
        map.Init();
    }

    public void OnMapCleared(BaseMap clearedMap)
    {
        currentStage.OnMapCleared(clearedMap); // ���� ���������� �� Ŭ���� �˸�
    }

    public void OnStageCleared(BaseStage clearedStage)
    {
        currentStageIndex++;
        LoadStage(currentStageIndex); // ���� �������� �ε�
    }

    Action onEndEffect = null;
    public void GameTimeScaleSlowEffect(float slowTime, float timeScale, Action onEndEffect = null)
    {
        if (timeScale >= 1.0f)
            return;

        if (coTimeScaleSlowEffect != null)
            return;

        if (onEndEffect != null)
            this.onEndEffect = onEndEffect;

        coTimeScaleSlowEffect = CoroutineHelper.StartCoroutine(CoTimeScaleSlowEffect(slowTime, timeScale));
    }

    public bool IsGameSlowEffect { get; private set; }
    Coroutine coTimeScaleSlowEffect;
    protected IEnumerator CoTimeScaleSlowEffect(float slowTime, float timeScale)
    {
        Time.timeScale = timeScale;
        IsGameSlowEffect = true;

        yield return new WaitForSecondsRealtime(slowTime);

        onEndEffect?.Invoke();
        Time.timeScale = 1.0f;
        IsGameSlowEffect = false;
        onEndEffect = null;
        coTimeScaleSlowEffect = null;
    }
}