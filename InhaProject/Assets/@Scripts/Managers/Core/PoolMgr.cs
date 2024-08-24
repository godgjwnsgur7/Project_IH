using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface Poolable
{
    public bool IsUsing { get; set; }
    public GameObject GameObject { get; }
}

public class PoolMgr
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }
        Stack<Poolable> poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int poolCount)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < poolCount; i++)
                Push(Create());
        }

        public void Add(GameObject original, int poolCount)
        {
            if (Original != original)
            {
                Debug.Log($"Failed to Add : {original}");
                return;
            }

            for (int i = 0; i < poolCount; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.GameObject.transform.parent = Root;
            poolable.GameObject.gameObject.SetActive(false);
            poolable.IsUsing = false;
            
            poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.GameObject.gameObject.SetActive(true);
            poolable.GameObject.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }

        public Poolable Pop(bool active = false)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            if (active)
                poolable.GameObject.SetActive(true);

            poolable.IsUsing = true;

            return poolable;
        }

        public Poolable Pop(Vector3 position, bool active = false)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.GameObject.SetActive(true);
            poolable.GameObject.transform.position = position;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    Transform root;

    int poolCount = 5;

    public void Init()
    {
        if (root == null)
        {
            root = new GameObject { name = "@Pool_Root" }.transform;
        }
    }

    public void CreatePool(GameObject original, int count)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = root;

        pools.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.GameObject.name;

        if (pools.ContainsKey(name) == false) // 이 경우엔 파괴
        {
            GameObject.Destroy(poolable.GameObject);
            return;
        }

        pools[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, bool active)
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, poolCount);

        return pools[original.name].Pop(active);
    }

    public Poolable Pop(GameObject original, Transform parent)
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, poolCount); 

        return pools[original.name].Pop(parent);
    }

    public Poolable Pop(GameObject original, Vector3 position = default(Vector3))
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, poolCount); 

        return pools[original.name].Pop(position);
    }

    public void GeneratePool(GameObject original, int count, Transform parent = null)
    {
        if (pools.ContainsKey(original.name) == true)
        {
            pools.TryGetValue(original.name, out var _pool);
            _pool.Add(original, count);
            return;
        }

        CreatePool(original, count);
    }

    public GameObject GetOriginal(string name)
    {
        if (pools.ContainsKey(name) == false)
            return null;

        return pools[name].Original;
    }

    public void Clear()
    {
        if (root != null)
        {
            foreach (Transform child in root)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        pools.Clear();
    }
}