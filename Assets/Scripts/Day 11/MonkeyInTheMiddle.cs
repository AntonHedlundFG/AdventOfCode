using System.Collections.Generic;
using UnityEngine;

public class MonkeyInTheMiddle : AdventOfCode
{
    private bool _divideByThree = false;
    protected override void PartOne()
    {
        _divideByThree = true;
        Monkey[] monkeys = GenerateMonkeys();
        RunRounds(ref monkeys, 20);
        ulong mb = CalculateMonkeyBusiness(monkeys);
        Debug.Log(mb);
    }

    protected override void PartTwo()
    {
        _divideByThree = false;
        Monkey[] monkeys = GenerateMonkeys();
        RunRounds(ref monkeys, 10000);
        ulong mb = CalculateMonkeyBusiness(monkeys);
        Debug.Log(mb);
    }


    private struct Monkey
    {
        public List<ulong> ItemList;
        public Operation Operation;
        public ulong Divisor;
        public int trueTarget;
        public int falseTarget;
        public ulong throwAmount;
    }

    private struct Operation
    {
        public Operator Operator;
        public int? A;
        public int? B;
    }
    private enum Operator
    {
        Add,
        Multiply
    }

    private Monkey[] GenerateMonkeys()
    {
        string[] lines = ParseFile();
        Monkey[] monkeys = new Monkey[lines.Length / 6];

        for (int i = 0; i < (lines.Length / 6); i++)
        {
            string[] monkeyLines = new string[5];
            for (int j = 1; j < 6; j++)
            {
                monkeyLines[j - 1] = lines[(i * 6) + j];
            }

            monkeys[i] = GenerateMonkey(monkeyLines);
        }

        return monkeys;
    }

    private Monkey GenerateMonkey(string[] lines)
    {
        Monkey monkey = new Monkey();
        monkey.ItemList = ParseStartingItems(lines[0]);
        monkey.Operation = ParseOperation(lines[1]);
        string[] testLines = new string[] { lines[2], lines[3], lines[4] };
        ParseTest(testLines, ref monkey);
        monkey.throwAmount = 0;

        return monkey;
    }

    private void ParseTest(string[] testLines, ref Monkey monkey)
    {
        monkey.Divisor = ulong.Parse(TrimLine(testLines[0])[3]);
        monkey.trueTarget = int.Parse(TrimLine(testLines[1])[5]);
        monkey.falseTarget = int.Parse(TrimLine(testLines[2])[5]);
    }

    List<ulong> ParseStartingItems(string line)
    {
        List<ulong> returnList = new List<ulong>();

        string[] words = TrimLine(line);

        for (int i = 2; i < words.Length; i++)
        {
            returnList.Add(ulong.Parse(words[i]));
        }

        return returnList;
    }

    private Operation ParseOperation(string line)
    {
        Operation op = new Operation();
        string[] words = TrimLine(line);

        op.Operator = words[4] == "+" ? Operator.Add : Operator.Multiply;

        op.A = int.TryParse(words[3], out int a) ? a : null;
        op.A = int.TryParse(words[5], out int b) ? b : null;

        return op;
    }

    private string[] TrimLine(string line)
    {
        line = line.TrimStart(' ');
        string[] words = line.Split(" ");
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].TrimEnd(',');
        }

        return words;
    }


    private void PerformMonkeyThrows(ref Monkey[] monkeys, int throwerIndex)
    {
        ref Monkey monkey = ref monkeys[throwerIndex];
        while (monkey.ItemList.Count != 0)
        {
            ulong itemValue = monkey.ItemList[0];
            monkey.ItemList.RemoveAt(0);
            monkey.throwAmount++;

            itemValue = InspectItem(monkey.Operation, itemValue);

            if (_divideByThree) { itemValue = itemValue / 3; }
            itemValue = itemValue % LeastMonkeyDivisor(monkeys);


            int targetMonkey = (itemValue % monkey.Divisor == 0)
                ? monkey.trueTarget
                : monkey.falseTarget;
            monkeys[targetMonkey].ItemList.Add(itemValue);
        }
    }

    private ulong InspectItem(Operation op, ulong startValue)
    {
        ulong a = (op.A == null) ? startValue : (ulong)op.A;
        ulong b = (op.B == null) ? startValue : (ulong)op.B;
        switch (op.Operator)
        {
            case Operator.Add:
                return a + b;
            case Operator.Multiply:
                return a * b;
        }
        return 0;
    }

    private void RunRounds(ref Monkey[] monkeys, int rounds)
    {
        for (int i = 0; i < rounds; i++)
        {
            for (int j = 0; j < monkeys.Length; j++)
            {
                PerformMonkeyThrows(ref monkeys, j);
            }
        }
    }

    private ulong CalculateMonkeyBusiness(Monkey[] monkeys)
    {
        int a = -1, b = -1;
        for (int i = 0; i < monkeys.Length; i++)
        {
            if (a == -1 || monkeys[i].throwAmount > monkeys[a].throwAmount)
            {
                b = a;
                a = i;
            }
            else if (b == -1 || monkeys[i].throwAmount > monkeys[b].throwAmount)
            {
                b = i;
            }
        }

        return monkeys[a].throwAmount * monkeys[b].throwAmount;
    }

    private ulong LeastMonkeyDivisor(Monkey[] monkeys)
    {
        ulong a = 1;
        foreach (Monkey monkey in monkeys)
        {
            a = a * monkey.Divisor;
        }

        return a;
    }
}