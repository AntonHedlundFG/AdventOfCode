using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCleaning : AdventOfCode
{
    protected override void PartOne()
    {
        int[][] assignmentPairs = GetAssignmentPairsFromFile();
        Debug.Log(NumberOfContainingPairs(assignmentPairs));
    }

    protected override void PartTwo()
    {
        int[][] assignmentPairs = GetAssignmentPairsFromFile();
        Debug.Log(NumberOfOverlappingPairs(assignmentPairs));
    }

    private int[][] GetAssignmentPairsFromFile()
    {
        string[] lines = ParseFile();
        int[][] assignmentPairs = new int[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            assignmentPairs[i] = new int[4];
            string[] pairs = lines[i].Split(new char[] { ',', '-' });
            for (int j = 0; j < pairs.Length; j++)
            {
                assignmentPairs[i][j] = int.Parse(pairs[j]);
            }
        }

        return assignmentPairs;
    }

    private bool PairContainsEachother(int[] pair)
    {
        if  (pair == null || pair.Length != 4) { return false; }

        int a1 = pair[0], a2 = pair[1], b1 = pair[2], b2 = pair[3];

        return ((a1 >= b1 && a2 <= b2) || (b1 >= a1 && b2 <= a2));
    }
    private int NumberOfContainingPairs(int[][] pairs)
    {
        int result = 0;
        for (int i = 0; i < pairs.Length; i++)
        {
            if (PairContainsEachother(pairs[i])) { result++; }
        }
        return result;
    }
    private int NumberOfOverlappingPairs(int[][] pairs)
    {
        int result = 0;
        for (int i = 0; i < pairs.Length; i++)
        {
            if (PairOverlaps(pairs[i])) { result++; }
        }
        return result;
    }
    private bool PairOverlaps(int[] pair)
    {
        if (pair == null || pair.Length != 4) { return false; }
        
        int a1 = pair[0], a2 = pair[1], b1 = pair[2], b2 = pair[3];

        return (AssignmentContains(a1, a2, b1, b2) || AssignmentContains(b1, b2, a1, a2));
    }
    private bool AssignmentContains(int a1, int a2, int b1, int b2)
    {
        return (AssignmentContains(a1, b1, b2) || AssignmentContains(a2, b1, b2));
    }
    private bool AssignmentContains(int a, int b1, int b2)
    {
        return (a == Mathf.Clamp(a, b1, b2));
    }
}
