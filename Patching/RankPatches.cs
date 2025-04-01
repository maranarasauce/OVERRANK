using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Overrank
{
    public static class RankPatches
    {
        private static Material defaultRankMaterial;
        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.Start))]
        [HarmonyPrefix]
        private static void InitRanks(StyleHUD __instance)
        {
            defaultRankMaterial = __instance.rankImage.material;
            __instance.ranks.AddRange(Database.Resource.ranks);
        }

        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.AddPoints))]
        [HarmonyPostfix]
        public static void UpRank(StyleHUD __instance)
        {
            if (__instance.currentMeter >= (float)__instance.currentRank.maxMeter && __instance.rankIndex < __instance.ranks.Count - 1)
            {
                __instance.AscendRank();
            }
        }

        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.AscendRank))]
        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.DescendRank))]
        [HarmonyPostfix]
        public static void ChangeRank(StyleHUD __instance)
        {
            if (__instance.rankIndex > 7)
            {
                __instance.rankImage.material = Database.Resource.rankMats[__instance.rankIndex - 8];
            } else
            {
                __instance.rankImage.material = defaultRankMaterial;
            }
        }

        const string CrackSizeKey = "_Crack_Size";
        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.UpdateHUD))]
        [HarmonyPostfix]
        public static void UpdateHUD(StyleHUD __instance)
        {
            if (__instance.rankIndex == 8)
            {
                Database.Resource.rankMats[0].SetFloat(CrackSizeKey, __instance.currentMeter / (float)__instance.currentRank.maxMeter);
            }
        }
    }
}
