using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAllocator : MonoBehaviour
{
    public Character character;

    public int points;

    public TMPro.TextMeshProUGUI pointsText;

    public bool CanBuy(Utils.AbilityScore abilityScore)
    {
        if (GetCost(abilityScore) <= points)
        {
            return true;
        }

        return false;
    }

    public int GetCost(Utils.AbilityScore abilityScore)
    {
        int baseStat = character.StatArray[(int)abilityScore];

        baseStat -= 10;
        baseStat /= 2;

        

        return Mathf.FloorToInt(Mathf.Clamp(baseStat, 1, 10));
    }

    public void Increment(Utils.AbilityScore abilityScore)
    {
        if (CanBuy(abilityScore))
        {
            points -= GetCost(abilityScore);

            character.StatArray[(int)abilityScore] += 1;
        }
    }

    public void Decrement(Utils.AbilityScore abilityScore)
    {
        if (character.StatArray[(int)abilityScore] > 8)
        {
            character.StatArray[(int)abilityScore] -= 1;

            points += GetCost(abilityScore);
        }
    }

    private void Update()
    {
        pointsText.text = points.ToString();
    }
}
