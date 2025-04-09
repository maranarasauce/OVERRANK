using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overrank.Patching
{
    public class CheatPatches
    {

        [HarmonyPatch(typeof(LeaderboardController), nameof(LeaderboardController.SubmitCyberGrindScore))]
        [HarmonyPrefix]
        private static bool BlockCGScores()
        {
            return false;
        }


        [HarmonyPatch(typeof(LeaderboardController), nameof(LeaderboardController.SubmitLevelScore))]
        [HarmonyPrefix]
        private static bool BlockLevelScores()
        {
            return false;
        }
    }
}
