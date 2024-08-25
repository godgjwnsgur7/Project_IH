using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JPlayerData
    {
        public int DataId;
        public float MaxHp;
        public float MaxMp;
        public float StrikingPower;
        public float MoveSpeed;
        public float JumpPower;
    }

    public class PlayerDataLoader : ILoader<int, JPlayerData>
    {
        public List<JPlayerData> Players = new List<JPlayerData>();

        public Dictionary<int, JPlayerData> MakeDict()
        {
            Dictionary<int, JPlayerData> playerDict = new Dictionary<int, JPlayerData>();
            foreach (JPlayerData player in Players)
                playerDict.Add(player.DataId, player);
            return playerDict;
        }
    }
}