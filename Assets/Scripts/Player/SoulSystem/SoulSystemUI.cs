using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulSystemUI : MonoBehaviour
{
    public Animator soulEffect;
    public Player player;
    public int soul;
    public string[] effects = {"soul0", "soul1", "soul2", "soul3", "soul4"};

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        soulEffect = GameObject.Find("Soul").GetComponent<Animator>();
        soulEffect.Play(effects[0]);
    }

   public void playEffect(int i)
    {
        Debug.Log(effects[i]);
        soulEffect.Play(effects[i]);
    }

}
    /*
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
*/