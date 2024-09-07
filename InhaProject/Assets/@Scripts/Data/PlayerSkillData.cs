using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JPlayerSkillData
    {
        public int DataId;
        public float SkillCoolTime;
    }

    public class PlayerSkillDataLoader : ILoader<int, JPlayerSkillData>
    {
        public List<JPlayerSkillData> PlayerSkills = new List<JPlayerSkillData>();

        public Dictionary<int, JPlayerSkillData> MakeDict()
        {
            Dictionary<int, JPlayerSkillData> playerSkillDict = new Dictionary<int, JPlayerSkillData>();
            foreach (JPlayerSkillData playerSkill in PlayerSkills)
                playerSkillDict.Add(playerSkill.DataId, playerSkill);
            return playerSkillDict;
        }
    }
}