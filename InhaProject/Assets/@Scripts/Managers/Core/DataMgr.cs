using Data;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();


}

public class DataMgr
{
    public Dictionary<int, JMapData> MapDict { get; private set; } = new Dictionary<int, JMapData>();
    public Dictionary<int, JNormalMonsterData> NormalMonsterDict { get; private set; } = new Dictionary<int, JNormalMonsterData>();

    public Dictionary<int, JPlayerData> PlayerDict { get; private set; } = new Dictionary<int, JPlayerData>();
    public Dictionary<int, JPlayerSkillData> PlayerSkillDict { get; private set; } = new Dictionary<int, JPlayerSkillData>();


    public void Init()
    {
        MapDict = LoadJson<MapDataLoader, int, JMapData>("MapData").MakeDict();
        NormalMonsterDict = LoadJson<NormalMonsterDataLoader, int, JNormalMonsterData>("NormalMonsterData").MakeDict();
        PlayerDict = LoadJson<PlayerDataLoader, int, JPlayerData>("PlayerData").MakeDict();
        PlayerSkillDict = LoadJson<PlayerSkillDataLoader, int, JPlayerSkillData>("PlayerSkillData").MakeDict();
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