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
    }
}
