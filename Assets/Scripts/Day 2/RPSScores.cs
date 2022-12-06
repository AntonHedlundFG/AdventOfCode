using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPSScores : AdventOfCode
{
    protected override void PartOne()
    {
        Game[] games = GetGames();
        int score = GetScore(games);
        Debug.Log(score);
    }

    protected override void PartTwo()
    {
        Game[] games = GetGamesAlternate();
        int score = GetScore(games);
        Debug.Log(score);
    }

    struct Game
    {
        public RPS PlayerMove;
        public RPS OpponentMove;

        public Game(RPS pm, RPS om)
        {
            PlayerMove = pm;
            OpponentMove = om;
        }
    }

    private enum RPS
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private enum Result
    {
        Win = 1,
        Lose = -1,
        Draw = 0
    }


    private RPS StrategyFromResult(RPS om, Result res)
    {
        int omint = (int)om;
        int resint = (int)res;
        int pmint = omint + resint;
        
        if (pmint > 3) { pmint -= 3; }
        if (pmint < 1) { pmint += 3; }

        return (RPS)pmint;
    }

    private RPS StringToRPS(string n)
    {
        switch (n)
        {
            case "A":
            case "X":
                return RPS.Rock;
            case "B":
            case "Y":
                return RPS.Paper;
            case "C":
            case "Z":
            default:
                return RPS.Scissors;
        }
    }

    private Result StringToResult(string n)
    {
        switch (n)
        {
            case "X":
                return Result.Lose;
            case "Y":
                return Result.Draw;
            case "Z":
            default:
                return Result.Win;
        }
    }

    private int GetScore(Game game)
    {
        int score = ((int)game.PlayerMove);
        if (game.PlayerMove == game.OpponentMove)
        {
            score += 3;
            return score;
        }
        int dif = ((int)game.OpponentMove) - ((int)game.PlayerMove);
        if (dif == -1 || dif == 2)
        {
            score += 6;
        }
        return score;
    }

    private int GetScore(Game[] games)
    {
        int totalScore = 0;
        for (int i = 0; i < games.Length; i++)
        {
            totalScore += GetScore(games[i]);
        }
        return totalScore;
    }

    private Game[] GetGames()
    {
        string[] lines = ParseFile();
        Game[] games = new Game[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            RPS om = StringToRPS(lines[i].Substring(0, 1));
            RPS pm = StringToRPS(lines[i].Substring(2, 1));
            games[i] = new Game(pm, om);
        }
        return games;
    }

    private Game[] GetGamesAlternate()
    {
        string[] lines = ParseFile();
        Game[] games = new Game[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            RPS om = StringToRPS(lines[i].Substring(0, 1));
            Result res = StringToResult(lines[i].Substring(2, 1));
            RPS pm = StrategyFromResult(om, res);
            games[i] = new Game(pm, om);
        }

        return games;
    }
    
}
