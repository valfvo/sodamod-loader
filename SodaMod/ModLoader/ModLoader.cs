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

        static void Main(string[] args)
        {
            // TODO
        }
    }
}
