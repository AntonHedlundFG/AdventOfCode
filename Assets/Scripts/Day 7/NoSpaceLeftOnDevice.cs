using System.Collections.Generic;
using UnityEngine;

public class NoSpaceLeftOnDevice : AdventOfCode
{
    protected override void PartOne()
    {
        Directory dir = GenerateDirectory();
        List<Directory> list = dir.AllSubDirectoriesOfSize(100000);
        Debug.Log(TotalDirectorySize(list));

    }
    protected override void PartTwo()
    {
        Directory dir = GenerateDirectory();
        int unusedSpace = 70000000 - dir.GetTotalSize();
        int reqSpace = 30000000 - unusedSpace;
        List<Directory> list = dir.AllSubDirectories();
        Directory smallestDir = FindSmallestDirectoryLargerThan(list, reqSpace);
        Debug.Log(smallestDir.GetTotalSize());
    }

    private class Directory
    {
        public List<Directory> subDirectories;
        public List<File> files;
        public Directory ParentDirectory;
        public string Name;

        public Directory(string name)
        {
            Name = name;
            subDirectories = new List<Directory>();
            files = new List<File>();
        }
        public Directory(string name, Directory parentDirectory)
        {
            Name = name;
            subDirectories = new List<Directory>();
            files = new List<File>();
            ParentDirectory = parentDirectory;
        }

        public int GetTotalSize()
        {
            int returnInt = 0;
            for (int i = 0; i < subDirectories.Count; i++)
            {
                returnInt += subDirectories[i].GetTotalSize();
            }
            for (int i = 0; i < files.Count; i++)
            {
                returnInt += files[i].FileSize;
            }
            return returnInt;
        }

        public Directory GetSubDirectory(string name) => subDirectories.Find(x => x.Name == name);
        public void ApplyListCommand(string line)
        {
            string[] splitLine = line.Split(" ");
            if (splitLine[0] == "dir")
            {
                subDirectories.Add(new Directory(splitLine[1], this));
                return;
            }
            files.Add(new File(int.Parse(splitLine[0]), splitLine[1]));
        }
        public List<Directory> AllSubDirectories()
        {
            List<Directory> returnList = new List<Directory>();
            returnList.Add(this);
            for (int i = 0; i < subDirectories.Count; i++)
            {
                List<Directory> subList = subDirectories[i].AllSubDirectories();
                for (int j = 0; j < subList.Count; j++)
                {
                    returnList.Add(subList[j]);
                }
            }
            return returnList;
        }

        public List<Directory> AllSubDirectoriesOfSize(int maxSize)
        {
            List<Directory> list = AllSubDirectories();
            List<Directory> returnList = new List<Directory>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetTotalSize() <= maxSize)
                {
                    returnList.Add(list[i]);
                }
            }
            return returnList;
        }

    }
    private struct File
    {
        public int FileSize;
        public string FileName;

        public File(int fileSize, string fileName)
        {
            FileSize = fileSize;
            FileName = fileName;
        }
    }
    private Directory GenerateDirectory()
    {
        Directory mainDir = new Directory("\\");
        Directory curDir = mainDir;
        string[] lines = ParseFile();

        int i = 0;
        while(i < lines.Length)
        {
            string[] cmd = lines[i].Split(" ");
            switch(cmd[1])
            {
                case "cd":
                    switch(cmd[2])
                    {
                        case "/":
                            curDir = mainDir;
                            break;
                        case "..":
                            curDir = curDir.ParentDirectory;
                            break;
                        default:
                            curDir = curDir.GetSubDirectory(cmd[2]);
                            break;
                    }
                    i++;
                    break;
                case "ls":
                    i++;
                    while (i < lines.Length && lines[i][0] != '$')
                    {
                        curDir.ApplyListCommand(lines[i]);
                        i++;
                    } 
                    break;
            }
        }

        return mainDir;
    }
    private int TotalDirectorySize(List<Directory> list)
    {
        int returnInt = 0;
        for (int i = 0; i < list.Count; i++)
        {
            returnInt += list[i].GetTotalSize();
        }
        return returnInt;
    }
    private Directory FindSmallestDirectoryLargerThan(List<Directory> list, int minSize)
    {
        Directory returnDir = null;
        int retSize = 0;
        for (int i = 0; i < list.Count; i++)
        {
            int curSize = list[i].GetTotalSize();
            if (curSize < minSize) { continue; }
            if (returnDir == null)
            {
                returnDir = list[i];
                retSize = curSize;
                continue;
            }
            if (retSize > curSize)
            {
                returnDir = list[i];
                retSize = curSize;
            }
        }
        return returnDir;
    }
}
