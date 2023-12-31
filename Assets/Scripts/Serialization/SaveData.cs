﻿using System;
using UnityEngine;

namespace Ltg8
{
    public class SaveData
    {
        public Vector3 PlayerPos;
        public int PlayerSceneId;

        public bool[] Flags = new bool[Enum.GetValues(typeof(Flag)).Length];
        public void SetFlag(Flag flag) => Flags[(int) flag] = true;
        public void ResetFlag(Flag flag) => Flags[(int) flag] = false;
        public bool IsFlagSet(Flag flag) => Flags[(int) flag];

        public int[] Vars = new int[Enum.GetValues(typeof(Var)).Length];
        public int GetVar(Var var) => Vars[(int) var];
        public void SetVar(Var var, int value) => Vars[(int)var] = value;
    }
}
