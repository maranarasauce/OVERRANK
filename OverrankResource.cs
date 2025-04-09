using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Overrank
{
    public class OverrankResource : ScriptableObject
    {
        public StyleRank[] ranks;
        public Material[] rankMats;
        public AudioClip[] rankAscendClips;
        public int ultrakillRankMax = 1500;
        public float ultrakillRankDrain = 8f;

        public GameObject requirementUI;

        public float styleRequirementMod = 1.2f;

        public int healthRequirement = 10;
        public float styleTime = 10f;
        public float[] styleTimeSpeeds = new float[3]
        {
            1f,
            2f,
            3f
        };

        public string ppRankName = "P+";
        public Color ppRankColor = new Color(0.843f, 0.2f, 1f, 1f);
        public string ppRankHex = "";
    }
}
