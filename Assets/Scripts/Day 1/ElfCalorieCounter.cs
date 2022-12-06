using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfCalorieCounter : AdventOfCode
{
    protected override void PartOne()
    {
        string[] lines = ParseFile();
        int mostCalories = -1;
        int currentCalories = 0;
        int i = 0;

        while (i < lines.Length)
        {
            try
            {
                int parseInt = int.Parse(lines[i]);
                currentCalories += parseInt;
            }
            catch (FormatException)
            {
                mostCalories = mostCalories < currentCalories ? currentCalories : mostCalories;
                currentCalories = 0;
            }
            i++;
        }
        Debug.Log(mostCalories);
    }

    protected override void PartTwo()
    {
        string[] lines = ParseFile();
        List<int> mostCalories = new List<int>();
        int i = 0;
        int currentCalories = 0;

        while(i < lines.Length)
        {
            try
            {
                int parseInt = int.Parse(lines[i]);
                currentCalories += parseInt;
            }
            catch (FormatException)
            {
                mostCalories = InsertKeepHighestThree(mostCalories, currentCalories);
                currentCalories = 0;
            }
            i++;
        }
        int totalCalories = 0;
        for (i = 0; i < mostCalories.Count; i++)
        {
            totalCalories += mostCalories[i];
        }
        Debug.Log(totalCalories);
    }

    private List<int> InsertKeepHighestThree(List<int> list, int newInt)
    {
        list.Add(newInt);

        if (list.Count < 4) { return list; }
        
        int lowestIndex = 0;
        int lowestValue = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] <= lowestValue)
            {
                lowestValue = list[i];
                lowestIndex = i;
            }
        }
        list.RemoveAt(lowestIndex);
        return list;
    }

}
