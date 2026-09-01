using BepInEx;
using BepInEx.Configuration;
using DG.Tweening.Plugins.Core;
using HarmonyLib;
using KomachiMod.Cards.Template;
using KomachiMod.Config;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.Presentation.UI;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


namespace KomachiMod
{
    [BepInPlugin(KomachiMod.PInfo.GUID, KomachiMod.PInfo.Name, KomachiMod.PInfo.version)]
    [BepInDependency(LBoLEntitySideloader.PluginInfo.GUID, BepInDependency.DependencyFlags.HardDependency)]
    // No longer need watermark for 1.8
    //[BepInDependency(AddWatermark.API.GUID, BepInDependency.DependencyFlags.SoftDependency)]
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
        public static List<ManaColor> offColors = new List<ManaColor>() { ManaColor.White, ManaColor.Green, ManaColor.Blue, ManaColor.Colorless };

        //Whether the Act 1 boss should be enabled.
        //The value can be customized LBoL/BepInEx/config/
        public static ConfigEntry<bool> enableAct1Boss;

        public static CustomConfigEntry<bool> enableAct1BossEntry = new CustomConfigEntry<bool>(
            value: true,
            section: "EnableAct1Boss",
            key: "EnableAct1Boss",
            description: "Toggle the Act 1 boss. Default: On");


        public static ConfigEntry<float> distanceMultiplier1;
        public static ConfigEntry<float> distanceMultiplier2;
        public static ConfigEntry<float> distanceMultiplier3;
        public static ConfigEntry<float> distanceMultiplier4;
        public static ConfigEntry<float> distanceMultiplier5;

        private static readonly Harmony harmony = KomachiMod.PInfo.harmony;

        internal static BepInEx.Logging.ManualLogSource log;

        internal static TemplateSequenceTable sequenceTable = new TemplateSequenceTable();

        internal static IResourceSource embeddedSource = new EmbeddedSource(Assembly.GetExecutingAssembly());

        // add this for audio loading
        internal static DirectorySource directorySource = new DirectorySource(KomachiMod.PInfo.GUID, "");

        private static bool hasRegistered = false;

        private void Awake()
        {
            log = Logger;
            // Load the custom config entry.
            enableAct1Boss = Config.Bind(enableAct1BossEntry.Section, enableAct1BossEntry.Key, enableAct1BossEntry.Value, enableAct1BossEntry.Description);
            Debug.Log($"Enabling komachi boss is {enableAct1Boss.Value}");

            // Configure distance multipliers
            distanceMultiplier1 = Config.Bind("Distance Multipliers", "Distance1_Multiplier", 2.0f,
                "Damage multiplier for Distance 1 (Very Close). Default: 2.0");

            distanceMultiplier2 = Config.Bind("Distance Multipliers", "Distance2_Multiplier", 1.5f,
                "Damage multiplier for Distance 2 (Close). Default: 1.5");

            distanceMultiplier3 = Config.Bind("Distance Multipliers", "Distance3_Multiplier", 1.0f,
                "Damage multiplier for Distance 3 (Normal). Default: 1.0");

            distanceMultiplier4 = Config.Bind("Distance Multipliers", "Distance4_Multiplier", 0.85f,
                "Damage multiplier for Distance 4 (Far). Default: 0.85");

            distanceMultiplier5 = Config.Bind("Distance Multipliers", "Distance5_Multiplier", 0.7f,
                "Damage multiplier for Distance 5 (Very Far). Default: 0.7");

            Debug.Log($"Distance multipliers loaded: {distanceMultiplier1.Value}, {distanceMultiplier2.Value}, {distanceMultiplier3.Value}, {distanceMultiplier4.Value}, {distanceMultiplier5.Value}");
            // very important. Without this the entry point MonoBehaviour gets destroyed
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            CardIndexGenerator.PromiseClearIndexSet();
            EntityManager.RegisterSelf();

            // InitializeCustomInput();
            harmony.PatchAll();
            try
            {
                Func<Sprite> getSprite = () => ResourceLoader.LoadSprite("BossIcon.png", directorySource);
                EnemyUnitTemplate.AddBossNodeIcon(nameof(KomachiMod.Enemies.KomachiMod), getSprite);
            }
            catch (ArgumentException ex) when (ex.Message.Contains("same key"))
            {
                log.LogWarning("Boss icon already registered");
            }
        }

        private static InputAction myF10Action;
        private void InitializeCustomInput()
        {
            // Create a new action with the name you want
            myF10Action = new InputAction("MyF10Action", InputActionType.Button);

            // Bind the F10 key to this action
            myF10Action.AddBinding("<Keyboard>/f10");

            // Enable the action so it starts listening for input events
            myF10Action.Enable();

            // Note: We don't subscribe to 'performed' here because we will poll in Update().

            Logger.LogInfo("Custom Input Action 'MyF10Action' bound to F10 and enabled.");
        }

        // Enable this and call it in awake whenever you want to make custom input.
        //private void Update()
        //{
        //    // The direct polling equivalent to Input.GetKeyDown(KeyCode.F10)
        //    if (myF10Action != null && myF10Action.WasPressedThisFrame())
        //    {
        //        // Call your action logic directly
        //        OnMyCustomActionPerformed();
        //    }
        //}

        private static void OnMyCustomActionPerformed()
        {
            Debug.Log($"Attempting to check boolets");
            if (BulletConfig._NameTable.ContainsKey("Knife"))
            {
                Debug.Log($"The nametable contains knife");
                Debug.Log($"Original bullet is named {BulletConfig._NameTable["Knife"].Name} and its widget is {BulletConfig._NameTable["Knife"].Widget}");

            }
            if (BulletConfig._NameTable.ContainsKey("KomachiModTestBullet1"))
            {
                Debug.Log($"Custom bullet is named {BulletConfig._NameTable["KomachiModTestBullet1"].Name} and its widget is {BulletConfig._NameTable["KomachiModTestBullet1"].Widget}");
            }
        }

        private void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();

        }
    }
}
