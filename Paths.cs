using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Overrank
{
    public static class Paths
    {
        public static string BundleProjectPath
        {
            get
            {
                if (string.IsNullOrEmpty(_bundleProjectPath))
                    _bundleProjectPath = Path.Combine("Assets", "Custom", "Maranara", "Code Mods", "Overrank");

                return _bundleProjectPath;
            }
        }
        private static string _bundleProjectPath;
        public static string PackedPath
        {
            get
            {
                if (string.IsNullOrEmpty(_packedPath))
                    _packedPath = Path.Combine($"{ModPath()}", "bundles");

                return _packedPath;
            }
        }
        private static string _packedPath;


        private static string ModPath()
        {
            return Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf("\\"));
        }
    }
}
