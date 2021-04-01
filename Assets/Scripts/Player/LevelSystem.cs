using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    public event EventHandler OnXPChanged;
    public event EventHandler OnLevelChanged;

    private int level;
    private int xp;
    private int xpToNextLevel;

    public LevelSystem()
    {
        level = 0;
        xp = 0;
        xpToNextLevel = 100;
    }

    public void AddXP(int amount)
    {
        xp += amount;
        if (xp >= xpToNextLevel)
        {
            level++;
            xp -= xpToNextLevel;
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
        return (float)xp / xpToNextLevel;
    }
}
