using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public class GameMgr 
{
    private Player _player;
    public Player Player => _player ??= GameObject.FindWithTag("Player")?.GetComponent<Player>();

    public BaseStage CurrStage { get; private set; } = null;

    int currStageId = 0;
    int maxStageId = 0;

    public void Init()
    {
        maxStageId = 0;
        DirectoryInfo di = new DirectoryInfo($"{Application.dataPath}/{AssetsPath.STAGE_PATH}");
        foreach (FileInfo file in di.GetFiles("*.prefab"))
        {
            maxStageId++;
            // ex : Stage 1.prefab => 1
            string[] strs = file.Name.Split('.')[0].Split(' '); 
            int stageNum = int.Parse(strs[strs.Length - 1]);
            if (maxStageId != stageNum)
                Debug.LogError($"���� : �������� �������� ����ֽ��ϴ� : {maxStageId}�� ��������");
        }
    }

    public void Clear()
    {

    }

    #region InGame
    private BaseStage LoadStage(int stageId)
    {
        return Managers.Resource.Instantiate($"{PrefabPath.STAGE_PATH}/Stage {stageId}").GetComponent<BaseStage>();
    }

    public void StartGame()
    {
        StartStage(1);
    }

    public void StartStage(int stageId)
    {
        currStageId = stageId;
        CurrStage = LoadStage(stageId);
        if (CurrStage == null)
        {
            Debug.LogError($"{stageId}�� ���������� �о�� �� �����ϴ�.");
            return;
        }

        Player player = Managers.Resource.Instantiate(PrefabPath.OBJECT_PLAYER_PATH + $"/{EPlayerType.Player}").GetComponent<Player>();
        player.transform.position = CurrStage.PlayerStartingPoint.position;
        Camera.main.GetComponent<CameraController>().SetTarget(player);
    }

    public void ClearStage()
    {
        if (maxStageId == currStageId)
        {
            ClearGame();
            return;
        }

        NextStage();
    }

    private void ClearGame()
    {
        // ���� Ŭ���� â ����, �κ�� ���ư���.
    }

    private void NextStage()
    {
        StartStage(currStageId + 1);
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
    #endregion

    #region OutGame

    #endregion
}