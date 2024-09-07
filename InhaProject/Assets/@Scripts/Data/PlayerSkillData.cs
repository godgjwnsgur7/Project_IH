using System.Collections.Generic;
using System;

namespace Data
{
    // 일단 미사용
    [Serializable]
    public class JPlayerSkillData
    {
        public int DataId;
        public List<float> DamageRatioList;
        public float SkillCoolTime;
        public float MpAmount;
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