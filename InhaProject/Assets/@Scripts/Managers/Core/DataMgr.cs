using Data;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();


}

public class DataMgr
{
    public Dictionary<int, JPlayerData> PlayerDict { get; private set; } = new Dictionary<int, JPlayerData>();
    public Dictionary<int, JNormalMonsterData> MonsterDict { get; private set; } = new Dictionary<int, JNormalMonsterData>();

    public void Init()
    {
        PlayerDict = LoadJson<PlayerDataLoader, int, JPlayerData>("PlayerData").MakeDict();
        MonsterDict = LoadJson<NormalMonsterDataLoader, int, JNormalMonsterData>("NormalMonsterData").MakeDict();
    }

    public Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    Loader LoadJson<Loader>(string path) where Loader : class
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}