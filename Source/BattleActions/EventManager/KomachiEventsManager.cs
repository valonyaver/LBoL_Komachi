using HarmonyLib;
using KomachiMod.BattleActions;
using KomachiMod.Cards;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
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

        //static void OnPlayerDrawing(CardEventArgs args)
        //{
        //    AutoDiscard(args.Card);
        //}
        //static void OnPlayerAddingMany(CardsEventArgs args)
        //{
        //    AutoDiscard(args.Cards.First());
        //}

        //static void OnPlayerAdding(CardMovingEventArgs args)
        //{
        //    AutoDiscard(args.Card);
        //}
        //static void AutoDiscard(Card cause)
        //{
        //    if (Battle.HandZone.Count == Battle.MaxHand)
        //    {
        //        while (Battle.HandZone.Count == Battle.MaxHand && Battle.HandZone.Any((Card card) => card is KomachiModSpiderLily))
        //        {
        //            Card firstSpiderLily = Battle.HandZone.FirstOrDefault(card => card is KomachiModSpiderLily);
        //            if (firstSpiderLily != null)
        //            {
        //               Battle.React(new DiscardAction(firstSpiderLily), cause, ActionCause.AutoExile);
        //            }
        //        }
        //    }
        //    return;
        //    //new DrawCardAction();
        //    //new AddCardsToHandAction();
        //    //new MoveCardAction();
        //}
    }
}