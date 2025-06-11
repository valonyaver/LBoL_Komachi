using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using LBoL.Base;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using KomachiMod.Cards.Template;
using KomachiMod.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace KomachiMod
{
    [BepInPlugin(KomachiMod.PInfo.GUID, KomachiMod.PInfo.Name, KomachiMod.PInfo.version)]
    [BepInDependency(LBoLEntitySideloader.PluginInfo.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(AddWatermark.API.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("LBoL.exe")]
    public class BepinexPlugin : BaseUnityPlugin
    {
        //The Unique mod ID of the mod.
        //If defined, this is also the ID used by the Act 1 boss.
        //WARNING: It is mandatory to rename it to avoid issues.
        public static string modUniqueID = "KomachiMod";
        //Name of the character.
        //This is also the prefix that is used before every .png file in DirResources. 
        public static string playerName = "Komachi";
        //Whether to us an ingame or custom model.
        //InGame: Will load the character model of the ingame character.
        //Custom: Will load DirResource/KomachiModel.png 
        public static bool useInGameModel = false;
        //If InGame is selected, this is the model that will be loaded. 
        //Check LBoL.EntityLib.EnemyUnits.Character or using LBoL.EntityLib.PlayerUnits for a list of all the characters available. 
        public static string modelName = nameof(Rin);
        //Some in-game model needs to be flipped (most notably elites).
        public static bool modelIsFlipped = true;
        //The character's off-color.
        //Used to separate cards in the card collection and put the off-color cards at the end.
        public static List<ManaColor> offColors = new List<ManaColor>() { ManaColor.Colorless, ManaColor.Green, ManaColor.Blue };

        //Whether the Act 1 boss should be enabled.
        //The value can be customized LBoL/BepInEx/config/
        public static ConfigEntry<bool> enableAct1Boss;

        public static CustomConfigEntry<bool> enableAct1BossEntry = new CustomConfigEntry<bool>(
            value: false,
            section: "EnableAct1Boss",
            key: "EnableAct1Boss",
            description: "Toggle the Act 1 boss. Default: Off");

        private static readonly Harmony harmony = KomachiMod.PInfo.harmony;

        internal static BepInEx.Logging.ManualLogSource log;

        internal static TemplateSequenceTable sequenceTable = new TemplateSequenceTable();

        internal static IResourceSource embeddedSource = new EmbeddedSource(Assembly.GetExecutingAssembly());

        // add this for audio loading
        internal static DirectorySource directorySource = new DirectorySource(KomachiMod.PInfo.GUID, "");


        private void Awake()
        {
            log = Logger;
            ///Load the custom config entry.
            enableAct1Boss = Config.Bind(enableAct1BossEntry.Section, enableAct1BossEntry.Key, enableAct1BossEntry.Value, enableAct1BossEntry.Description);

            // very important. Without this the entry point MonoBehaviour gets destroyed
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            CardIndexGenerator.PromiseClearIndexSet();
            EntityManager.RegisterSelf();

            harmony.PatchAll();

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(AddWatermark.API.GUID))
                WatermarkWrapper.ActivateWatermark();

            Func<Sprite> getSprite = () => ResourceLoader.LoadSprite("BossIcon.png", directorySource);
            EnemyUnitTemplate.AddBossNodeIcon(nameof(KomachiMod.Enemies.KomachiMod), getSprite);
        }

        private void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }
    }
}
