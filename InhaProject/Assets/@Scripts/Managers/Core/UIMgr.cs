using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr
{
    private int sortingOrder = 10;

    private Dictionary<string, UI_BasePopup> popupPool = new Dictionary<string, UI_BasePopup>();
    private Dictionary<string, UI_BaseWindow> windowPool = new Dictionary<string, UI_BaseWindow>();
    
    private Stack<UI_BasePopup> popupStack = new Stack<UI_BasePopup>();
    private UI_BaseWindow currActiveWindow = null;

    public UI_BaseScene SceneUI { private set; get; }

    #region UI Root
    private GameObject uiWindowRoot { get; set; } = null;
    private GameObject UIWindowRoot
    {
        get
        {
            if(uiWindowRoot == null) uiWindowRoot = GameObject.Find("@UI_WindowRoot");
            if (uiWindowRoot == null) uiWindowRoot = new GameObject { name = "@UI_WindowRoot" };
            return uiWindowRoot;
        }
    }

    private GameObject UIPopupRoot { get; set; } = null;
    #endregion

    public void Init()
    {
        if(UIPopupRoot == null)
            UIPopupRoot = GameObject.Find("@UI_PopupRoot");

        if (UIPopupRoot == null)
            UIPopupRoot = new GameObject { name = "@UI_PopupRoot" };

        UnityEngine.Object.DontDestroyOnLoad(UIPopupRoot);
        CacheAllPopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();

        uiWindowRoot = null;
        currActiveWindow = null;
        windowPool.Clear();
    }

    public void SetSceneUI(UI_BaseScene sceneUI)
    {
        SceneUI = sceneUI;
    }

    public void SetCanvas(GameObject go, bool sort = true, int sortingOrder = 0)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        if (canvas == null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
        }

        CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
        if (cs != null)
        {
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1920, 1080);
        }

        go.GetOrAddComponent<GraphicRaycaster>();

        if (sort)
        {
            canvas.sortingOrder = this.sortingOrder;
            this.sortingOrder++;
        }
        else
        {
            canvas.sortingOrder = sortingOrder;
        }
    }

    public T OpenWindowUI<T>(UIParam param = null) where T : UI_BaseWindow
    {
        string name = typeof(T).Name;

        if (windowPool.TryGetValue(name, out UI_BaseWindow window) == false)
        {
            GameObject go = Managers.Resource.Instantiate($"{PrefabPath.UI_WINDOW_PATH}/{name}");
            window = Util.GetOrAddComponent<T>(go);
            windowPool[name] = window;
            window.CloseWindowUI();
        }

        if (param != null)
            window.SetInfo(param);

        window.transform.SetParent(UIWindowRoot.transform);
        window.OpenWindowUI();

        if (currActiveWindow != null)
        {
            currActiveWindow.CloseWindowUI();
        }

        currActiveWindow = window;

        return window as T;
    }

    public void CloseWindowUI(UI_BaseWindow window)
    {
        currActiveWindow = null;
        window.gameObject.SetActive(false);
    }

    public void CacheAllPopupUI()
    {
        // UI_BasePopup을 상속받은 모든 클래스 타입들을 가져와 list에 담는다
        var list = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(UI_BasePopup)));

        foreach(Type type in list)
        {
            ChchePopupUI(type);
        }
    }

    private void ChchePopupUI(Type type)
    {
        string name = type.Name;

        if(popupPool.TryGetValue(name, out UI_BasePopup popup) == false)
        {
            GameObject go = Managers.Resource.Instantiate($"{PrefabPath.UI_POPUP_PATH}/{name}");
            go.SetActive(false);
            popup = go.GetOrAddComponent<UI_BasePopup>();
            popup.transform.SetParent(UIPopupRoot.transform);
            popupPool[name] = popup;
        }
    }

    public T OpenPopupUI<T>(UIParam param = null) where T : UI_BasePopup
    {
        string name = typeof(T).Name;

        if (popupPool.TryGetValue(name, out UI_BasePopup popup) == false)
        {
            GameObject go = Managers.Resource.Instantiate($"{PrefabPath.UI_POPUP_PATH}/{name}");
            popup = go.GetOrAddComponent<UI_BasePopup>();
            popupPool[name] = popup;
            popup.ClosePopupUI();
        }

        if(param != null)
            popup.SetInfo(param);

        popup.transform.SetParent(UIPopupRoot.transform);
        popup.OpenPopupUI();
        popupStack.Push(popup);

        return popup as T;
    }

    public void ClosePopupUI(UI_BasePopup popup)
    {
        if (popupStack.Count == 0)
        {
            Debug.Log("popupStack Count is Zero");
            return;
        }

        if (popupStack.Peek() != popup)
        {
            Debug.Log("팝업 닫기 실패 !\n" +
                $"최상위 팝업 {popupStack.Peek().name}과 닫으려는 팝업 {popup.name}이 다릅니다.");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
        {
            Debug.Log("popupStack Count is Zero");
            return;
        }

        UI_BasePopup popup = popupStack.Pop();
        popup.DeActivePopup();
        sortingOrder--;
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
            ClosePopupUI();
    }
}
