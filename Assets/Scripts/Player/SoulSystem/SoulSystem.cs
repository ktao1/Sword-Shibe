using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoulSystem
{
    public event EventHandler OnSoulChanged;
    public event EventHandler OnStageChanged;

    private int stage;
    private int soul;

    private static readonly int[] soulPertStage = new[] { 10, 25, 50, 75, 100 };

    public SoulSystem()
    {
        stage = 0;
        soul = 0;
    }

    public void AddSoul(int amount)
    {

        if (!isMaxStage())
        {
            soul += amount;
            while (!isMaxStage() && soul >= GetSoultoNextStage(stage))
            {
                stage++;
                if (OnStageChanged != null) OnStageChanged(this, EventArgs.Empty);
            }
            if (OnSoulChanged != null) OnSoulChanged(this, EventArgs.Empty);
        }
    }

    public int getSoul()
    {
        return soul;
    }
    public int getStage()
    {
        return stage;
    }

    public int GetSoultoNextStage(int stage)
    {
        if (stage < soulPertStage.Length)
        {
            return soulPertStage[stage];
        }
        else
        {
            // level Invalid;
            Debug.LogError("Stage invalid: " + stage);
            return 1000;
        }
    }

    public bool isMaxStage()
    {
        return isMaxStage(stage);
    }
    public bool isMaxStage(int stage)
    {
        return stage == soulPertStage.Length - 1;
    }

}
