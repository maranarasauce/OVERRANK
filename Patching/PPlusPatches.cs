using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overrank.Patching
{
    public static class PPlusPatches
    {
        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.Start))]
        [HarmonyPrefix]
        private static void AttachUPlusTracker(StyleHUD __instance)
        {
            __instance.gameObject.AddComponent<PPlusTracker>();
        }

        [HarmonyPatch(typeof(LevelStats), nameof(LevelStats.Start))]
        [HarmonyPrefix]
        private static void WidenLevelStats(LevelStats __instance)
        {
            __instance.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 345f);

            GameObject uiInst = GameObject.Instantiate(Database.Resource.requirementUI);
            uiInst.transform.SetParent(__instance.transform, true);
            RectTransform rect = uiInst.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(10f, -310f);
            rect.sizeDelta = new Vector2(280, 30f);
            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;
            PPlusTracker.instance.SetUI(uiInst);
        }

        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.Awake))]
        [HarmonyPrefix]
        private static void AddPPlusStyleRequirement(StatsManager __instance)
        {
            __instance.killRanks = __instance.killRanks.AddItem(__instance.killRanks[__instance.killRanks.Length - 1] + 100000).ToArray();

            int style = __instance.styleRanks[__instance.styleRanks.Length - 1];
            style = (int)(style * Database.Resource.styleRequirementMod);
            __instance.styleRanks = __instance.styleRanks.AddItem(style).ToArray();
        }

        [HarmonyPatch(typeof(RankHelper), nameof(RankHelper.GetRankLetter))]
        [HarmonyPrefix]
        private static bool GetLetter(int rank, ref string __result)
        {
            if (rank == 7)
            {
                __result = Database.Resource.ppRankName;
                return true;
            }
            return false;
        }

        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.GetRanks))]
        [HarmonyPrefix]
        private static bool GetRanks(StatsManager __instance, ref string __result, int[] ranksToCheck, float value, bool reverse, bool addToRankScore = false)
        {
            int num = 0;
            bool flag = true;
            while (flag)
            {
                if (num < ranksToCheck.Length)
                {
                    if ((reverse && value <= (float)ranksToCheck[num]) || (!reverse && value >= (float)ranksToCheck[num]))
                    {
                        num++;
                        continue;
                    }

                    if (addToRankScore)
                    {
                        __instance.rankScore += num;
                    }

                    switch (num)
                    {
                        case 0:
                            __result = "<color=#0094FF>D</color>";
                            return false;
                        case 1:
                            __result = "<color=#4CFF00>C</color>";
                            return false;
                        case 2:
                            __result = "<color=#FFD800>B</color>";
                            return false;
                        case 3:
                            __result = "<color=#FF6A00>A</color>";
                            return false;
                        case 4:
                            __result = "<color=#FF0000>S</color>";
                            return false;
                    }

                    continue;
                }

                if (addToRankScore)
                {
                    __instance.rankScore += 4;
                }

                if (reverse)
                    __result = "<color=#FF0000>S</color>";
                else __result = $"<color={Database.Resource.ppRankHex}>{Database.Resource.ppRankName}</color>";
                return false;
            }

            __result = "X";
            return false;
        }

        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.GetFinalRank))]
        [HarmonyPostfix]
        private static void GetFinalRank(StatsManager __instance)
        {
            
            if (__instance.rankScore == 12 && !__instance.asscon.cheatsEnabled && PPlusTracker.Instance.AchievedUPlus())
            {
                string text = $"<color=#FFFFFF>{Database.Resource.ppRankName}</color>";
                __instance.fr.totalRank.transform.parent.GetComponent<Image>().color = Database.Resource.ppRankColor;
                __instance.fr.totalRank.fontSize = 200;
                __instance.fr.SetRank(text);
            }
            
        }

        [HarmonyPatch(typeof(RankHelper), nameof(RankHelper.GetRankBackgroundColor))]
        [HarmonyPrefix]
        private static bool GetBackgroundColor(int rank, ref Color __result)
        {
            if (rank == 7)
            {
                __result = Database.Resource.ppRankColor;
                return true;
            }
            return false;
        }

        [HarmonyPatch(typeof(RankHelper), nameof(RankHelper.GetRankForegroundColor))]
        [HarmonyPrefix]
        private static bool GetForegroundColor(int rank, ref string __result)
        {
            if (rank == 7)
            {
                __result = $"#FFFFFF";
                return true;
            }
            return false;
        }
    }
}
