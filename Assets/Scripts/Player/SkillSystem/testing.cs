using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SkillTree skillTree;

    private void Start()
    {
        skillTree.setPlayerSkills(player.GetPlayerSkills());
    }
}
