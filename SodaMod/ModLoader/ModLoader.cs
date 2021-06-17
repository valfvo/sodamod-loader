using HarmonyLib;
using System;
using System.IO;

namespace SodaMod.ModLoader
{
    public static class ModLoader
    {
        public static readonly string BaseDirectory = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SodaModLoader");

        public static readonly string ModDirectory = 
            Path.Combine(BaseDirectory, "Mods");

        internal static Harmony patcher = new Harmony("app.fvo.sodamod-loader");

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyLoad += ModLoader_AssemblyLoadEventHandler;
            SodaMod.IO.Logger.Connect();
        }

        private static void ModLoader_AssemblyLoadEventHandler(
            object sender,
            AssemblyLoadEventArgs args)
        {
            string assemblyName = args.LoadedAssembly.GetName().Name;
            if (assemblyName == "System.Xml")
            {
                AppDomain.CurrentDomain.AssemblyLoad -= ModLoader_AssemblyLoadEventHandler;
                PatchAllAssemblies();
            }
        }

        private static void PatchAllAssemblies()
        {
            patcher.PatchAll();
        }
    }
}
