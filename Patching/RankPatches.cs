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
        private static AudioSource src;
        private static Material defaultRankMaterial;
        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.Start))]
        [HarmonyPrefix]
        private static void InitRanks(StyleHUD __instance)
        {
            src = __instance.gameObject.AddComponent<AudioSource>();
            src.volume = 0.5f;
            lastPlayedIndex = -1;
            defaultRankMaterial = __instance.rankImage.material;

            __instance.ranks[7].drainSpeed = Database.Resource.ultrakillRankDrain;
            __instance.ranks[7].maxMeter = Database.Resource.ultrakillRankMax;
            __instance.ranks.AddRange(Database.Resource.ranks);
        }

        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.AddPoints))]
        [HarmonyPostfix]
        public static void AddPoints(StyleHUD __instance)
        {
            if (__instance.currentMeter >= (float)__instance.currentRank.maxMeter && __instance.rankIndex < __instance.ranks.Count - 1)
            {
                __instance.AscendRank();
            }
        }

        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.DescendRank))]
        [HarmonyPostfix]
        public static void OnRankChange(StyleHUD __instance)
        {
            if (__instance.rankIndex > 7)
            {
                __instance.rankImage.material = Database.Resource.rankMats[__instance.rankIndex - 8];
                UpdateHUD(__instance);
            } else
            {
                __instance.rankImage.material = defaultRankMaterial;
            }
        }


        static int lastPlayedIndex = -1;
        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.AscendRank))]
        [HarmonyPostfix]
        public static void AscendRank(StyleHUD __instance)
        {
            OnRankChange(__instance);
            if (__instance.rankIndex > 7)
            {
                int index = __instance.rankIndex - 8;
                if (index == lastPlayedIndex)
                    return;
                src.clip = Database.Resource.rankAscendClips[index];
                src.Play();
                lastPlayedIndex = index; indexRefresh = 30f;
            }
        }


        static float indexRefresh = 0f;
        [HarmonyPatch(typeof(StyleHUD), nameof(StyleHUD.UpdateHUD))]
        [HarmonyPostfix]
        public static void UpdateHUD(StyleHUD __instance)
        {
            if (indexRefresh > 0f)
                indexRefresh = Mathf.MoveTowards(indexRefresh, 0f, Time.unscaledDeltaTime);
            else
            {
                indexRefresh = -1f;
                lastPlayedIndex = -1;
            }

            if (__instance.rankIndex < 8)
                return;
            float t = __instance.currentMeter / (float)__instance.currentRank.maxMeter;

            switch (__instance.rankIndex)
            {
                case 8:
                    UpdateOverkillHUD(__instance, Database.Resource.rankMats[0], t);
                    break;
                case 9:
                    UpdateHellbornHUD(__instance, Database.Resource.rankMats[1], t);
                    break;
                case 10:
                    UpdateOmnicideHUD(__instance, Database.Resource.rankMats[2], t);
                    break;
            }
        }

        private static float RaisedTime(float t)
        {
            return Mathf.Clamp01(3f * (t - 0.333f) - 0.25f);
        }

        private static Color BlendColor(Color a, Color b, float t)
        {
            return Color.Lerp(a, b, t);
            Vector3 aHSV = Vector3.zero;
            Color.RGBToHSV(a, out aHSV.x, out aHSV.y, out aHSV.z);
            Vector3 bHSV = Vector3.zero;
            Color.RGBToHSV(b, out bHSV.x, out bHSV.y, out bHSV.z);

            if (bHSV.x > 0.5f && aHSV.y < 0.5f)
            {
                bHSV.x -= 1f;
            } else if (bHSV.x < 0.5f && aHSV.y > 0.5f)
            {
                bHSV.x += 1f;
            }

            Vector3 c = Vector3.Lerp(aHSV, bHSV, t);
            return Color.HSVToRGB(c.x, c.y, c.z);
        }

        const string CrackSizeKey = "_Crack_Size";
        const string CrackColorKey = "_Crack_Color";
        static Color CRACK_COLOR_START = new Color(0.645283f, 0.1213694f, 0f);
        static Color CRACK_COLOR_END   = Color.yellow;
        const string OverflowAmountKey = "_Overflow_Amount";
        const string CrackGlowKey = "_Crack_Glow_Amount";

        public static void UpdateOverkillHUD(StyleHUD hud, Material mat, float t)
        {
            float rt = RaisedTime(t);
            mat.SetFloat(CrackSizeKey, t);
            //mat.color = Color.Lerp(Color.white, Color.red, t);
            mat.SetColor(CrackColorKey, Color.Lerp(CRACK_COLOR_START, CRACK_COLOR_END, rt));
            mat.SetFloat(OverflowAmountKey, rt * 2f);
            mat.SetFloat(CrackGlowKey, (rt * 2f) - 0.5f);
        }

        static Color HELLBORN_BOTTOM_START = Color.red;
        static Color HELLBORN_TOP_START = Color.yellow;
        static Color HELLBORN_BOTTOM_END =   new Color(1f, 0f, 1f);
        static Color HELLBORN_TOP_END =      Color.blue;
        const float HELLBORN_FIRE_SPEED_START = -0.2f;
        const float HELLBORN_FIRE_SPEED_END = -0.8f;
        const string ShakeIntensityKey = "_Shake_Intensity";
        const float HELLBORN_SHAKE_START = 0f;
        const float HELLBORN_SHAKE_END = 10f;

        public static void UpdateHellbornHUD(StyleHUD hud, Material mat, float t)
        {
            float raisedT = RaisedTime(t);
            mat.SetColor(FireBottomKey, BlendColor(HELLBORN_BOTTOM_START, HELLBORN_BOTTOM_END, raisedT));
            mat.SetColor(FireTopKey, BlendColor(HELLBORN_TOP_START, HELLBORN_TOP_END, raisedT));
            float speed = Mathf.Lerp(HELLBORN_FIRE_SPEED_START, HELLBORN_FIRE_SPEED_END, t);
            mat.SetFloat(FireOffsetKey, mat.GetFloat(FireOffsetKey) + speed * Time.deltaTime);
            float shake = Mathf.Lerp(HELLBORN_SHAKE_START, HELLBORN_SHAKE_END, raisedT);
            mat.SetFloat(ShakeIntensityKey, shake);
        }

        static Color OMNICIDE_BOTTOM_START =  new Color(1f, 0f, 1f);
        static Color OMNICIDE_TOP_START = Color.blue;
        static Color OMNICIDE_BOTTOM_END =      new Color(0.6942911f, 0f, 1f);
        static Color OMNICIDE_TOP_END =       Color.white;
        static Color OMNICIDE_RANKCOLOR_START = Color.red;
        static Color OMNICIDE_RANKCOLOR_END = Color.black;
        const string FireBottomKey =           "_Flame_Color_Bottom";
        const string FireTopKey =              "_Flame_Color_Top";
        const string FireDistortionTopKey = "_Flame_Distortion_Top";
        const string FireOffsetKey = "_Flame_Offset";
        const string FireFresnel = "_Flame_Fresnel";
        const string RankColor = "_Rank_Color";
        const float OMNICIDE_FIRE_DISTORTION_START = 0.06f;
        const float OMNICIDE_FIRE_DISTORTION_END = 0.12f;
        const float OMNICIDE_FIRE_SPEED_START = -0.5f;
        const float OMNICIDE_FIRE_SPEED_END = -1.3f;
        const float OMNICIDE_SHAKE_START = 10f;
        const float OMNICIDE_SHAKE_END = 20f;
        const float OMNICIDE_FRESNEL_START = 0.4f;
        const float OMNICIDE_FRESNEL_END = 0f;


        public static void UpdateOmnicideHUD(StyleHUD hud, Material mat, float t)
        {
            float raisedT = RaisedTime(t);
            mat.SetColor(RankColor, BlendColor(OMNICIDE_RANKCOLOR_START, OMNICIDE_RANKCOLOR_END, raisedT));
            mat.SetColor(FireBottomKey, BlendColor(OMNICIDE_BOTTOM_START, OMNICIDE_BOTTOM_END, raisedT));
            mat.SetColor(FireTopKey, BlendColor(OMNICIDE_TOP_START, OMNICIDE_TOP_END, raisedT));
            mat.SetFloat(FireDistortionTopKey, Mathf.Lerp(OMNICIDE_FIRE_DISTORTION_START, OMNICIDE_FIRE_DISTORTION_END, raisedT));
            float speed = Mathf.Lerp(OMNICIDE_FIRE_SPEED_START, OMNICIDE_FIRE_SPEED_END, raisedT);
            mat.SetFloat(FireOffsetKey, mat.GetFloat(FireOffsetKey) + speed * Time.deltaTime);
            float shake = Mathf.Lerp(OMNICIDE_SHAKE_START, OMNICIDE_SHAKE_END, raisedT);
            mat.SetFloat(ShakeIntensityKey, shake);
            float fresnel = Mathf.Lerp(OMNICIDE_FRESNEL_START, OMNICIDE_FRESNEL_END, raisedT);
            mat.SetFloat(FireFresnel, fresnel);
        }
    }
}
