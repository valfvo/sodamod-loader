using System;
using System.IO;
using HarmonyLib;

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
            // TODO
        }
    }
}
