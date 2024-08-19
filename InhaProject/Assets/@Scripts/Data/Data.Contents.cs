#region Sample
using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class SampleData
    {
        public int dataId;

    }

    public class SampleDataLoader : ILoader<int, SampleData>
    {
        public List<SampleData> Samples = new List<SampleData>();

        public Dictionary<int, SampleData> MakeDict()
        {
            Dictionary<int, SampleData> sampleDict = new Dictionary<int, SampleData>();
            foreach (SampleData sample in Samples)
                sampleDict.Add(sample.dataId, sample);
            return sampleDict;
        }
    }
    #endregion
}
