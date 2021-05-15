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
    public string[] effects = { "soul0", "soul1", "soul2", "soul3", "soul4" };

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
