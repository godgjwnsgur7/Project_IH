using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JMonsterData
    {
        public int DataId;
        public float MaxHp;
        public float StrikingPower;
        public float MoveSpeed;
        public float ChaseSpeed;
        public float DetectDistance;
        public float ChaseDistance;
        public float AttackDistance;
    }

    public class MonsterDataLoader : ILoader<int, JMonsterData>
    {
        public List<JMonsterData> Monsters = new List<JMonsterData>();

        public Dictionary<int, JMonsterData> MakeDict()
        {
            Dictionary<int, JMonsterData> monsterDict = new Dictionary<int, JMonsterData>();
            foreach (JMonsterData monster in Monsters)
                monsterDict.Add(monster.DataId, monster);
            return monsterDict;
        }
    }
}
