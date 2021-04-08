using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class LevelSystemAnimator
{
    public event EventHandler OnXPChanged;
    public event EventHandler OnLevelChanged;

    private LevelSystem levelSystem;
    private bool isAnimating;
    private float updateTimer;
    private float updateTimerMax;

    private int level;
    private int xp;

    public LevelSystemAnimator(LevelSystem levelSystem)
    {

        SetLevelSystem(levelSystem);
        updateTimerMax = 0.01f;

        FunctionUpdater.Create(() => Update());

    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {

        this.levelSystem = levelSystem;

        level = levelSystem.getLevel();
        xp = levelSystem.GetXP();

        levelSystem.OnXPChanged += levelSystem_OnXPChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }

    private void levelSystem_OnXPChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }

    private void Update()
    {
        if (isAnimating)
        {
            updateTimer += Time.deltaTime;
            while(updateTimer > updateTimerMax)
            {
                updateTimer -= updateTimerMax;
                UpdateXP();
            }
        }
    }

    private void UpdateXP()
    {
        if (level < levelSystem.getLevel())
        {
            addXP();
        }
        else
        {
            if (xp < levelSystem.GetXP())
            {
                addXP();
            }
            else
            {
                isAnimating = false;
            }
        }
    }


    private void addXP()
    {
        xp++;
        if(xp >= levelSystem.GetXPtoNextLevel(level))
        {
            level++;
            xp = 0;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnXPChanged != null) OnXPChanged(this, EventArgs.Empty);
    }

    public int getLevel()
    {
        return level;
    }

    public float GetXPNormalized()
    {
        if (levelSystem.isMaxLevel())
        {
            return 1f;
        }
        else
            return (float)xp / levelSystem.GetXPtoNextLevel(level);
    }

}
