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
    private LevelSystemAnimator levelSystemAnimator;

    private void Awake()
    {
        levelText = transform.Find("LevelText").GetComponent<Text>();
        XPBarImage = transform.Find("XPBar").Find("XP").GetComponent<Image>();
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
        this.levelSystem = levelSystem;
    }

    public void SetLevelSystemAnimator(LevelSystemAnimator levelSystemAnimator)
    {
        // set the levelsystem object
        this.levelSystemAnimator = levelSystemAnimator;

        // update the starting values;
        SetLevelNum(levelSystemAnimator.getLevel());
        SetXPBarSize(levelSystemAnimator.GetXPNormalized());

        // subscribe to the changed events
        levelSystemAnimator.OnXPChanged += levelSystemAnimator_OnXPChanged;
        levelSystemAnimator.OnLevelChanged += levelSystemAnimator_OnLevelChanged;
    }

    private void levelSystemAnimator_OnLevelChanged(object sender, EventArgs e)
    {
        SetLevelNum(levelSystemAnimator.getLevel());
    }

    private void levelSystemAnimator_OnXPChanged(object sender, EventArgs e)
    {
        SetXPBarSize(levelSystemAnimator.GetXPNormalized());
    }
}
