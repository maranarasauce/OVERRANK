using HarmonyLib;
using Overrank.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Overrank.Patching
{
    public static class PPlusPatches
    {
        [HarmonyPatch(typeof(GameProgressSaver), nameof(GameProgressSaver.SetSlot))]
        [HarmonyPostfix]
        private static void SetSlot()
        {
            Progress.SlotSwitched();
        }

        [HarmonyPatch(typeof(LevelSelectPanel), nameof(LevelSelectPanel.CheckScore))]
        [HarmonyPostfix]
        private static void LevelSelectPanelPP(LevelSelectPanel __instance)
        {
            int num = __instance.levelNumber;
            if (__instance.levelNumber == 666 || __instance.levelNumber == 100)
            {
                num += __instance.levelNumberInLayer - 1;
            }

            bool hasPP = Progress.HasPP(num);
            TMP_Text componentInChildren = __instance.transform.Find("Stats").Find("Rank").GetComponentInChildren<TMP_Text>();
            if (hasPP)
            {
                PPLayerSelect.AttachLink(__instance.ls).GivePP();
                componentInChildren.text = $"<color=#FFFFFF>{Database.Resource.ppRankName}</color>";
                componentInChildren.fontSize = 40;
                Image component = componentInChildren.transform.parent.GetComponent<Image>();
                component.color = Database.Resource.ppRankColor;
                component.sprite = __instance.filledPanel;
            } else componentInChildren.fontSize = 60;

            RankData rank = GameProgressSaver.GetRank(num);
            int difficulty = PrefsManager.Instance.GetInt("difficulty");
            if (rank.ranks[difficulty] == 12 && (rank.challenge || !__instance.challengeIcon) && (__instance.allSecrets || rank.secretsAmount == 0))
            {
                if (hasPP)
                {
                    Color ube = Color.Lerp(Database.Resource.ppRankColor, Color.white, 0.5f); __instance.GetComponent<Image>().color = ube;
                    if (__instance.challengeIcon)
                    {
                        TMP_Text componentInChildren2 = __instance.challengeIcon.GetComponentInChildren<TMP_Text>();
                        
                        componentInChildren2.color = ube;
                    }
                } else
                {
                    __instance.GetComponent<Image>().color = new Color(1f, 0.686f, 0f, 0.75f);
                }
                
            }
            else
            {
                __instance.GetComponent<Image>().color = __instance.defaultColor;
            }
        }

        [HarmonyPatch(typeof(ChapterSelectButton), nameof(ChapterSelectButton.Awake))]
        [HarmonyPrefix]
        private static void ChapterSelectEnable(ChapterSelectButton __instance)
        {
            PPChapterSelect.AttachLink(__instance);
        }

        [HarmonyPatch(typeof(LayerSelect), nameof(LayerSelect.CheckScore))]
        [HarmonyPrefix]
        private static void ChapterSelectFix(LayerSelect __instance)
        {
            PPLayerSelect.AttachLink(__instance).CheckScores();
        }

        [HarmonyPatch(typeof(ChapterSelectButton), nameof(ChapterSelectButton.CheckScore))]
        [HarmonyPostfix]
        private static void ChapterSelectFix(ChapterSelectButton __instance)
        {
            PPChapterSelect.AttachLink(__instance).CheckPP();
        }

        [HarmonyPatch(typeof(ChapterSelectButton), nameof(ChapterSelectButton.OnDisable))]
        [HarmonyPostfix]
        private static void ChapterSelectDisable(ChapterSelectButton __instance)
        {
            PPChapterSelect.GetLink(__instance)?.OnDisable();
        }

        [HarmonyPatch(typeof(LayerSelect), nameof(LayerSelect.OnDisable))]
        [HarmonyPrefix]
        private static void LayerSelectDisable(LayerSelect __instance)
        {
            PPLayerSelect.GetLink(__instance)?.OnDisable();
        }

        [HarmonyPatch(typeof(LayerSelect), nameof(LayerSelect.Gold))]
        [HarmonyPostfix]
        private static void LayerSelectCheck(LayerSelect __instance)
        {
            PPLayerSelect.GetLink(__instance)?.CheckPP();
        }

        [HarmonyPatch(typeof(LayerSelect), nameof(LayerSelect.SecretMissionDone))]
        [HarmonyPostfix]
        private static void LayerSelectCheck2(LayerSelect __instance)
        {
            PPLayerSelect.GetLink(__instance)?.CheckPP();
        }

        [HarmonyPatch(typeof(LayerSelect), nameof(LayerSelect.AddScore))]
        [HarmonyPostfix]
        private static void LayerSelectCheck3(LayerSelect __instance)
        {
            PPLayerSelect.GetLink(__instance)?.CheckPP();
        }

        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.Start))]
        [HarmonyPrefix]
        private static void AttachUPlusTracker(StyleHUD __instance)
        {
            if (SceneHelper.CurrentScene.ToLower() == "endless")
                return;

                __instance.gameObject.AddComponent<PPlusTracker>();
        }

        [HarmonyPatch(typeof(LevelStats), nameof(LevelStats.Start))]
        [HarmonyPrefix]
        private static void InitializeUI(LevelStats __instance)
        {
            if (SceneHelper.CurrentScene.ToLower() == "endless")
                return;

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
            
            if (__instance.rankScore == 12 && !__instance.asscon.cheatsEnabled && PPlusTracker.Instance.AchievedUPlus(__instance))
            {
                string text = $"<color=#FFFFFF>{Database.Resource.ppRankName}</color>";
                __instance.fr.totalRank.transform.parent.GetComponent<Image>().color = Database.Resource.ppRankColor;
                __instance.fr.totalRank.fontSize = 200;
                __instance.fr.SetRank(text);

                Progress.AchievePP();
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
