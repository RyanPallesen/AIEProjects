using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    public Character character;
    public PointAllocator pointAllocator;

    public Character.AbilityScore abilityScore;

    public TMPro.TextMeshProUGUI modifier;
    public TMPro.TextMeshProUGUI bonus;

    public TMPro.TextMeshProUGUI title;

    public Button incrementButton;
    public Button decrementButton;

    private void Start()
    {
        incrementButton.onClick.AddListener(() => pointAllocator.Increment(abilityScore));
        decrementButton.onClick.AddListener(() => pointAllocator.Decrement(abilityScore));
    }
    // Update is called once per frame
    void Update()
    {
        modifier.text = character.StatArray[(int)abilityScore].ToString();
        bonus.text = character.GetModifier(abilityScore).ToString();
        title.text = abilityScore.ToString().Substring(0, 3).ToUpper();
    }
}
