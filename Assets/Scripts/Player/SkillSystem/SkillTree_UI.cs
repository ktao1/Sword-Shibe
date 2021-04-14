using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
public class SkillTree_UI : MonoBehaviour
{
    private PlayerSkills playerSkills;
    private List<SkillButton> skillButtonsList;

    [SerializeField] private SkillUnlockPath[] skillUnlockPathArray;

    private Text skillPontsText;
    private void Awake()
    {
        skillPontsText = transform.Find("SkillPoints").GetComponent<Text>();
    }


    public void setPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;

        setSkillButtonsList();

        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerSkills.OnSkillPointsChanged += PlayerSkills_OnSkillPointsChanged;

        UpdateVisuals();
        UpdateSkillPoints();
    }

    private void PlayerSkills_OnSkillPointsChanged(object sender, System.EventArgs e)
    {
        UpdateSkillPoints();
    }

    public void setSkillButtonsList()
    {
        skillButtonsList = new List<SkillButton>();
        // Health
        skillButtonsList.Add(new SkillButton(transform.Find("HealthTrack").Find("HealthUp1Btn"), playerSkills, PlayerSkills.SkillType.HealthUp_1));
        skillButtonsList.Add(new SkillButton(transform.Find("HealthTrack").Find("HealthUp2Btn"), playerSkills, PlayerSkills.SkillType.HealthUp_2));
        skillButtonsList.Add(new SkillButton(transform.Find("HealthTrack").Find("HealthUp3Btn"), playerSkills, PlayerSkills.SkillType.HealthUp_3));
        // Dash
        skillButtonsList.Add(new SkillButton(transform.Find("DashFasterTrack").Find("DashFaster1Btn"), playerSkills, PlayerSkills.SkillType.DashFaster_1));
        skillButtonsList.Add(new SkillButton(transform.Find("DashFasterTrack").Find("DashFaster2Btn"), playerSkills, PlayerSkills.SkillType.DashFaster_2));
        skillButtonsList.Add(new SkillButton(transform.Find("DashFasterTrack").Find("DashFaster3Btn"), playerSkills, PlayerSkills.SkillType.DashFaster_3));
        // Attack
        skillButtonsList.Add(new SkillButton(transform.Find("AttackTrack").Find("Attack1Btn"), playerSkills, PlayerSkills.SkillType.AttackSpeed_1));
        skillButtonsList.Add(new SkillButton(transform.Find("AttackTrack").Find("Attack2Btn"), playerSkills, PlayerSkills.SkillType.AttackSpeed_2));
        skillButtonsList.Add(new SkillButton(transform.Find("AttackTrack").Find("Attack3Btn"), playerSkills, PlayerSkills.SkillType.AttackSpeed_3));

    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnskillUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateSkillPoints()
    {
        skillPontsText.text = playerSkills.GetSkillPoints().ToString();
    }

    private void UpdateVisuals()
    {
        foreach (SkillButton skillButton in skillButtonsList)
        {
            skillButton.UpdateVisual();
        }

        foreach (SkillUnlockPath skillUnlockPath in skillUnlockPathArray)
        {
            foreach(Image linkImage in skillUnlockPath.linkImageArray)
            {
                linkImage.color = new Color(.5F, .5F, .5f);
            }
        }
        foreach (SkillUnlockPath skillUnlockPath in skillUnlockPathArray)
        {
            if(playerSkills.isSkillUnlocked(skillUnlockPath.SkillType) || playerSkills.CanUnlock(skillUnlockPath.SkillType))
            {
                foreach (Image linkImage in skillUnlockPath.linkImageArray)
                {
                    linkImage.color = Color.white;
                }
            }
        }
    }


    private class SkillButton
    {
        private Transform transform;
        private Image image;
        private PlayerSkills playerSkills;
        private PlayerSkills.SkillType skillType;

        public SkillButton(Transform transform, PlayerSkills playerSkills, PlayerSkills.SkillType skillType)
        {
            this.transform = transform;
            Debug.Log(transform);
            this.playerSkills = playerSkills;
            this.skillType = skillType;
            this.image = transform.Find("image").GetComponent<Image>();

            transform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                if (!playerSkills.TryUnlockSkill(skillType))
                {
                    Debug.Log("Can't unlock" + skillType + "!");
                }
            };
        }

        public void UpdateVisual()
        {
            if (playerSkills.isSkillUnlocked(skillType))
            {
                image.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                if (playerSkills.CanUnlock(skillType))
                {
                    image.color = new Color32(110, 110, 110, 255);
                }
                else
                {
                    image.color = new Color32(200, 0, 0, 255);
                }
            }
        }

    }

    [System.Serializable]
    public class SkillUnlockPath
    {
        public PlayerSkills.SkillType SkillType;
        public Image[] linkImageArray;
    }
}
