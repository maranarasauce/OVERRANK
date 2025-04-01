using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

namespace Overrank
{
    public static class Singletons
    {
        public static StyleHUD shud { get => StyleHUD.Instance; }
        public static GunControl gc { get => GunControl.Instance; }
        public static WeaponCharges wc { get => WeaponCharges.Instance; }
        public static TimeController timeController { get => TimeController.Instance; }
    }
}
