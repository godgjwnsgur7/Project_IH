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

    [field: SerializeField, ReadOnly]
    public BaseStage CurrStage { get; private set; } = null;
    
    public bool IsBossStage
    {
        get
        {
            return CurrStage.StageType == EStageType.BossStage;
        }
    }

    int currStageId = 1;
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
                Debug.LogError($"에러 : 스테이지 프리팹이 비어있습니다 : {maxStageId}번 스테이지");
        }
    }

    public void Clear()
    {

    }

    public void TestStage(int stageId)
    {
        currStageId = stageId;
        CurrStage = null;
        if (stageId == 0)
            CurrStage = Managers.Resource.Instantiate($"{PrefabPath.STAGE_PATH}/SamplePrefab/TestStage").GetComponent<BaseStage>();
        else
            CurrStage = LoadStage(stageId);

        if (CurrStage == null)
        {
            Debug.LogError($"{stageId}번 스테이지를 읽어올 수 없습니다.");
            return;
        }

        _player = Managers.Resource.Instantiate(PrefabPath.OBJECT_PLAYER_PATH + $"/{EPlayerType.Player}").GetComponent<Player>();
        Player.transform.position = CurrStage.PlayerStartingPoint.position;
        Player.SetInfo(0);

        Camera.main.GetComponent<MainCameraController>().SetTarget(_player);
    }

    #region InGame
    private BaseStage LoadStage(int stageId)
    {
        return Managers.Resource.Instantiate($"{PrefabPath.STAGE_PATH}/Stage {stageId}").GetComponent<BaseStage>();
    }

    public void StartNewGame()
    {
        currStageId = 1;
        StartContinueGame();
    }

    public void StartContinueGame()
    {
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }

    public void StartStage()
    {
        if (CurrStage != null)
            Managers.Resource.Destroy(CurrStage.gameObject);

        CurrStage = LoadStage(currStageId);
        if (CurrStage == null)
        {
            Debug.LogError($"{currStageId}번 스테이지를 읽어올 수 없습니다.");
            return;
        }

        if(_player == null)
            _player = Managers.Resource.Instantiate(PrefabPath.OBJECT_PLAYER_PATH + $"/{EPlayerType.Player}").GetComponent<Player>();

        Player.transform.position = CurrStage.PlayerStartingPoint.position;
        Player.SetInfo(0);

        Camera.main.GetComponent<MainCameraController>().SetTarget(_player);
    }

    public void ClearStage()
    {
        currStageId++;
        if (maxStageId < currStageId)
        {
            ClearGame();
            return;
        }

        NextStage();
    }

    private void NextStage()
    {
        UIFadeEffectParam param = new UIFadeEffectParam(null, StartStage, null);
        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }

    public void ClearFailedStage()
    {
        --currStageId;
        Managers.UI.OpenPopupUI<UI_GameOverPopup>();
    }

    private void ClearGame()
    {
        // Managers.UI.SpawnObjectUI<>

        Managers.UI.SpawnObjectUI<UI_ClearObject>(EUIObjectType.UI_ClearObject);
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