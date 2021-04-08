using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public enum SkillType
    {
        HealthUp1,
        HealthUp2,
        HealthUp3,
    }

    private List<SkillType> unlockedSkillTypeList;
    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    public void UnlockSkill(SkillType skilltype)
    {
        unlockedSkillTypeList.Add(skilltype);
    }

    public bool isSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }
}
