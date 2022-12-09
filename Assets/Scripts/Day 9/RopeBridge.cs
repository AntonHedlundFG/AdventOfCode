using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBridge : AdventOfCode
{
    protected override void PartOne()
    {
        Debug.Log(CalculateTailTouchedTiles());
    }
    protected override void PartTwo()
    {
        Debug.Log(CalculateTailTouchedTilesWithBody(10));
    }
    private struct Instruction
    {
        public Direction direction;
        public int steps;
        public Instruction(Direction dir, int s)
        {
            steps = s;
            direction = dir;
        }
    }
    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
    private Instruction[] ParseInstructions()
    {
        string[] lines = ParseFile();
        Instruction[] instructions = new Instruction[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(" ");
            char c = line[0][0];
            int n = int.Parse(line[1]);
            instructions[i] = new Instruction(DirFromChar(c), n);
        }
        return instructions;
    }
    private Direction DirFromChar(char c)
    {
        switch (c)
        {
            case 'U':
                return Direction.Up;
            case 'D':
                return Direction.Down;
            case 'R':
                return Direction.Right;
            case 'L':
            default:
                return Direction.Left;
        }
    }
    private int CalculateTailTouchedTiles()
    {
        Vector2Int tailTile = new Vector2Int(0, 0);
        Vector2Int headTile = new Vector2Int(0, 0);
        HashSet<Vector2Int> visitedSet = new HashSet<Vector2Int>();
        visitedSet.Add(tailTile);
        Instruction[] instructions = ParseInstructions();

        for (int i = 0; i < instructions.Length; i++)
        {
            for (int j = 0; j < instructions[i].steps; j++)
            {
                headTile = NewPositionFromDirection(headTile, instructions[i].direction);
                tailTile = UpdateTailTileFromHeadMovement(tailTile, headTile);
                visitedSet.Add(tailTile);
            }
        }

        return visitedSet.Count;
    }
    private int CalculateTailTouchedTilesWithBody(int bodySize)
    {
        Vector2Int[] body = new Vector2Int[bodySize];
        for (int i = 0; i < bodySize; i++)
        {
            body[i] = new Vector2Int(0, 0);
        }
        HashSet<Vector2Int> visitedSet = new HashSet<Vector2Int>();
        Instruction[] instructions = ParseInstructions();

        for (int i = 0; i < instructions.Length; i++) //Iterate through all input lines.
        {
            for (int j = 0; j < instructions[i].steps; j++) //Repeat instruction based on the line's input.
            {
                body[0] = NewPositionFromDirection(body[0], instructions[i].direction); //Move the head.
                for (int k = 1; k < body.Length; k++) //Move the entire body and tail to follow the head.
                {
                    body[k] = UpdateTailTileFromHeadMovement(body[k], body[k - 1]);
                }
                visitedSet.Add(body[body.Length - 1]); //Mark tail position.
            }
        }

        return visitedSet.Count;
    }
    Vector2Int NewPositionFromDirection(Vector2Int startPos, Direction dir)
    {
        int x = startPos.x, y = startPos.y;
        switch (dir)
        {
            case Direction.Up:
                y++;
                break;
            case Direction.Down:
                y--;
                break;
            case Direction.Right:
                x++;
                break;
            case Direction.Left:
            default:
                x--;
                break;
        }
        return new Vector2Int(x, y);
    }
    Vector2Int UpdateTailTileFromHeadMovement(Vector2Int tailTile, Vector2Int headTile)
    {
        int x = tailTile.x;
        int y = tailTile.y;
        int xDelta = headTile.x - x;
        int yDelta = headTile.y - y;

        if ((Math.Abs(xDelta) == 2 && Math.Abs(yDelta) >= 1) || (Math.Abs(yDelta) == 2 && Math.Abs(xDelta) >= 1))
        {
            x += 1 * (Math.Sign(xDelta));
            y += 1 * (Math.Sign(yDelta));
            return new Vector2Int(x, y);
        }
        if (Math.Abs(xDelta) == 2)
        {
            x += 1 * (Math.Sign(xDelta));
            return new Vector2Int(x, y);
        }
        if (Math.Abs(yDelta) == 2)
        {
            y += 1 * (Math.Sign(yDelta));
            return new Vector2Int(x, y);
        }
        return tailTile;
    }
}
