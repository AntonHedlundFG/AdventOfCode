using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BeaconExclusionZone : AdventOfCode
{
    [SerializeField] private int row = 10;
    protected override void PartOne()
    {
        SensorPair[] pairs = GetSensorPairs();
        HashSet<Vector2Int> blocked = BlockedPositionsInRow(pairs, row);
        blocked = ClearBeaconsFromBlockedPositions(blocked, pairs);
        Debug.Log(blocked.Count);
    }
    protected override void PartTwo()
    {
        SensorPair[] pairs = GetSensorPairs();
        int maxPos = 4000000;

        for (int i = 0; i < maxPos; i++)
        {
            for (int j = 0; j < maxPos; j++)
            {
                if (TestBeaconPosition(new Vector2Int(i, j), pairs))
                {
                    Debug.Log(i + ", " + j);
                }
            }
        }

    }

    private struct SensorPair
    {
        public Vector2Int Sensor;
        public Vector2Int Beacon;
        public int ManDist { get; private set; }

        public SensorPair(Vector2Int sensor, Vector2Int beacon)
        {
            Sensor = sensor;
            Beacon = beacon;
            ManDist = ManhattanDistance(Sensor, Beacon);
        }
    }

    private SensorPair[] GetSensorPairs()
    {
        string[] lines = ParseFile();
        SensorPair[] pairs = new SensorPair[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            pairs[i] = PositionsFromString(lines[i]);
        }

        return pairs;
    }

    private SensorPair PositionsFromString(string line)
    {
        

        int a = RemoveNextDigit(line, out line);
        int b = RemoveNextDigit(line, out line);
        int c = RemoveNextDigit(line, out line);
        int d = RemoveNextDigit(line, out line);

        SensorPair pair = new SensorPair(new Vector2Int(a, b), new Vector2Int(c, d));
        return pair;
    }
    private int RemoveNextDigit(string line, out string cutLine)
    {
        string n = new string(line
            .SkipWhile(c => !char.IsDigit(c) && c != '-')
            .TakeWhile(c => char.IsDigit(c) || c == '-').ToArray());
        cutLine = new string(line
            .SkipWhile(c => !char.IsDigit(c) && c != '-')
            .SkipWhile(c => char.IsDigit(c) || c == '-').ToArray());
        return int.Parse(n);
    }

    private static int ManhattanDistance(Vector2Int from, Vector2Int to) => Math.Abs(to.x - from.x) + Math.Abs(to.y - from.y);

    private HashSet<Vector2Int> BlockedPositionsInRow(SensorPair[] pairs, int row)
    {
        HashSet<Vector2Int> blocked = new HashSet<Vector2Int>();

        for (int i = 0; i < pairs.Length; i++)
        {
            blocked.UnionWith(BlockedPositionsInRow(pairs[i], row));
        }

        return blocked;
    }
    private HashSet<Vector2Int> BlockedPositionsInRow(SensorPair pair, int row)
    {
        HashSet<Vector2Int> set = new HashSet<Vector2Int>();

        Vector2Int sensor = pair.Sensor;
        int manDist = pair.ManDist;

        int i = 0;
        while (Math.Abs(sensor.y - row) + i <= manDist)
        {
            set.Add(new Vector2Int(sensor.x + i, row));
            set.Add(new Vector2Int(sensor.x - i, row));
            i++;
        }

        return set;
    }

    private HashSet<Vector2Int> ClearBeaconsFromBlockedPositions(HashSet<Vector2Int> blocked, SensorPair[] pairs)
    {
        for (int i = 0; i < pairs.Length; i++)
        {
            blocked.Remove(pairs[i].Beacon);
        }
        return blocked;
    }

    private bool TestBeaconPosition(Vector2Int position, SensorPair[] pairs)
    {
        for (int i = 0; i < pairs.Length; i++)
        {
            if (ManhattanDistance(position, pairs[i].Sensor) <= pairs[i].ManDist) { return false; }
        }
        return true;
    }

}
