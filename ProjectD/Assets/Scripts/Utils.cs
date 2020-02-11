using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static AbilityScore[] AbilityStatTypes = new AbilityScore[25]
    {
        AbilityScore.STRENGTH,
        AbilityScore.DEXTERITY,
        AbilityScore.DEXTERITY,
        AbilityScore.DEXTERITY,
        AbilityScore.INTELLIGENCE,
        AbilityScore.INTELLIGENCE,
        AbilityScore.INTELLIGENCE,
        AbilityScore.INTELLIGENCE,
        AbilityScore.INTELLIGENCE,
        AbilityScore.WISDOM,
        AbilityScore.WISDOM,
        AbilityScore.WISDOM,
        AbilityScore.WISDOM,
        AbilityScore.WISDOM,
        AbilityScore.CHARISMA,
        AbilityScore.CHARISMA,
        AbilityScore.CHARISMA,
        AbilityScore.CHARISMA,
        AbilityScore.DEXTERITY,
        AbilityScore.STRENGTH,
        AbilityScore.INTELLIGENCE,
        AbilityScore.WISDOM,
        AbilityScore.DEXTERITY,
        AbilityScore.CONSTITUTION,
        AbilityScore.CHARISMA,
    };

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
        PERSUASION,
        INITIATIVE,
        STRENGTHSAVE,
        INTELLIGENCESAVE,
        WISDOMSAVE,
        DEXTERITYSAVE,
        CONSTITUTIONSAVE,
        CHARISMASAVE
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

    public enum AbilityScore
    {
        STRENGTH,
        INTELLIGENCE,
        WISDOM,
        DEXTERITY,
        CONSTITUTION,
        CHARISMA
    }

    public static int GetModifier(Character character, AbilityScore statType)
    {
        float baseStat = character.StatArray[(int)statType];

        baseStat -= 10;
        baseStat /= 2f;
        baseStat = Mathf.FloorToInt(baseStat);
        return (int)baseStat;
    }

    public static int GetModifier(Character character, AbilityType statType)
    {
        int output = 0;

        output += GetModifier(character, AbilityStatTypes[(int)statType]);//Default modifier bonus
        output += GetProficiency(character, statType);//Default Proficiency
        output += GetAbilityBonus(character, statType);//Any extra bonuses

        return output;
    }

    public static int GetProficiency(Character character, AbilityType statType)
    {
        return (character.AbilityProficiencies[(int)statType] * character.proficiency);
    }

    public static int GetAbilityBonus(Character character, AbilityType statType)
    {
        return character.AbilityBonuses[(int)statType];
    }

    public static int RollDice(int numDie, int DieType, int Advantages = 0)
    {
        int result = 0;

        for (int o = 0; o < numDie; o++)
        {
            int random = UnityEngine.Random.Range(1, DieType + 1);




            for (int rolls = 0; rolls < Mathf.Abs(Advantages); rolls++)
            {

                if (Advantages > 0)//Advantage
                {
                    int newRandom = UnityEngine.Random.Range(1, DieType + 1);
                    Debug.Log("  Rolled Advantage, got " + newRandom);
                    random = Mathf.Max(random, newRandom);
                }
                else if (Advantages < 0)//Disadvantage
                {
                    int newRandom = UnityEngine.Random.Range(1, DieType + 1);
                    Debug.Log("  Rolled Disadvantage, got " + newRandom);
                    random = Mathf.Min(random, newRandom);

                }
            }

            result += random;
        }

        return result;
    }


    public static int InterpretString(Character character, string input, out string outputString)
    {
        int output = 0;

        input.Replace(" ", "");
        input.ToLower();

        outputString = "";

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

            if (numbers.Contains(input[i]))//work with new number in sequence
            {
                workingNumber *= 10;

                if (Int32.TryParse(input[i].ToString(), out int result))
                {
                    workingNumber += result;
                }
            }

            if (input[i] == '[')//Character to begin building string
            {
                isBuildingString = !isBuildingString;
            }

            if (input[i] == ']')//end string and apply
            {
                isBuildingString = !isBuildingString;
                workingString = workingString.ToUpper();
                if (Enum.TryParse<AbilityScore>(workingString, out AbilityScore resultScore))
                {
                    int toAdd = Utils.GetModifier(character, resultScore);

                    if (isNegative)
                    {
                        toAdd *= -1;
                        isNegative = !isNegative;

                    }

                    output += toAdd;
                }
                else if (Enum.TryParse<AbilityType>(workingString, out AbilityType resultType))
                {
                    int toAdd = GetModifier(character, resultType);

                    if (isNegative)
                    {
                        toAdd *= -1;
                        isNegative = !isNegative;
                    }

                    output += toAdd;
                }
                else if (workingString == "ADVANTAGE")
                {



                    if (workingNumber == 0)
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
                    Debug.Log("Added " + character.proficiency + " from PROFICIENCY");
                    output += character.proficiency;
                }
                else//damage types. Output dictionary with damage of each type?
                {
                    Debug.LogWarning(workingString + " is not a valid AbilityType or AbilityScore");
                }

                workingString = "";
            }

            if (isBuildingString)//add character to current string
            {

                if (!numbers.Contains(input[i]) && !variables.Contains(input[i]))
                {
                    workingString += input[i];
                }

            }
            else
            {

                if (input[i] == 'd')//set current stored number as number of dice
                {
                    numDice = workingNumber;
                    workingNumber = 0;
                }

                if (variables.Contains(input[i]) || i == input.Length - 1)//reached end of valid input via variable or end of string
                {
                    if (workingNumber > 0 && numDice > 0)//if can roll dice
                    {
                        diceType = workingNumber;
                        workingNumber = 0;

                        int result = RollDice(numDice, diceType, Advantage);

                        if (isNegative)
                        {
                            result *= -1;
                            isNegative = !isNegative;
                        }

                        output += result;

                    }
                    else //if adding number
                    {
                        output += workingNumber;
                    }

                    numDice = 0;
                }
            }

            if (variables.Contains(input[i]))//Set negative
            {
                isNegative = input[i] == '-';
            }
        }

        return output;
    }
}
