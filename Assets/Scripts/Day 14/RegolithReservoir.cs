using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegolithReservoir : AdventOfCode
{
    protected override void PartOne()
    {
        Debug.Log(PerformTest(false));
    }

    protected override void PartTwo()
    {
        Debug.Log(PerformTest(true));
    }

    private int PerformTest(bool withFloor)
    {
        HashSet<Vector2Int> set = GenerateRockFormation();
        int lowestPoint = LowestPoint(set);
        int i = 0;
        while(i < 1000000) //Safe-guard for infinite loop
        {
            bool result;
            HashSet<Vector2Int> setAfter = TryDropSand(set, lowestPoint, out result, withFloor);
            if (!result) { break; }
            i++;
        }
        return i;
    }

    private HashSet<Vector2Int> GenerateRockFormation()
    {
        HashSet<Vector2Int> rockCoords = new HashSet<Vector2Int>();

        string[] lines = ParseFile();
        for (int i = 0; i < lines.Length; i++)
        {
            HashSet<Vector2Int> lineList = PointsFromPath(lines[i]);
            foreach (Vector2Int coord in lineList)
            {
                rockCoords.Add(coord);
            }
        }


        return rockCoords;
    }

    private HashSet<Vector2Int> PointsFromPath(string line)
    {
        HashSet<Vector2Int> returnList = new HashSet<Vector2Int>();

        string[] pairs = line.Split(" -> ");
        for (int i = 0; i < pairs.Length - 1; i++)
        {
            HashSet<Vector2Int> addSet = PointsInLine(ParsePair(pairs[i]), ParsePair(pairs[i + 1]));
            returnList.UnionWith(addSet);

        }

        return returnList;
    }
    private Vector2Int ParsePair(string word)
    {
        string[] words = word.Split(",");
        return new Vector2Int(int.Parse(words[0]), int.Parse(words[1]));
    }
    private HashSet<Vector2Int> PointsInLine(Vector2Int from, Vector2Int to)
    {
        HashSet<Vector2Int> returnList = new HashSet<Vector2Int>();
        Vector2Int dir = to - from;
        dir.Clamp(-Vector2Int.one, Vector2Int.one);

        Vector2Int nextVector = from;
        while (nextVector != to)
        {
            returnList.Add(nextVector);
            nextVector += dir;
        }
        returnList.Add(to);
        return returnList;
    }

    private int LowestPoint(HashSet<Vector2Int> set)
    {
        int result = int.MinValue;
        foreach (Vector2Int vector in set)
        {
            result = Math.Max(result, vector.y);
        }
        return result;
    }

    private HashSet<Vector2Int> TryDropSand(HashSet<Vector2Int> set, int lowestPoint, out bool result, bool withFloor)
    {
        Vector2Int curPos = new Vector2Int(500, 0);

        //Check if space is filled already
        if (set.Contains(curPos))
        {
            result = false;
            return set;
        }

        while (curPos.y <= lowestPoint)
        {
            Vector2Int testPos = curPos;

            //Check below
            testPos += new Vector2Int(0, 1);
            if (!set.Contains(testPos))
            {
                curPos = testPos;
                continue;
            }

            //Check below to left
            testPos += new Vector2Int(-1, 0);
            if (!set.Contains(testPos))
            {
                curPos = testPos;
                continue;
            }

            //Check below to right
            testPos += new Vector2Int(2, 0);
            if (!set.Contains(testPos))
            {
                curPos = testPos;
                continue;
            }

            //Sand is at rest
            set.Add(curPos);
            result = true;
            return set;
        }

        if(withFloor)
        {
            set.Add(curPos);
            result = true;
            return set;
        }

        result = false;
        return set;
    }

}
