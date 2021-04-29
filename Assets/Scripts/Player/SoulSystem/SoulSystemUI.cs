using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulSystemUI : MonoBehaviour
{
    public Text stageText;
    private SoulSystem soulSystem;
    private SoulSystemAnimator soulSystemAnimator;
    
    private void SetStageNum(int stage)
    {
        stageText.text = stage.ToString(); 
    }

    public void SetSoulSystem(SoulSystem soulSystem)
    {
        this.soulSystem = soulSystem;
    }

    public void SetSoulSystemAnimator(SoulSystemAnimator soulSystemAnimator)
    {
        this.soulSystemAnimator = soulSystemAnimator;

        SetStageNum(soulSystemAnimator.getStage());

        soulSystemAnimator.OnSoulChanged += SoulSystemAnimator_OnSoulChanged;
        soulSystemAnimator.OnStageChanged += SoulSystemAnimator_OnStageChanged;
    }

    private void SoulSystemAnimator_OnStageChanged(object sender, EventArgs e)
    {
        SetStageNum(soulSystemAnimator.getStage());
    }

    private void SoulSystemAnimator_OnSoulChanged(object sender, EventArgs e)
    {
        
    }
}
