using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler OnSkillPointsChanged;
    public event EventHandler<OnskillUnlockedEventArgs> OnSkillUnlocked;
    public class OnskillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }
    public enum SkillType
    {
        None,
        HealthUp_1,
        HealthUp_2,
        HealthUp_3,
        DashFaster_1,
        DashFaster_2,
        DashFaster_3,
        AttackSpeed_1,
        AttackSpeed_2,
        AttackSpeed_3,
    }

    private List<SkillType> unlockedSkillTypeList;

    private int skillPoints; 
    public void addSkillPoint()
    {
        skillPoints++;
        OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    public int GetSkillPoints()
    {
        return skillPoints;
    }

    private void UnlockSkill(SkillType skilltype)
    {
        if (!isSkillUnlocked(skilltype))
        {
            unlockedSkillTypeList.Add(skilltype);
            OnSkillUnlocked?.Invoke(this, new OnskillUnlockedEventArgs { skillType = skilltype });
        }
        
    }

    public bool isSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public bool CanUnlock(SkillType skillType)
    {
        SkillType skillRequirement = getSkillRequirement(skillType);
        if (skillRequirement != SkillType.None)
        {
            if (isSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            return true;
        }
    }

    public SkillType getSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.HealthUp_2: 
                return SkillType.HealthUp_1;
            case SkillType.HealthUp_3: 
                return SkillType.HealthUp_2;
            case SkillType.DashFaster_2:
                return SkillType.DashFaster_1;
            case SkillType.DashFaster_3:
                return SkillType.DashFaster_2;
            case SkillType.AttackSpeed_2:
                return SkillType.AttackSpeed_1;
            case SkillType.AttackSpeed_3:
                return SkillType.AttackSpeed_2;
        }
        return SkillType.None;
    }

    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlock(skillType))
        {
            if (skillPoints > 0)
            {
                skillPoints--;
                OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
                UnlockSkill(skillType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
