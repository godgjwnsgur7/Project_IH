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

    private GameObject ObjectPrefab;

    
    public string[] itemPrefabsPath { get; set; }

    private List<GameObject> activeObjects = new List<GameObject>();

    //������Ʈ ����
    public GameObject SpawnObject(string path, Vector3 position, Quaternion rotation)
    {
        if (path == null)
            return null;

        // ������ Ÿ�Կ� �´� ������ �ε� �� �ν��Ͻ�ȭ
        GameObject obj = Managers.Resource.Instantiate(path, null);
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            activeObjects.Add(obj);
        }
        return obj;
    }
   

    // ������Ʈ ����
    public void DespawnObject(GameObject obj)
    {
        if (obj == null) return;

        activeObjects.Remove(obj);
        Destroy(obj);
    }

    // ���� Ȱ��ȭ�� ������Ʈ�� ����
    public List<GameObject> GetActiveObjects()
    {
        return new List<GameObject>(activeObjects);
    }

    private void LoadItemPrefabs()
    {
        itemPrefabsPath = new string[(int)EItemType.Max - 2]; // Max None  ����

        for (int i = 0; i < itemPrefabsPath.Length; i++)
        {
            string path = PrefabPath.OBJECT_ITEM_PATH + "/"+((EItemType)i + 1).ToString();
            itemPrefabsPath[i] = PrefabPath.OBJECT_ITEM_PATH + "/" + (Define.EItemType.None+i+1).ToString();
        }
    }

    private void Awake()
    {
        LoadItemPrefabs();

    }
}
