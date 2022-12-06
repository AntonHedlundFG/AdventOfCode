using UnityEngine;

public class TuningTrouble : AdventOfCode
{
    protected override void PartOne()
    {
        string line = ParseFile()[0];
        int firstMarkerIndex = FindFirstUniqueMarkerIndex(line, 4);
        Debug.Log(firstMarkerIndex);
    }
    protected override void PartTwo()
    {
        string line = ParseFile()[0];
        int firstMarkerIndex = FindFirstUniqueMarkerIndex(line, 14);
        Debug.Log(firstMarkerIndex);
    }
    private int FindFirstUniqueMarkerIndex(string line, int length)
    {
        for (int i = 0; i < line.Length - length; i++)
        {
            if (SubstringContainsOnlyUniqueSymbols(line, i, length)) { return i + length; }
        }
        return -1;
    }
    private bool SubstringContainsOnlyUniqueSymbols(string line, int startIndex, int length)
    {
        for (int i = startIndex; i < startIndex + length - 1; i++)
        {
            for (int j = i + 1; j < startIndex + length; j++)
            {
                if (line[i] == line[j]) { return false; }
            }
        }
        return true;
    }
}
