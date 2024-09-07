using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JMapData
    {
        public int DataId;
        public string MapName;
    }

    public class MapDataLoader : ILoader<int, JMapData>
    {
        public List<JMapData> Maps = new List<JMapData>();

        public Dictionary<int, JMapData> MakeDict()
        {
            Dictionary<int, JMapData> mapDict = new Dictionary<int, JMapData>();
            foreach (JMapData map in Maps)
                mapDict.Add(map.DataId, map);
            return mapDict;
        }
    }
}