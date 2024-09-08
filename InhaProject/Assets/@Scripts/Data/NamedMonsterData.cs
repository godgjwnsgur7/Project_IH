using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JNamedMonsterData
    {
        public int DataId;
        public float MaxHp;
        public float StrikingPower;
        public float MoveSpeed;

    }

    public class NamedMonsterDataLoader : ILoader<int, JNamedMonsterData>
    {
        public List<JNamedMonsterData> Monsters = new List<JNamedMonsterData>();

        public Dictionary<int, JNamedMonsterData> MakeDict()
        {
            Dictionary<int, JNamedMonsterData> monsterDict = new Dictionary<int, JNamedMonsterData>();
            foreach (JNamedMonsterData monster in Monsters)
                monsterDict.Add(monster.DataId, monster);
            return monsterDict;
        }
    }
}
