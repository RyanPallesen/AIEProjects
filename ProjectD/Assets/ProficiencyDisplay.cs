using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProficiencyDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI abilityText;
    public Character.AbilityType abilityType;

    public ProficiencyAllocator allocator;

    public Button IncrementButton;
    public Button DecrementButton;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        IncrementButton.onClick.AddListener(() => allocator.IncreaseProficiency(abilityType));
        DecrementButton.onClick.AddListener(() => allocator.DecreaseProficiency(abilityType));
    }
    // Update is called once per frame
    void Update()
    {
        abilityText.text = abilityType.ToString();

        if(allocator.character.AbilityProficiencies[(int)abilityType] > 0)
        {
            abilityText.text += " +" + allocator.character.AbilityProficiencies[(int)abilityType].ToString();
        }
    }
}
