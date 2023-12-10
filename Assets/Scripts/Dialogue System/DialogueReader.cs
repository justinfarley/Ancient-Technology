using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Reads the Scripts provided in the assets from csv <para></para>
/// </summary>
public static class DialogueReader
{
    private const string filePath = "./Assets/Dialogue.csv";

    private static Dictionary<string, int> sceneToIndexPairs = new Dictionary<string, int>()
    {
        { "Introduction", 0 },
    };
    public class Pair
    {
        private readonly char x;
        private readonly string[] y;
        private readonly int index;
        public Pair(int index, char x, string[] y)
        {
            this.y = new string[y.Length];
            this.x = x;
            this.index = index;
            for (int i = 0; i < y.Length; i++)
            {
                this.y[i] = y[i];
            }
        }

        public bool EqualTo(Pair obj)
        {
            if(GetX().Equals(obj.GetX()))
            {
                for(int i = 0; i < obj.y.Length; i++)
                {
                    if (!obj.y[i].Equals(y[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public char GetX()
        {
            return x;
        }
        public string[] GetY()
        {
            return y;
        }
        public int GetIndex()
        {
            return index;
        }
        public override string ToString()
        {
            return GetX() + ", " + GetY();
        }
    }
    public static Dictionary<int, Pair> GetScripts()
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamReader sr = new StreamReader(stream);
        Dictionary<int, Pair> ret = new Dictionary<int, Pair>();
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            string[] parts = line.Split("//");
            parts[0] = parts[0].Replace(".", ".#");
            parts[0] = parts[0].Replace("?", "?#");
            parts[0] = parts[0].Replace("!", "!#");
            parts[0] = parts[0].Replace(";", ";#");
            parts[0] = parts[0].Replace(":", ":#");
            string[] scriptParts = Regex.Split(parts[0], "#");
            Pair pair = new Pair(int.Parse(parts[1][^1] + ""), parts[1][0], scriptParts);
            Debug.Log(pair);
            ret.Add(int.Parse(parts[1][^1] + ""), pair);
        }
        return ret;
    }
    public static Pair GetScript(int index)
    {
        Dictionary<int, Pair> scripts = GetScripts();
        scripts.TryGetValue(index, out Pair n);
        return n;
    }
    /// <summary>
    /// SEE <see cref="sceneToIndexPairs"/> FOR WHAT STRINGS TO USE and add them
    /// </summary>
    /// <param name="scene">the name</param>
    /// <returns>the script</returns>
    public static Pair GetScript(string scene)
    {
        Dictionary<int, Pair> scripts = GetScripts();
        sceneToIndexPairs.TryGetValue(scene, out int index);

        return scripts[index];
    }
}

