using System.Collections.Generic;
using UnityEngine;

public class HillClimbing : AdventOfCode
{
    protected override void PartOne()
    {
        Vector2Int startPos, endPos;
        int[][] intGrid = GenerateIntGrid(out startPos, out endPos);
        Node[][] grid = GenerateNodeGrid(intGrid);
        List<Node> path = FindPath(grid, startPos, endPos);
        Debug.Log(path.Count);
    }
    protected override void PartTwo()
    {
        Vector2Int startPos, endPos;
        int[][] intGrid = GenerateIntGrid(out startPos, out endPos);
        Node[][] grid = GenerateNodeGrid(intGrid);
        int bestRoute = FindShortestScenicRoute(grid, endPos);
        Debug.Log(bestRoute);
    }

    private int FindShortestScenicRoute(Node[][] grid, Vector2Int endPos)
    {
        int length = int.MaxValue;

        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j].Height == 1)
                {
                    List<Node> curPath = FindPath(grid, new Vector2Int(i, j), endPos);
                    if (curPath != null && curPath.Count < length)
                    {
                        length = curPath.Count;
                    }
                }
            }
        }

        return length;
    }
    private int[][] GenerateIntGrid(out Vector2Int startPos, out Vector2Int endPos)
    {
        string[] lines = ParseFile();
        int[][] grid = new int[lines.Length][];

        Vector2Int rStartPos = new Vector2Int(-1, -1);
        Vector2Int rEndPos = new Vector2Int(-1, -1);

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new int[lines[i].Length];
            for (int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j] = (int)(char.ToUpper(lines[i][j])) - 64;

                if (lines[i][j] == 'S')
                {
                    rStartPos = new Vector2Int(i, j);
                    grid[i][j] = 1;
                }
                if (lines[i][j] == 'E')
                {
                    rEndPos = new Vector2Int(i, j);
                    grid[i][j] = 26;
                }

            }
        }

        startPos = rStartPos;
        endPos = rEndPos;

        return grid;
    }
    private Node[][] GenerateNodeGrid(int[][] intGrid)
    {
        Node[][] grid = new Node[intGrid.Length][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new Node[intGrid[i].Length];
            for (int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j] = new Node(intGrid[i][j], i, j);
            }
        }
        return grid;
    }

    private List<Node> FindPath(Node[][] grid, Vector2Int startPos, Vector2Int endPos)
    {
        HashSet<Node> visitedNodes = new HashSet<Node>();

        Node startNode = grid[startPos.x][startPos.y];
        Node endNode = grid[endPos.x][endPos.y];

        ClearPaths(grid);

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(startNode);
        visitedNodes.Add(startNode);

        while (queue.Count > 0)
        {
            Node curNode = queue.Dequeue();

            if (curNode.Equals(endNode)) { return curNode.Path; }

            List<Node> neighbours = GetNeighbours(grid, curNode);
            foreach (Node neighbour in neighbours)
            {
                if (visitedNodes.Contains(neighbour) || !IsVisitable(curNode, neighbour)) { continue; }
                neighbour.Path.AddRange(curNode.Path);
                neighbour.Path.Add(curNode);
                queue.Enqueue(neighbour);
                visitedNodes.Add(neighbour);
            }
        }

        return null;
    }
    private struct Node
    {
        public int Height;
        public List<Node> Path;
        public int X;
        public int Y;
        public Node(int height, int x, int y)
        {
            Height = height;
            Path = new List<Node>();
            X = x;
            Y = y;
        }
        public void ClearPaths() { Path.Clear(); }

    }
    
    private List<Node> GetNeighbours(Node[][] grid, int x, int y)
    {
        List<Node> neighbours = new List<Node>();
        if (x > 0) { neighbours.Add(grid[x - 1][y]); }
        if (y > 0) { neighbours.Add(grid[x][y - 1]); }
        if (x < grid.Length - 1) { neighbours.Add(grid[x + 1][y]); }
        if (y < grid[x].Length - 1) { neighbours.Add(grid[x][y + 1]); }

        return neighbours;
    }
    private List<Node> GetNeighbours(Node[][] grid, Node node)
    {
        return GetNeighbours(grid, node.X, node.Y);
    }
    private bool IsVisitable(Node from, Node to) => to.Height <= from.Height + 1;

    private void ClearPaths(Node[][] grid)
    {
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j].ClearPaths();
            }
        }
    }

}
