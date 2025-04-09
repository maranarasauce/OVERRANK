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
        private class LevelKey
        {
            public string level;
            public int difficulty;
        }

        private static List<LevelKey> completeLevels;
        private static bool loaded;
        public static void Load()
        {
            if (loaded) return;

            string path = Path.Combine(Paths.PackedPath, Paths.ProgressFile);
            completeLevels = new List<LevelKey>();
            if (File.Exists(path))
            {
                string info = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(info))
                {
                    string[] levels = info.Split(Seperator);
                    foreach (string level in levels)
                    {
                        if (!string.IsNullOrEmpty(level))
                        {
                            string[] split = level.Split(SubSeperator);
                            LevelKey key = new LevelKey()
                            {
                                level = split[0],
                                difficulty = int.Parse(split[1])
                            };

                            completeLevels.Add(key);
                        }
                    }
                }
            }

            loaded = true;
        }

        public static void SlotSwitched()
        {
            loaded = false;
        }

        const char SubSeperator = '?';
        const char Seperator = '`';
        public static void Save()
        {
            string info = string.Empty;
            foreach (var level in completeLevels)
            {
                info += level.level;
                info += SubSeperator;
                info += level.difficulty;
                info += Seperator;
            }
            string path = Path.Combine(Paths.PackedPath, Paths.ProgressFile);
            File.WriteAllText(path, info);
        }

        public static void AchievePP()
        {
            string level = Path.GetFileNameWithoutExtension(GameProgressSaver.resolveCurrentLevelPath);

            bool alreadyExists = false;
            foreach (var preexisting in completeLevels)
            {
                if (preexisting.level == level)
                {
                    preexisting.difficulty = PrefsManager.Instance.GetInt("difficulty");
                    break;
                }
            }

            if (!alreadyExists)
            {
                LevelKey key = new LevelKey()
                {
                    difficulty = PrefsManager.Instance.GetInt("difficulty"),
                    level = level
                };
                completeLevels.Add(key);
            }
                
            Save();
        }

        public static bool HasPP(int num)
        {
            Load();
            string level = Path.GetFileNameWithoutExtension(GameProgressSaver.LevelProgressPath(num));
            foreach (LevelKey key in completeLevels)
            {
                int difficulty = PrefsManager.Instance.GetInt("difficulty");

                if (key.level == level && key.difficulty >= difficulty)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
