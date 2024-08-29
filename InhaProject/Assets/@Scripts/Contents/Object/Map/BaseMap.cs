using Unity.VisualScripting;
using UnityEngine;

public class BaseMap : BaseObject
{
    public string mapName; // 맵 이름
    public bool isCleared; // 맵이 클리어되었는지 여부

    public void ClearMap()
    {
        if (!isCleared)
        {
            isCleared = true;
            Managers.Game.OnMapCleared(this); // GameMgr에 클리어된 맵을 전달
        }
    }

    // 맵 초기화
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        isCleared = false;
        // 맵 초기화 로직 추가

        return true;
    }
}
