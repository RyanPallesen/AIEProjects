using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Character.AbilityScore;


public class Character : MonoBehaviour
{
    public string Test;


    public int level;
    public int proficiency
    {
        get { return (int)Mathf.Floor((7 + level) / 4); }
    }

    public int[] StatArray = new int[6];

    public enum AbilityScore
    {
        STRENGTH,
        INTELLIGENCE,
        WISDOM,
        DEXTERITY,
        CONSTITUTION,
        CHARISMA

    }



    private readonly AbilityScore[] AbilityStatTypes = new AbilityScore[18]
    {
        STRENGTH,
        DEXTERITY,
        DEXTERITY,
        DEXTERITY,
        INTELLIGENCE,
        INTELLIGENCE,
        INTELLIGENCE,
        INTELLIGENCE,
        INTELLIGENCE,
        WISDOM,
        WISDOM,
        WISDOM,
        WISDOM,
        WISDOM,
        CHARISMA,
        CHARISMA,
        CHARISMA,
        CHARISMA,
    };

    [HideInInspector] public int[] AbilityProficiencies = new int[18];
    [HideInInspector] public int[] AbilityBonuses = new int[18];

    public enum AbilityType
    {
        ATHLETICS,
        ACROBATICS,
        SLEIGHTOFHAND,
        STEALTH,
        ARCANA,
        HISTORY,
        INVESTIGATION,
        NATURE,
        RELIGION,
        ANIMALHANDLING,
        INSIGHT,
        MEDICINE,
        PERCEPTION,
        SURVIVAL,
        DECEPTION,
        INTIMIDATION,
        PERFORMANCE,
        PERSUASION
    }

    public int GetModifier(AbilityScore statType)
    {
        int baseStat = StatArray[(int)statType];

        baseStat -= 10;
        baseStat /= 2;
        return (int)Mathf.Floor(baseStat);
    }

    public int GetModifier(AbilityType statType)
    {
        return GetModifier(AbilityStatTypes[(int)statType]) + (AbilityProficiencies[(int)statType] * proficiency) + AbilityBonuses[(int)statType];
    }

    public int InterpretString(string input)
    {
        int output = 0;

        input.Replace(" ", "");
        input.ToLower();

        List<char> numbers = "0123456789".ToCharArray().ToList();
        List<char> variables = "+-".ToCharArray().ToList();


        int workingNumber = 0;
        bool isNegative = false;

        string workingString = "";
        bool isBuildingString = false;
        int numDice = 0;
        int diceType = 0;

        for (int i = 0; i < input.Length; i++)
        {

            if (input[i] == '$' || isBuildingString)
            {
                if (input[i] == '$')
                {
                    isBuildingString = !isBuildingString;
                    if (Enum.TryParse<AbilityScore>(workingString, out AbilityScore resultScore))
                    {
                        int toAdd = GetModifier(resultScore);
                        output += toAdd;
                        if (isNegative)
                        {
                            toAdd *= -1;
                        }
                        Debug.Log("Added modifier value of " + toAdd + " from " + workingString);
                    }
                    else if (Enum.TryParse<AbilityType>(workingString, out AbilityType resultType))
                    {
                        int toAdd = GetModifier(resultType);
                        if (isNegative)
                        {
                            toAdd *= -1;
                        }
                        output += toAdd;
                        Debug.Log("Added modifier value of " + toAdd + " from " + workingString);
                    }
                    else//add proficiency, Advantage/Disadvantage, damage types. Output dictionary with damage of each type?
                    {
                        Debug.LogWarning(workingString + " is not a valid AbilityType or AbilityScore");
                    }

                    workingString = "";
                }
                else
                {
                    workingString += input[i];
                }
            }
            else
            {
                if (numbers.Contains(input[i]))
                {
                    workingNumber *= 10;

                    if (Int32.TryParse(input[i].ToString(), out int result))
                    {
                        workingNumber += result;
                    }
                }
                else if (input[i] == 'd')
                {
                    numDice = workingNumber;
                    workingNumber = 0;
                }

                if (variables.Contains(input[i]) || i == input.Length - 1)// +, -
                {

                    isNegative = input[i] == '-';

                    if (numDice > 0)
                    {
                        diceType = workingNumber;
                        workingNumber = 0;
                        Debug.Log("Reached end, " + numDice + "d" + diceType);

                        for (int o = 0; o < numDice; o++)
                        {
                            int random = UnityEngine.Random.Range(1, diceType + 1);

                            if (isNegative)
                            {
                                random *= -1;
                            }

                            Debug.Log("Rolled d" + diceType + ", got " + random);
                            output += random;

                        }
                    }
                    numDice = 0;
                }
            }
        }

        return output;
    }
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Final was " + InterpretString(Test));
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
