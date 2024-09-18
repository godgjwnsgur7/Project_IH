using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;
using static TMPro.Examples.TMP_ExampleScript_01;


public class ObjectMgr : MonoBehaviour
{
    #region Root
    private GameObject _monsterRoot;
    public GameObject MonsterRoot
    {
        get
        {
            if (_monsterRoot == null) _monsterRoot = GameObject.Find("@MonsterRoot");
            if (_monsterRoot == null) _monsterRoot = new GameObject { name = "@MonsterRoot" };
            return _monsterRoot;
        }
    }
    #endregion

    private Dictionary<EItemType, string> itemPrefabsDict = new Dictionary<EItemType, string>();

    private List<GameObject> activeObjects = new();

    //������Ʈ ����
    public void SpawnItemObject(EItemType itemType, Vector3 position = default, Quaternion rotation = default)
    {
        if (itemType == EItemType.None || itemType == EItemType.Max)
            return;
    
        // ������ Ÿ�Կ� �´� ������ �ε� �� �ν��Ͻ�ȭ
        GameObject obj = Managers.Resource.Instantiate(itemPrefabsDict[itemType], null);
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            activeObjects.Add(obj);  
        }
    }

    /// <summary>
    /// ����Ʈ ������Ʈ ���� - �ı�(��Ȱ��ȭ)�� ����Ʈ�� ������ �ڵ����� ����
    /// </summary>
    public BaseEffectObject SpawnEffectObject(EEffectObjectType type, Vector3 position, EffectParam param = null)
    {
        BaseEffectObject effectObject = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_EFFECTOBEJCT_PATH}/{type}", position).GetComponent<BaseEffectObject>();
        effectObject.SetInfo(param);
        return effectObject;
    }

    public void SpawnNormalMonster(ENormalMonsterType type, Vector3 position)
    {
        NormalMonster normalMonster = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_MONSTER_PATH}/{type}", position).GetComponent<NormalMonster>();
        
        if(normalMonster != null)
        {
            normalMonster.transform.parent = MonsterRoot.transform;
        }
    }

    public void Init()
    {
        SetItemPrefabsDict();
    }

    // ������Ʈ ����
    public void DespawnObject(GameObject obj)
    {
        if (obj == null) return;

        activeObjects.Remove(obj);
        obj.SetActive(false);
       // Destroy(obj);
    }

    // ���� Ȱ��ȭ�� ������Ʈ�� ����
    public List<GameObject> GetActiveObjects()
    {
        return new List<GameObject>(activeObjects);
    }

    private void SetItemPrefabsDict()
    {
        for (EItemType i = EItemType.None + 1; i < EItemType.Max; ++i)
        {
            itemPrefabsDict[i] = PrefabPath.OBJECT_ITEM_PATH + "/" + i.ToString();
        }
    }
}
