using HarmonyLib;

namespace KomachiMod
{
    public static class PInfo
    {
        //Rename the variable below to prevent conflicts between mod.
        public const string GUID = "valon.LBoL.character.Komachi";
        public const string Name = "KomachiMod";
        public const string version = "0.0.1";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
