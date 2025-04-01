using BepInEx;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Overrank
{
    [BepInPlugin("maranara_overrank", "OVERRANK", "1.0.0")]
    public class Overrank : BaseUnityPlugin
    {
        public static Harmony harmony;
        private void Start()
        {
            harmony = new Harmony("maranara_overrank");
            harmony.PatchAll(typeof(RankPatches));

            Database.Init();
        }
    }
}