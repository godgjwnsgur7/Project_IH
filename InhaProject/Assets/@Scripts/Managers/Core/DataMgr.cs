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
    public Dictionary<int, JNamedMonsterData> NamedMonsterDict { get; private set; } = new Dictionary<int, JNamedMonsterData>();
    public Dictionary<int, JNormalMonsterData> NormalMonsterDict { get; private set; } = new Dictionary<int, JNormalMonsterData>();

    public Dictionary<int, JPlayerData> PlayerDict { get; private set; } = new Dictionary<int, JPlayerData>();
    public Dictionary<int, JPlayerSkillData> PlayerSkillDict { get; private set; } = new Dictionary<int, JPlayerSkillData>();

    public Dictionary<string, JSkillSlotData> SkillSlotDict { get; private set; } = new Dictionary<string, JSkillSlotData>();
    public Dictionary<string, JItemSlotData> ItemSlotDataDict { get; private set; } = new Dictionary<string, JItemSlotData>();


    public void Init()
    {
        MapDict = LoadJson<MapDataLoader, int, JMapData>("MapData").MakeDict();
        NamedMonsterDict = LoadJson<NamedMonsterDataLoader, int, JNamedMonsterData>("NamedMonsterData").MakeDict();
        NormalMonsterDict = LoadJson<NormalMonsterDataLoader, int, JNormalMonsterData>("NormalMonsterData").MakeDict();
        PlayerDict = LoadJson<PlayerDataLoader, int, JPlayerData>("PlayerData").MakeDict();
        PlayerSkillDict = LoadJson<PlayerSkillDataLoader, int, JPlayerSkillData>("PlayerSkillData").MakeDict();
        SkillSlotDict = LoadJson<SkillSlotDataLoader, string, JSkillSlotData>("SkillSlotData").MakeDict();
        ItemSlotDataDict = LoadJson<ItemSlotDataLoader, string, JItemSlotData>("ItemSlotData").MakeDict();
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