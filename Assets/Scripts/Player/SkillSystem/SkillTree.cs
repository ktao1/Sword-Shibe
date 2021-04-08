using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class SkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

    private void Awake()
    {
        transform.Find("SkillBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            playerSkills.UnlockSkill(PlayerSkills.SkillType.HealthUp1);
        };
    }

    public void setPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
    }
}
