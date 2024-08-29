using UnityEngine;
using System.Collections.Generic;

public class BaseStage : BaseObject
{
    public string stageName; // �������� �̸�
    public List<BaseMap> maps; // �� ���������� ��� �� ����Ʈ
    private int currentMapIndex;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        currentMapIndex = 0;
        foreach (var map in maps)
        {
            map.Init();
        }

        return true;
    }

    public void OnMapCleared(BaseMap clearedMap)
    {
        currentMapIndex++;

        if (currentMapIndex >= maps.Count)
        {
            Managers.Game.OnStageCleared(this); // GameMgr�� �������� Ŭ���� �˸�
        }
        else
        {
            Managers.Game.LoadNextMap(maps[currentMapIndex]); // ���� �� �ε�
        }
    }
}
