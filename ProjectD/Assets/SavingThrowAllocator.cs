using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingThrowAllocator : MonoBehaviour
{
    public GameObject prefab;

    public Character character;

    public TMPro.TextMeshProUGUI pointsText;

    public int points;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject myObject = Instantiate<GameObject>(prefab, transform);

            myObject.transform.Translate(new Vector3(0, -25 * i, 0));

            myObject.GetComponent<SavingThrowDisplay>().allocator = this;
            myObject.GetComponent<SavingThrowDisplay>().abilityType = (Character.AbilityScore)i;
            myObject.GetComponent<SavingThrowDisplay>().Init();
        }
    }

    public void IncreaseProficiency(Character.AbilityScore abilityType)
    {
        if (CanAffordProficiency(abilityType))
        {
            points -= GetCost(abilityType);

            character.SavingThrowProficiencies[(int)abilityType] += 1;
        }
    }

    public void DecreaseProficiency(Character.AbilityScore abilityType)
    {
        if (character.SavingThrowProficiencies[(int)abilityType] > 0)
        {
            character.SavingThrowProficiencies[(int)abilityType] -= 1;

            points += GetCost(abilityType);
        }
    }

    private bool CanAffordProficiency(Character.AbilityScore abilityType)
    {
        return (GetCost(abilityType) <= points);
    }

    private int GetCost(Character.AbilityScore abilityType)
    {
        return character.SavingThrowProficiencies[(int)abilityType] + 1;
    }

    // Update is called once per frame
    private void Update()
    {
        pointsText.text = points.ToString();
    }
}
