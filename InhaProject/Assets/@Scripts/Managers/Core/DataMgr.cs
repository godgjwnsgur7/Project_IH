using Data;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();


}

public class DataMgr
{
    public Dictionary<int, Data.SampleData> SampleDict { get; private set; } = new Dictionary<int, Data.SampleData>();

    public void Init()
    {
        SampleDict = LoadJson<Data.SampleDataLoader, int, Data.SampleData>("SampleData").MakeDict();
    }

    public Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/JsonData/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    Loader LoadJson<Loader>(string path) where Loader : class
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/JsonData/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}