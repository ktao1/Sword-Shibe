using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class SoulSystemAnimator : MonoBehaviour
{
    public event EventHandler OnSoulChanged;
    public event EventHandler OnStageChanged;
    private SoulSystem soulSystem;
    private bool isAnimating;
    private float updateTimer;
    private float updateTimerMax;

    private int stage;
    private int soul;

    public SoulSystemAnimator(SoulSystem soulSystem)
    {
        setSoulSystem(soulSystem);
        updateTimerMax = 0.01f;

        FunctionUpdater.Create(() => Update());
    }
    public void setSoulSystem(SoulSystem soulSystem)
    {
        this.soulSystem = soulSystem;
        stage = soulSystem.getStage();
        soul = soulSystem.getSoul();
        soulSystem.OnStageChanged += SoulSystem_OnLevelChanged;
        soulSystem.OnSoulChanged += SoulSystem_OnSoulChanged;
    }

    private void SoulSystem_OnSoulChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }

    private void SoulSystem_OnLevelChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating)
        {
            updateTimer += Time.deltaTime;
            while (updateTimer > updateTimerMax)
            {
                updateTimer -= updateTimerMax;
                UpdateSoul();
            }
        }
    }
    private void UpdateSoul()
    {
        if (stage < soulSystem.getStage())
        {
            addSoul();
        }
        else
        {
            if (soul < soulSystem.getSoul())
            {
                addSoul();
            }
            else
            {
                isAnimating = false;
            }
        }
    }
    private void addSoul()
    {
        soul++;
        if (soul >= soulSystem.GetSoultoNextStage(stage))
        {
            stage++;
            soul = 0;
            if (OnStageChanged != null) OnStageChanged(this, EventArgs.Empty);
        }
        if (OnSoulChanged != null) OnSoulChanged(this, EventArgs.Empty);
    }

    public int getStage()
    {
        return stage;
    }

}
