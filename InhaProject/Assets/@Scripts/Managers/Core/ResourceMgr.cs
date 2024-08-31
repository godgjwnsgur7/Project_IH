using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class ResourceMgr
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 풀링된 오브젝트일 경우 위탁
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original).GameObject;

        GameObject go = Object.Instantiate(original);
        go.name = original.name;
        return go;
    }

    public GameObject Instantiate(string path, Transform parent, Vector3 position = default)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 풀링된 오브젝트일 경우 위탁
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, position).GameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public GameObject Instantiate(string path, Vector3 position, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 풀링된 오브젝트일 경우 위탁
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, position).GameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // 풀링된 오브젝트일 경우 위탁
        if (go.TryGetComponent<Poolable>(out Poolable poolGo))
        {
            Managers.Pool.Push(poolGo);
            return;
        }

        Object.Destroy(go);
    }
}