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

    private static readonly int[] XpPerLevel = new[] {100, 150, 200,
                                                      250, 300, 350,
                                                      400, 450, 500,
                                                      550, 600, 650,
                                                      700, 750, 800,
                                                      850, 900, 950,
                                                      1000
                                                      };

    public LevelSystem()
    {
        level = 0;
        xp = 0;
    }

    public void AddXP(int amount)
    {
        if (!isMaxLevel())
        {
            xp += amount;
            while (!isMaxLevel() && xp >= GetXPtoNextLevel(level))
            {
                level++;
                xp -= GetXPtoNextLevel(level);
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }
            if (OnXPChanged != null) OnXPChanged(this, EventArgs.Empty);
        }
    }

    public int getLevel()
    {
        return level;
    }

    public float GetXPNormalized()
    {
        if (isMaxLevel())
        {
            return 1f;
        }
        else
            return (float)xp / GetXPtoNextLevel(level);
    }

    public int GetXP()
    {
        return xp;
    }
    public int GetXPtoNextLevel(int level)
    {
        if(level < XpPerLevel.Length)
        {
            return XpPerLevel[level];
        }
        else
        {
            // level Invalid;
            Debug.LogError("Lvel invalid: " + level);
            return 1000;
        }
    }

    public bool isMaxLevel()
    {
        return isMaxLevel(level);
    }
    public bool isMaxLevel(int level)
    {
        return level == XpPerLevel.Length - 1;
    }
}
