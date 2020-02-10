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
    public int[] SavingThrows = new int[6];
    public int[] SavingThrowProficiencies = new int[6];

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

    public enum DamageType
    {
        SLASHING,
        PIERCING,
        BLUDGEONING,
        COLD,
        POISON,
        ACID,
        PSYCHIC,
        FIRE,
        NECROTIC,
        RADIANT,
        FORCE,
        THUNDER,
        LIGHTNING
    }

    public List<DamageType> Resistances = new List<DamageType>();
    public List<DamageType> Weaknesses = new List<DamageType>();
    public List<DamageType> Immunities = new List<DamageType>();

    public int GetModifier(AbilityScore statType)
    {
        float baseStat = StatArray[(int)statType];

        baseStat -= 10;
        baseStat /= 2f;
        baseStat = Mathf.FloorToInt(baseStat);
        return (int)baseStat;
    }

    public int GetModifier(AbilityType statType)
    {
        return GetModifier(AbilityStatTypes[(int)statType]) + (AbilityProficiencies[(int)statType] * proficiency) + AbilityBonuses[(int)statType];
    }

    public int GetSavingThrow(AbilityScore statType)
    {
        return SavingThrows[(int)statType] + (proficiency * SavingThrowProficiencies[(int)statType]);
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
        
        int Advantage = 0;

        string workingString = "";
        bool isBuildingString = false;

        int numDice = 0;
        int diceType = 0;

        for (int i = 0; i < input.Length; i++)
        {


            if (numbers.Contains(input[i]))
            {
                workingNumber *= 10;

                if (Int32.TryParse(input[i].ToString(), out int result))
                {
                    workingNumber += result;
                }
            }

            if (input[i] == '[' || isBuildingString)
            {
                if(input[i] == '[')
                {
                    isBuildingString = !isBuildingString;
                }
                else if (input[i] == ']')
                {
                    isBuildingString = !isBuildingString;
                    workingString = workingString.ToUpper();
                    if (Enum.TryParse<AbilityScore>(workingString, out AbilityScore resultScore))
                    {
                        int toAdd = GetModifier(resultScore);
                        if (isNegative)
                        {
                            toAdd *= -1;
                            isNegative = !isNegative;

                        }
                        output += toAdd;

                        Debug.Log("Added modifier value of " + toAdd + " from " + workingString);
                    }
                    else if (Enum.TryParse<AbilityType>(workingString, out AbilityType resultType))
                    {
                        int toAdd = GetModifier(resultType);
                        if (isNegative)
                        {
                            toAdd *= -1;
                            isNegative = !isNegative;
                        }
                        output += toAdd;
                        Debug.Log("Added modifier value of " + toAdd + " from " + workingString);
                    }
                    else if (workingString == "ADVANTAGE")
                    {



                        if(workingNumber == 0)
                        {
                            workingNumber = 1;
                        }

                        Advantage += workingNumber;
                        workingNumber = 0;
                    }
                    else if (workingString == "DISADVANTAGE")
                    {

                        if (workingNumber == 0)
                        {
                            workingNumber = 1;
                        }

                        Advantage -= workingNumber;
                        workingNumber = 0;

                    }
                    else if (workingString == "PROFICIENCY")
                    {
                        Debug.Log("Added " + proficiency + " from PROFICIENCY");
                        output += proficiency;
                    }
                    else//damage types. Output dictionary with damage of each type?
                    {
                        Debug.LogWarning(workingString + " is not a valid AbilityType or AbilityScore");
                    }

                    workingString = "";
                }
                else
                {
                    if (!numbers.Contains(input[i]) && !variables.Contains(input[i]))
                    {
                        workingString += input[i];
                    }
                }
            }
            else
            {

                if (input[i] == 'd')
                {
                    numDice = workingNumber;
                    workingNumber = 0;
                }

                if (variables.Contains(input[i]) || i == input.Length - 1)// +, -
                {
                    if (workingNumber > 0 && numDice > 0)
                    {
                        diceType = workingNumber;
                        workingNumber = 0;
                        Debug.Log("Reached end, " + numDice + "d" + diceType);

                        for (int o = 0; o < numDice; o++)
                        {
                            int random = UnityEngine.Random.Range(1, diceType + 1);

                            Debug.Log(" Rolled a d" + diceType + ", got " + random);

                            if (Advantage != 0)
                            {

                                for (int rolls = 0; rolls < Mathf.Abs(Advantage); rolls++)
                                {

                                    if (Advantage > 0)//Advantage
                                    {
                                        int newRandom = UnityEngine.Random.Range(1, diceType + 1);
                                        Debug.Log("  Rolled Advantage, got " + newRandom);
                                        random = Mathf.Max(random, newRandom);
                                    }
                                    else if (Advantage < 0)//Disadvantage
                                    {
                                        int newRandom = UnityEngine.Random.Range(1, diceType + 1);
                                        Debug.Log("  Rolled Disadvantage, got " + newRandom);
                                        random = Mathf.Min(random, newRandom);

                                    }
                                }

                            }

                            if (isNegative)
                            {
                                random *= -1;
                            }


                            output += random;

                        }

                        if(isNegative)
                        {
                            isNegative = !isNegative;
                        }
                    }
                    else
                    {
                        output += workingNumber;
                    }
                    numDice = 0;
                }
            }

            if (variables.Contains(input[i]))
            {
                isNegative = input[i] == '-';
            }
        }

        return output;
    }
    // Start is called before the first frame update
    private void Start()
    {
    }

    public TMPro.TMP_InputField input;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Final was " + InterpretString(input.text));
        }
    }
}
