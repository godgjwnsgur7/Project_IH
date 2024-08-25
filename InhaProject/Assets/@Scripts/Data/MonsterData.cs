using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class MonsterData
    {
        public int DataId;
        public float MoveSpeed;
        public float ChaseSpeed;
        public float DetectDistance;
        public float ChaseDistance;
        public float AttackDistance;
    }

    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> Monsters = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> monsterDict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in Monsters)
                monsterDict.Add(monster.DataId, monster);
            return monsterDict;
        }
    }
}
