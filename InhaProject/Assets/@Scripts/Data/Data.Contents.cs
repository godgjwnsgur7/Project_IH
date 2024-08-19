using System.Collections.Generic;
using System;

namespace Data
{
    #region Player
    [Serializable]
    public class PlayerData
    {
        public int DataId;
        public float MoveSpeed;
        public float JumpPower;
    }

    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        public List<PlayerData> Players = new List<PlayerData>();

        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> playerDict = new Dictionary<int, PlayerData>();
            foreach (PlayerData player in Players)
                playerDict.Add(player.DataId, player);
            return playerDict;
        }
    }
    #endregion

    #region Monster
    [Serializable]
    public class MonsterData
    {
        public int DataId;
        public float MoveSpeed;
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
    #endregion
}
