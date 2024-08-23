using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStage : BaseObject
{
    public string stageName;

    public List<BaseMap> maps = new List<BaseMap>();
    public int currentMapIndex { get; set; } = 0;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        foreach (BaseMap map in maps)
        {
            map.Init(); // �� ���� �ʱ�ȭ
        }
        /*

        NormalMap normalMapInstance = Instantiate(normalMapPrefab);
        BossMap bossMapInstance = Instantiate(bossMapPrefab);


        maps.Add(normalMapInstance);
        maps.Add(bossMapInstance);
        �ణ �̷������� �������� �߰��ұ� ��...
         */

        return true;
    }

    public void StartStage()
    {
        if (!_init)
        {
            return;
        }

        if (maps.Count > 0)
        {
            LoadCurrentMap();
        }
    }
    public void NextMap()
    {
        // ���� �� �ε����� �� ����Ʈ�� �������� ������ Ȯ��
        if (currentMapIndex < maps.Count)
        {
            // ���� ���� ��ε� (��Ȱ��ȭ �� ���� ���� ��)
            maps[currentMapIndex].UnloadMap();
            currentMapIndex++; // ���� ������ �ε��� ����

            // ���� ���� �����ϴ��� Ȯ��
            if (currentMapIndex < maps.Count)
            {
                // ���� ���� �ε� (Ȱ��ȭ �� ���� ���� ��)
                LoadCurrentMap();
            }
            else
            {
                // ��� ���� �Ϸ��� ��� �������� �Ϸ� ó��
                OnStageCompleted(); // �������� �Ϸ� �� ������ �߰� ����
            }
        }
    }

    protected virtual void OnStageCompleted()
    {
        // ���������� �Ϸ�� �� ������ ������ �����ϴ� ���� �޼���
    }

    private void LoadCurrentMap()
    {
        // ���� �� �ε����� �ش��ϴ� ���� �ε� (Ȱ��ȭ �� ���� ���� ��)
        maps[currentMapIndex].LoadMap();
    }
}