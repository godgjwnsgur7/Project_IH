using System.Collections.Generic;
using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class JSkillSlotData
    {
        public string SkillType;
        public string Name;
        public string Script;
        public string Cooltime;
        public string MpAmount;
    }

    public class SkillSlotDataLoader : ILoader<string, JSkillSlotData>
    {
        public List<JSkillSlotData> Skills = new List<JSkillSlotData>();

        public Dictionary<string, JSkillSlotData> MakeDict()
        {
            Dictionary<string, JSkillSlotData> skillSlotDict = new Dictionary<string, JSkillSlotData>();
            foreach (JSkillSlotData skill in Skills)
            {
                skillSlotDict.Add(skill.SkillType, skill);
            }
            return skillSlotDict;
        }
    }
}
