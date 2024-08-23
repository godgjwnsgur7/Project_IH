using System.Collections.Generic;
using System;

namespace Data
{
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
}