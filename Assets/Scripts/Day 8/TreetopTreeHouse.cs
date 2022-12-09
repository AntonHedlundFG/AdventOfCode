using System.Collections.Generic;
using UnityEngine;

public class TreetopTreeHouse : AdventOfCode
{
    protected override void PartOne()
    {
        int[][] trees = GenerateTrees();
        Debug.Log(CountVisibleTrees(trees));
    }
    protected override void PartTwo()
    {
        int[][] trees = GenerateTrees();
        Debug.Log(CalculateBestScenicScore(trees));
    }

    private int[][] GenerateTrees()
    {
        string[] lines = ParseFile();
        int[][] trees = new int[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            trees[i] = new int[lines[i].Length];
            for (int j = 0; j < lines[i].Length; j++)
            {
                trees[i][j] = int.Parse(lines[i][j].ToString());
            }
        }
        return trees;
    }
    private int CountVisibleTrees(int[][] trees)
    {
        HashSet<Vector2Int> visibleCoords = new HashSet<Vector2Int>();
        for (int i = 0; i < trees.Length; i++)
        {
            //Left-To-Right
            int[] ltrarray = new int[trees.Length];
            for (int j = 0; j < trees.Length; j++)
            {
                ltrarray[j] = trees[i][j];
            }
            List<int> ltr = GetVisibleTreesInLine(ltrarray, false);
            foreach (int a in ltr)
            {
                visibleCoords.Add(new Vector2Int(i, a));
            }

            //Right-To-Left
            int[] rtlarray = new int[trees.Length];
            for (int j = 0; j < trees.Length; j++)
            {
                rtlarray[j] = trees[i][j];
            }
            List<int> rtl = GetVisibleTreesInLine(rtlarray, true);
            foreach (int a in rtl)
            {
                visibleCoords.Add(new Vector2Int(i, a));
            }

            //Top-To-Bottom
            int[] ttbarray = new int[trees.Length];
            for (int j = 0; j < trees.Length; j++)
            {
                ttbarray[j] = trees[j][i];
            }
            List<int> ttb = GetVisibleTreesInLine(ttbarray, false);
            foreach (int a in ttb)
            {
                visibleCoords.Add(new Vector2Int(a, i));
            }

            //Bottom-To-Top
            int[] bttarray = new int[trees.Length];
            for (int j = 0; j < trees.Length; j++)
            {
                bttarray[j] = trees[j][i];
            }
            List<int> btt = GetVisibleTreesInLine(bttarray, true);
            foreach (int a in btt)
            {
                visibleCoords.Add(new Vector2Int(a, i));
            }
        }

        return visibleCoords.Count;
    }
    private List<int> GetVisibleTreesInLine(int[] line, bool backwards)
    {
        List<int> list = new List<int>();
        int curHeight = -1;
        if (!backwards)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] > curHeight)
                {
                    list.Add(i);
                    curHeight = line[i];
                }
            }
        } else
        {
            for (int i = line.Length - 1; i >= 0; i--)
            {
                if (line[i] > curHeight)
                {
                    list.Add(i);
                    curHeight = line[i];
                }
            }
        }
        
        return list;
    }

    private int CalculateScenicScore(int[][] trees, int x, int y)
    {
        int baseHeight = trees[x][y];
        int n = 0, e = 0, s = 0, w = 0;

        //North
        for (int i = 1; x + i < trees.Length; i++)
        {
            n++;
            if (trees[x + i][y] >= baseHeight) { break; }
        }

        //South
        for (int i = 1; x - i >= 0; i++)
        {
            s++;
            if (trees[x - i][y] >= baseHeight) { break; }
        }

        //East
        for (int i = 1; y + i < trees.Length; i++)
        {
            e++;
            if (trees[x][y + i] >= baseHeight) { break; }
        }

        //West
        for (int i = 1; y - i >= 0; i++)
        {
            w++;
            if (trees[x][y - i] >= baseHeight) { break; }
        }

        return n*e*s*w;
    }
    private int CalculateBestScenicScore(int[][] trees)
    {
        int bestScore = -1;
        for (int i = 0; i < trees.Length; i++)
        {
            for (int j = 0; j < trees.Length; j++)
            {
                bestScore = Mathf.Max(bestScore, CalculateScenicScore(trees, i, j));
            }
        }
        return bestScore;
    }
}
