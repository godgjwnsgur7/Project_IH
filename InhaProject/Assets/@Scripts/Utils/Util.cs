using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static void SortingDictValue<T>(Dictionary<int, T> dict)
    {
        // Dict<Key, Value> -> List[index](Key, Value)
        List<(int, T)> list = new List<(int, T)>();
        foreach (KeyValuePair<int, T> kvp in dict)
            list.Add((kvp.Key, kvp.Value));

        // 정렬
        list.Sort((x, y) => x.Item1 > y.Item1 ? 1 : -1);

        // Reset Dict
        dict.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            int key = i + 1;
            T data = list[i].Item2;
            dict.Add(key, data);
        }
    }

#if UNITY_EDITOR
    public static GameObject Editor_InstantiateObject(Transform transform)
    {
        GameObject tempObject = new GameObject();
        GameObject go = GameObject.Instantiate(tempObject, transform);
        GameObject.DestroyImmediate(tempObject);
        return go;
    }

    public static void Editor_FileDelete(string path, string fileExtension = ".json")
    {
        if (fileExtension[0] != '.')
            fileExtension = $".{fileExtension}";

        if (File.Exists($"{path}{fileExtension}"))
        {
            File.Delete($"{path}{fileExtension}");
            File.Delete($"{path}.meta");
        }
    }
#endif
}