using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventOfCode : MonoBehaviour
{
    [SerializeField] private TextAsset _inputFile;
    protected string[] ParseFile() => _inputFile?.text.Split("\n");

    [ContextMenu("PartOne()")]
    protected void RunPartOne()
    {
        PartOne();
    }
    [ContextMenu("PartTwo()")]
    protected void RunPartTwo()
    {
        PartTwo();
    }

    protected virtual void PartOne() { }
    protected virtual void PartTwo() { }

}
