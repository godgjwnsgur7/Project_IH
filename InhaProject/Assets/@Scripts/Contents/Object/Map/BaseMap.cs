using Unity.VisualScripting;
using UnityEngine;

public class BaseMap : BaseObject
{
    public string mapName; // �� �̸�
    public bool isCleared; // ���� Ŭ����Ǿ����� ����

    public void ClearMap()
    {
        if (!isCleared)
        {
            isCleared = true;
            Managers.Game.OnMapCleared(this); // GameMgr�� Ŭ����� ���� ����
        }
    }

    // �� �ʱ�ȭ
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        isCleared = false;
        // �� �ʱ�ȭ ���� �߰�

        return true;
    }
}
