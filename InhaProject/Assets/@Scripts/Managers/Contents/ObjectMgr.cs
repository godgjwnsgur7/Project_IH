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
    [SerializeField] 
    private GameObject interactablePrefab;
    [SerializeField] 
    private GameObject triggerablePrefab;

    private List<GameObject> activeObjects = new List<GameObject>();

    // ������Ʈ ����
    public GameObject SpawnObject(EItemType type, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetPrefabByType(type);
        if (prefab == null)
            return null;

        GameObject obj = Instantiate(prefab, position, rotation);
        activeObjects.Add(obj);

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

    private GameObject GetPrefabByType(EItemType type)
    {
        switch (type)
        {
            case EItemType.Interaction:
                return interactablePrefab;
            case EItemType.Trigger:
                return triggerablePrefab;
            default:
                return null;
        }
    }
}
