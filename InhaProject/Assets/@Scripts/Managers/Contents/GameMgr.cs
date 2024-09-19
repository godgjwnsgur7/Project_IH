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
        maxStageId = 3;
    }

    public void Clear()
    {

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
        {
            Managers.Resource.Destroy(CurrStage.gameObject);
        }

        CurrStage = LoadStage(currStageId);
        if (CurrStage == null)
        {
            Debug.LogError($"{currStageId}번 스테이지를 읽어올 수 없습니다.");
            return;
        }

        if (IsBossStage)
        {
            Managers.Sound.PlayBgm(EBgmSoundType.BossMap);
            Camera.main.GetComponent<MainCameraController>()?.SetActiveMinimapCamera(false);
        }
        else
        {
            Managers.Sound.PlayBgm(EBgmSoundType.NormalMap);
            Camera.main.GetComponent<MainCameraController>()?.SetActiveMinimapCamera(true);
        }

        if (_player == null)
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
            CoroutineHelper.StartCoroutine(CoClearGame());
            return;
        }

        NextStage();
    }

    private IEnumerator CoClearGame()
    {
        yield return new WaitForSecondsRealtime(1f);
        ClearGame();
    }

    private void NextStage()
    {
        UIFadeEffectParam param = new UIFadeEffectParam(null, StartStage, null);
        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }

    public void ClearFailedStage()
    {
        Managers.UI.OpenPopupUI<UI_GameOverPopup>().SetInfo(new UIParam());
    }

    private void ClearGame()
    {
        currStageId = 1;
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