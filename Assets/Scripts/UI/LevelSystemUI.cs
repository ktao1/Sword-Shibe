using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystemUI : MonoBehaviour
{
    private Text levelText;
    private Image XPBarImage;
    private LevelSystem levelSystem;
    private void Awake()
    {
        levelText = transform.Find("LevelText").GetComponent<Text>();
        XPBarImage = transform.Find("XPBar").Find("XP").GetComponent<Image>();

        SetXPBarSize(.1f);
        SetLevelNum(7);
    }

    private void SetXPBarSize(float XPNormalized)
    {
        XPBarImage.fillAmount = XPNormalized;
    }

    private void SetLevelNum(int level)
    {
        levelText.text = "Level " + (level + 1);
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        // set the levelsystem object
        this.levelSystem = levelSystem;

        // update the starting values;
        SetLevelNum(levelSystem.getLevel());
        SetXPBarSize(levelSystem.GetXPNormalized());

        // subscribe to the changed events
        levelSystem.OnXPChanged += levelSystem_OnXPChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        SetLevelNum(levelSystem.getLevel());
    }

    private void levelSystem_OnXPChanged(object sender, EventArgs e)
    {
        SetXPBarSize(levelSystem.GetXPNormalized());
    }
}
