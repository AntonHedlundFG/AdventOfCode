using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class SupplyStacks : AdventOfCode
{
    protected override void PartOne()
    {
        RunProgram(false);
    }

    protected override void PartTwo()
    {
        RunProgram(true);
    }
    private void RunProgram(bool entireStack)
    {
        string[] lines = ParseFile();
        string[][] split = SplitStartConditionsFromInstructions(lines);

        Stack<char>[] startConditions = ParseStartConditions(split[0]);
        int[][] instructions = ParseInstructions(split[1]);

        Stack<char>[] endConditions = ExecuteInstructions(startConditions, instructions, entireStack);
        DebugLogTopObjects(endConditions);
    }

    private string[][] SplitStartConditionsFromInstructions(string[] lines)
    {
        int i;
        for (i = 0; lines[i].Length != 0; i++) { }

        string[] startConditions = new string[i - 1];
        Array.Copy(lines, 0, startConditions, 0, startConditions.Length);

        string[] instructions = new string[lines.Length - 1 - i];
        Array.Copy(lines, i + 1, instructions, 0, instructions.Length);

        return new string[][] { startConditions, instructions };
    }
    private Stack<char>[] ParseStartConditions(string[] conditions)
    {
        int n = (conditions[0].Length + 1) / 4;
        Stack<char>[] returnStacks = new Stack<char>[n];
        for (int i = 0; i < returnStacks.Length; i++)
        {
            returnStacks[i] = new Stack<char>();
        }

        for (int i = conditions.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < n; j++)
            {
                char symbol = conditions[i][1 + 4 * j];
                if (symbol != ' ')
                {
                    returnStacks[j].Push(symbol);
                }
            }
        }
        return returnStacks;
    }
    private int[][] ParseInstructions(string[] instructions)
    {
        int[][] returnInts = new int[instructions.Length][];

        for (int i = 0; i < instructions.Length; i++)
        {
            returnInts[i] = new int[3];
            string[] line = instructions[i].Split(" ");
            returnInts[i][0] = int.Parse(line[1]);
            returnInts[i][1] = int.Parse(line[3]);
            returnInts[i][2] = int.Parse(line[5]);
        }

        return returnInts;
    }
    private Stack<char>[] ExecuteInstructionsOneByOne(Stack<char>[] startConditions, int[][] instructions)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            for (int j = 0; j < instructions[i][0]; j++)
            {
                startConditions[instructions[i][2] - 1].Push(startConditions[instructions[i][1] - 1].Pop());
            }
        }
        return startConditions;
    }
    private Stack<char>[] ExecuteInstructionsEntireStack(Stack<char>[] startConditions, int[][] instructions)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            char[] tempPile = new char[instructions[i][0]];
            for (int j = 0; j < instructions[i][0]; j++)
            {
                tempPile[j] = startConditions[instructions[i][1] - 1].Pop();
            }
            for (int j = tempPile.Length - 1; j >= 0; j--)
            {
                startConditions[instructions[i][2] - 1].Push(tempPile[j]);
            }
        }
        return startConditions;
    }
    private Stack<char>[] ExecuteInstructions(Stack<char>[] startConditions, int[][] instructions, bool entireStack)
    {
        return entireStack ? ExecuteInstructionsEntireStack(startConditions, instructions) : ExecuteInstructionsOneByOne(startConditions, instructions);
    }

    private void DebugLogTopObjects(Stack<char>[] conditions)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < conditions.Length; i++)
        {
            sb.Append(conditions[i].Peek());
        }
        Debug.Log(sb.ToString());
    }
}
