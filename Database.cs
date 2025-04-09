using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Overrank
{
    public static class Database
    {

        private static OverrankResource resource;
        public static OverrankResource Resource { get { return resource; } }

        public static void Init()
        {
            string path = Path.Combine(Paths.PackedPath, "overrank");
            AssetBundle bundle = AssetBundle.LoadFromFile(path);
            resource = bundle.LoadAsset<OverrankResource>(Path.Combine(Paths.BundleProjectPath, "OverrankResource.asset"));
            OverwriteConfig();

        }

        private static void OverwriteConfig()
        {
            string configPath = Path.Combine(Paths.PackedPath, "config.txt");
            if (!File.Exists(configPath))
                return;
            string configFile = File.ReadAllText(configPath);

            string[] lines = configFile.Split('\n');
            Dictionary<string, float> lib = new Dictionary<string, float>();
            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                    continue;
                string[] kvp = line.Split(':');
                string key = kvp[0];
                float value = 0f; float.TryParse(kvp[1], out value);
                lib.Add(key, value);
            }

            resource.ranks[0].drainSpeed =  lib["overkill_drain"];
            resource.ranks[0].maxMeter = (int)lib["overkill_max"];
            resource.ranks[1].drainSpeed =  lib["hellborn_drain"];
            resource.ranks[1].maxMeter = (int)lib["hellborn_max"];
            resource.ranks[2].drainSpeed =  lib["omnicide_drain"];
            resource.ranks[2].maxMeter = (int)lib["omnicide_max"];
            resource.ultrakillRankDrain = lib["ultrakill_drain"];
            resource.ultrakillRankMax = (int)lib["ultrakill_max"];
            resource.healthRequirement = (int)lib["prequirement_hp"];
            resource.styleRequirementMod = lib["prequirement_rankstylemodifier"];
            resource.styleTimeSpeeds = new float[3];
            resource.styleTimeSpeeds[0] = lib["prequirement_overkillspeed"];
            resource.styleTimeSpeeds[1] = lib["prequirement_hellbornspeed"];
            resource.styleTimeSpeeds[2] = lib["prequirement_omnicidespeed"];
        }
    }
}
