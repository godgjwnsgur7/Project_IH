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
}
