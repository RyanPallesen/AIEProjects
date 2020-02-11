using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProficiencyAllocator : MonoBehaviour
{
    public GameObject prefab;

    public Character character;

    public TMPro.TextMeshProUGUI pointsText;

    public int points;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            GameObject myObject = Instantiate<GameObject>(prefab, transform);

            myObject.transform.Translate(new Vector3(0, -25 * i, 0));

            myObject.GetComponent<ProficiencyDisplay>().allocator = this;
            myObject.GetComponent<ProficiencyDisplay>().abilityType = (Utils.AbilityType)i;
            myObject.GetComponent<ProficiencyDisplay>().Init();
        }
    }

    public void IncreaseProficiency(Utils.AbilityType abilityType)
    {
        if (CanAffordProficiency(abilityType))
        {
            points -= GetCost(abilityType);

            character.AbilityProficiencies[(int)abilityType] += 1;
        }
    }

    public void DecreaseProficiency(Utils.AbilityType abilityType)
    {
        if (character.AbilityProficiencies[(int)abilityType] > 0)
        {
            character.AbilityProficiencies[(int)abilityType] -= 1;

            points += GetCost(abilityType);
        }
    }

    private bool CanAffordProficiency(Utils.AbilityType abilityType)
    {
        return (GetCost(abilityType) <= points);
    }

    private int GetCost(Utils.AbilityType abilityType)
    {
        return ((character.AbilityProficiencies[(int)abilityType] + 1 )* (character.AbilityProficiencies[(int)abilityType] + 1)) - character.AbilityProficiencies[(int)abilityType];
    }

    // Update is called once per frame
    private void Update()
    {
        pointsText.text = points.ToString();
    }
}
