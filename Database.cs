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
        }
    }
}
