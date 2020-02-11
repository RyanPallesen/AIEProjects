using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utils;

public class Character : MonoBehaviour
{
    public int level;
    public int proficiency
    {
        get { return (int)Mathf.Floor((7 + level) / 4); }
    }

    public int[] StatArray = new int[6];

    [HideInInspector] public int[] AbilityProficiencies = new int[26];
    [HideInInspector] public int[] AbilityBonuses = new int[26];

    public List<DamageType> Resistances = new List<DamageType>();
    public List<DamageType> Weaknesses = new List<DamageType>();
    public List<DamageType> Immunities = new List<DamageType>();

}
