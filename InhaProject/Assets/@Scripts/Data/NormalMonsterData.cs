using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JNormalMonsterData
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

    public class NormalMonsterDataLoader : ILoader<int, JNormalMonsterData>
    {
        public List<JNormalMonsterData> Monsters = new List<JNormalMonsterData>();

        public Dictionary<int, JNormalMonsterData> MakeDict()
        {
            Dictionary<int, JNormalMonsterData> monsterDict = new Dictionary<int, JNormalMonsterData>();
            foreach (JNormalMonsterData monster in Monsters)
                monsterDict.Add(monster.DataId, monster);
            return monsterDict;
        }
    }
}
