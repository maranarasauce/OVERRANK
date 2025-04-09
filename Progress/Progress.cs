using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overrank
{
    public static class Progress
    {
        private static List<string> completeLevels;
        private static bool loaded;
        public static void Load()
        {
            if (loaded) return;

            string path = Path.Combine(Paths.PackedPath, "pp.bepis");
            completeLevels = new List<string>();
            if (File.Exists(path))
            {
                string info = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(info))
                {
                    string[] levels = info.Split(Seperator);
                    foreach (string level in levels)
                    {
                        if (!string.IsNullOrEmpty(level))
                            completeLevels.Add(level);
                    }
                }
            }

            loaded = true;
        }

        const char Seperator = '`';
        public static void Save()
        {
            string info = string.Empty;
            foreach (string level in completeLevels)
            {
                info += level;
                info += Seperator;
            }
            string path = Path.Combine(Paths.PackedPath, "pp.bepis");
            File.WriteAllText(path, info);
        }

        public static void AchievePP()
        {
            string level = GameProgressSaver.resolveCurrentLevelPath;
            if (!completeLevels.Contains(level))
                completeLevels.Add(level);
            Save();
        }

        public static bool HasPP(int level)
        {
            Load();
            return completeLevels.Contains(GameProgressSaver.LevelProgressPath(level));
        }
    }
}
