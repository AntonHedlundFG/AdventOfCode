using System.Collections.Generic;
using UnityEngine;

public class Rucksack : AdventOfCode
{
    protected override void PartOne()
    {
        string[] rucksacks = ParseFile();
        int totalPriority = 0;
        for (int i = 0; i < rucksacks.Length; i++)
        {
            totalPriority += GetPriorityFromSplitRucksack(SplitRucksack(rucksacks[i]));
        }

        Debug.Log(totalPriority);
    }
    protected override void PartTwo()
    {
        string[] rucksacks = ParseFile();
        int totalPriority = 0;
        for (int i = 0; i < rucksacks.Length - 2; i += 3)
        {
            totalPriority += GetPriority(GetBadgeTypeFromRucksackTrio(new string[] { rucksacks[i], rucksacks[i + 1], rucksacks[i + 2] }));
        }
        Debug.Log(totalPriority);
    }

    private string[] SplitRucksack(string line)
    {
        int halfLength = line.Length / 2;
        string a = line.Substring(0, halfLength);
        string b = line.Substring(halfLength, halfLength);
        return new string[] { a, b };
    }
    private int GetPriority(char a)
    {
        int value = char.ToUpper(a) - 64;
        if (char.IsUpper(a)) { value += 26; }
        return value;
    }
    private int GetPriorityFromSplitRucksack(string[] sack)
    {
        string a = sack[0], b = sack[1];

        HashSet<char> firstCompartment = new HashSet<char>();
        for (int i = 0; i < a.Length; i++)
        {
            firstCompartment.Add(a[i]);
        }

        for (int i = 0; i < b.Length; i++)
        {
            if (firstCompartment.Contains(b[i]))
            {
                return GetPriority(b[i]);
            }
        }


        return 0;
    }
    private char GetBadgeTypeFromRucksackTrio(string[] rucksacks)
    {
        string a = rucksacks[0], b = rucksacks[1], c = rucksacks[2];
        HashSet<char> rs1 = new HashSet<char>(), rs2 = new HashSet<char>();

        for (int i = 0; i < a.Length; i++)
        {
            rs1.Add(a[i]);
        }
        for (int i = 0; i < b.Length; i++)
        {
            if (rs1.Contains(b[i])) { rs2.Add(b[i]); }
        }
        for (int i = 0; i < c.Length; i++)
        {
            if (rs2.Contains(c[i])) { return c[i]; }
        }
        return 'a';
    }

}
