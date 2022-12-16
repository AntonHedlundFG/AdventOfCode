using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistressSignal : AdventOfCode
{
    protected override void PartOne()
    {
        Packet[] packets = GetPackets();
        int value = ComparePacketPairs(packets);
        Debug.Log(value);
    }
    protected override void PartTwo()
    {
        
    }

    private class Packet
    {
        public bool IsList;
        public int Value;
        public List<Packet> List;

        public Packet(int value)
        {
            this.Value = value;
            IsList = false;
        }
        public Packet(List<Packet> packets)
        {
            this.List = packets;
            IsList = true;
        }

        public Packet(int value, int amount)
        {
            List = new List<Packet>();
            for (int i = 0; i < amount; i++)
            {
                List.Add(new Packet(value));
            }
            IsList = true;
        }
    }

    private Packet GetPacketFromLine(string line)
    {
        string newLine = line.Remove(0, 1);
        return GetPacketRecursion(ref newLine);
    }
    private Packet GetPacketRecursion(ref string line)
    {
        List<Packet> list = new List<Packet>();
        int a;
        while (line.Length > 0)
        {
            switch (line[0])
            {
                case '[':
                    line = line.Remove(0, 1);
                    list.Add(GetPacketRecursion(ref line));
                    break;
                case ']':
                    line = line.Remove(0, 1);
                    return new Packet(list);
                case ',':
                    line = line.Remove(0, 1);
                    break;
                default:
                    a = 0;
                    int parsed;
                    while (int.TryParse(line.Substring(0, 1), out parsed))
                    {
                        a = a * 10 + parsed;
                        line = line.Remove(0, 1);
                    }
                    list.Add(new Packet(a));
                    break;
            }
        }
        Debug.Log("Should not be here");
        return null;
    } 
    
    private Packet[] GetPackets()
    {
        string[] lines = ParseFile();
        Packet[] packets = new Packet[lines.Length / 3 * 2];
        
        for (int i = 0; i < lines.Length / 3; i++)
        {
            packets[i * 2] = GetPacketFromLine(lines[i * 3]);
            packets[i * 2 + 1] = GetPacketFromLine(lines[i * 3] + 1);
        }

        return packets;
    }

    private bool ComparePackets(Packet a, Packet b)
    {
        if (!(a.IsList || b.IsList)) 
        {
            return a.Value <= b.Value; 
        }

        if (a.IsList && b.IsList)
        {
            int i = 0;
            while (i < a.List.Count && i < b.List.Count)
            {
                if (!ComparePackets(a.List[i], b.List[i])) { return false; }
                i++;
            }
            if (a.List.Count > i) { return false; }
            return true;
        }
        
        
        if (a.IsList)
        {
            b = new Packet(b.Value, a.List.Count);
        } else
        {
            a = new Packet(a.Value, b.List.Count);
        }
        return ComparePackets(a, b);
    }

    private int ComparePacketPairs(Packet[] packets)
    {
        int result = 0;

        for (int i = 0; i < packets.Length/2; i++)
        {
            if (ComparePackets(packets[i * 2], packets[i * 2 + 1]))
            {
                result += i;
            }
        }
        return result;
    }

    private void DebugLogPacket(Packet packet)
    {
        if(!packet.IsList)
        {
            Debug.Log(packet.Value);
            return;
        }
        Debug.Log("[");
        for (int i = 0; i < packet.List.Count; i++)
        {
            DebugLogPacket(packet.List[i]);
            if (i < packet.List.Count - 1) { Debug.Log(","); }
        }
        Debug.Log("]");
    }
}
