﻿using BepInEx;
using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.InputSystem;
using BepInEx.Logging;

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

            src = BepInEx.Logging.Logger.CreateLogSource("overrank");

            Database.Init();
        }

        private void Update()
        {
            if (Keyboard.current.digit7Key.wasPressedThisFrame)
            {
                StyleHUD.Instance.AddPoints(600, string.Empty);
            }
            if (Keyboard.current.digit8Key.wasPressedThisFrame)
            {
                StyleHUD.Instance.AddPoints(-600, string.Empty);
            }
        }

        static ManualLogSource src;
        public static void Log(string lol)
        {
            src.Log(LogLevel.Info, lol);
        }
    }
}