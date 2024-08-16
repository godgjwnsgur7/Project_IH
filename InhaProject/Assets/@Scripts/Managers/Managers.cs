using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.Serialization;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static bool Initialized { get; set; } = false;

    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    #region InGame Contents
    private GameMgr _game = new GameMgr();
    private ObjectMgr _object = new ObjectMgr();

    public static GameMgr Game { get { return Instance?._game; } }
    public static ObjectMgr Object { get { return Instance?._object; } }
    #endregion

    #region Core
    private InputMgr _input = null; // Init에서 생성
    private ResourceMgr _resource = new ResourceMgr();
    private SceneMgr _scene = new SceneMgr();
    private SoundMgr _sound = new SoundMgr();

    public static InputMgr Input { get { return Instance?._input; } }
    public static ResourceMgr Resource { get { return Instance?._resource; } }
    public static SceneMgr Scene { get { return Instance?._scene; } }
    public static SoundMgr Sound { get { return Instance?._sound; } }
    #endregion


    public static void Init()
    {
        if (s_instance == null && Initialized == false)
        {
            Initialized = true;

            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            // 초기화
            s_instance = go.GetComponent<Managers>();
            s_instance._input = Instance._resource.Instantiate(PrefabPath.INPUTMANAGER_PATH, s_instance.transform).GetComponent<InputMgr>();
            s_instance._input.Init();
            s_instance._object.Init();
        }
    }

    /// <summary>
    /// 씬 이동 시 호출
    /// </summary>
    public static void Clear()
    {
        Scene.Clear();
    }
}
