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
    private string[] effects = { "EXP", "cyanSoul_Animation", "greenSoul_Animation", "redSoul_Animation", "redSoul_Animation" };

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
