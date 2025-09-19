using HarmonyLib;
using KomachiMod.BattleActions;
using KomachiMod.Cards;
using KomachiMod.Cards.Template;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoLEntitySideloader.CustomKeywords;
using System.Linq;

namespace KomachiMod.Source.BattleActions.EventManager
{
    [HarmonyPatch]
    class KomachiEventsManager
    {

        public static GameEvent<DistanceChangedEventArgs> DistanceChanged { get; set; }
        public static GameEvent<ApplyVengefulSpiritEventArgs> AppliedVengefulSpirit;
        public static GameEvent<KomachiReleaseEventArgs> spiritsReleasing;
        public static GameEvent<KomachiReleaseEventArgs> spiritsReleased;
        public static GameEvent<DetonateVengefulSpiritEventArgs> DetonatedSpirits;
        public static BattleController Battle;

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
        private static bool Prefix(GameRunController __instance)
        {
            DistanceChanged = new GameEvent<DistanceChangedEventArgs>();
            AppliedVengefulSpirit = new GameEvent<ApplyVengefulSpiritEventArgs>();
            spiritsReleasing = new GameEvent<KomachiReleaseEventArgs>();
            spiritsReleased = new GameEvent<KomachiReleaseEventArgs>();
            DetonatedSpirits = new GameEvent<DetonateVengefulSpiritEventArgs>();

            
            //Battle = __instance.Battle;
            //Battle.Player.HandleBattleEvent<CardEventArgs>
            //    (Battle.Predraw,
            //    new GameEventHandler<CardEventArgs>(OnPlayerDrawing), GameEventPriority.ConfigDefault);
            //Battle.Player.HandleBattleEvent<CardsEventArgs>
            //    (Battle.CardsAddingToHand,
            //    new GameEventHandler<CardsEventArgs>(OnPlayerAddingMany), GameEventPriority.ConfigDefault);
            //Battle.Player.HandleBattleEvent<CardMovingEventArgs>
            //    (Battle.CardMoving,
            //    new GameEventHandler<CardMovingEventArgs>(OnPlayerAdding), GameEventPriority.ConfigDefault);
            return true;
        }

        [HarmonyPatch(typeof(BattleController), "get_HandIsFull")]
        static void Postfix(ref bool __result, BattleController __instance)
        {
            // If hand is physically full
            if (__instance.HandZone.Count == __instance.MaxHand)
            {
                // Check if there are any auto-discard cards
                bool hasAutoDiscard = __instance.HandZone.Any(card =>
                    card is KomachiCard && (card as KomachiCard).isAutoDiscard);

                // If auto-discard cards exist, return false (hand is not "full" for game logic)
                if (hasAutoDiscard)
                {
                    __result = false;
                }
            }
        }
    }
}