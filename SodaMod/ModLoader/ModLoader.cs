using HarmonyLib;
using SodaMod.IO;
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
            Logger.Connect();
            Logger.WriteLine("test 1");
            Logger.WriteLine("test 2");
            Logger.Close();
        }
    }
}
