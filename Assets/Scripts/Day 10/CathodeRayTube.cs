using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class CathodeRayTube : AdventOfCode
{
    protected override void PartOne()
    {
        Instruction[] instructions = ParseInstructions();
        List<int> positions = new List<int>(){20, 60, 100, 140, 180, 220};
        List<int> strengths = CalcValuesAtSpecificCycles(instructions, positions);
        int total = 0;
        for (int i = 0; i < strengths.Count; i++)
        {
            Debug.Log(positions[i] + ": " + strengths[i]);
            total += strengths[i];
        }
        Debug.Log("Total: " + total);
    }

    protected override void PartTwo()
    {
        Instruction[] instructions = ParseInstructions();
        List<int> values = CalcValuesAtAllCycles(instructions);

        bool[] picture = GeneratePicture(values);
        DrawPicture(picture);
    }

    private enum InstructionType
    {
        Add,
        None
    }

    private struct Instruction
    {
        public InstructionType Type;
        public int Value;
        public Instruction(InstructionType type, int value)
        {
            Type = type;
            Value = value;
        }
        public Instruction(InstructionType type)
        {
            Type = type;
            Value = 0;
        }
    }

    private Instruction[] ParseInstructions()
    {
        string[] lines = ParseFile();
        Instruction[] instr = new Instruction[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            switch (lines[i][0])
            {
                case 'n':
                    instr[i] = new Instruction(InstructionType.None);
                    break;
                case 'a':
                default:
                    string[] line = lines[i].Split(" ");
                    instr[i] = new Instruction(InstructionType.Add, int.Parse((line[1])));
                    break;
            }
        }
        return instr;
    }
    
    private List<int> CalcValuesAtAllCycles(Instruction[] instructions)
    {
        List<int> returnList = new List<int>();
        int value = 1;
        for (int i = 0; i < instructions.Length; i++)
        {
            switch (instructions[i].Type)
            {
                case InstructionType.None:
                    returnList.Add(value);
                    break;
                case InstructionType.Add:
                    returnList.Add(value);
                    returnList.Add(value);
                    value += instructions[i].Value;
                    break;
            }
        }
        return returnList;
    }

    private List<int> CalcValuesAtSpecificCycles(Instruction[] instructions, List<int> cycles)
    {
        List<int> returnList = new List<int>();
        List<int> values = CalcValuesAtAllCycles(instructions);
        for (int i = 0; i < cycles.Count; i++)
        {
            returnList.Add(values[cycles[i] - 1] * cycles[i]);
        }

        return returnList;
    }

    private bool[] GeneratePicture(List<int> values)
    {
        bool[] returnArray = new bool[values.Count];

        for (int i = 0; i < values.Count; i++)
        {
            returnArray[i] = (values[i] >= (i - 1) % 40 && values[i] <= (i + 1) % 40);
        }

        return returnArray;
    }

    private void DrawPicture(bool[] picture)
    {
        for (int i = 0; i < 6; i++)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < 40; j++)
            {
                
                switch (picture[j + (i * 40)])
                {
                    case false:
                        sb.Append('.');
                        break;
                    case true:
                        sb.Append('#');
                        break;
                }
                
            }
            Debug.Log(sb.ToString());
        }
    }
}
