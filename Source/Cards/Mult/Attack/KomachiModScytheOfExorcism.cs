using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KomachiMod.Cards
{
    public sealed class KomachiModScytheRedLilyDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(7010);
            config.GunNameBurst = GunNameID.GetGunFromId(7011);

            // config.ImageId = "KomachiAttackB";

            config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7, Any = 2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 18;
            config.UpgradedDamage = 24;

            // Amount needed and generated on release.
            config.Value1 = 4;

            // Release cost.
            config.Value2 = 6;
            config.UpgradedValue2 = 4;


            config.RelativeEffects = new List<string>() 
            { 
                nameof(KomachiModVengefulSpiritSe), 
                nameof(KomachiDetonationKeyword), 
                nameof(KomachiModReleaseKeyword),
                nameof(Amulet)
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiDetonationKeyword),
                nameof(KomachiModReleaseKeyword),
                nameof(Amulet)
            };

            config.RelativeCards = new List<string>()
            {
                nameof(KomachiModSpiderLily)
            };
            config.UpgradedRelativeCards = new List<string>()
            {
                nameof(KomachiModSpiderLily)
            };



            config.Illustrator = "violence kumahina";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModScytheRedLilyDef))]
    public sealed class KomachiModScytheRedLily : KomachiCard
    {
        public int one = 1;
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, Value2);
        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, Value2);
        }

        public Interaction BoomCondition()
        {
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModDetonateToken noBoom = Library.CreateCard<KomachiModDetonateToken>();
            KomachiModScytheRedLily boom = Library.CreateCard<KomachiModScytheRedLily>();
            // indicate them
            noBoom.ChoiceCardIndicator = 1; // uses extra description 1
            boom.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            noBoom.SetBattle(base.Battle);
            boom.SetBattle(base.Battle);
            // add em to the list
            list.Add(noBoom);
            list.Add(boom);
            Debug.Log("End of boom precon");
            return new MiniSelectCardInteraction(list) { Source = this };
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            // look above for vengeful spirit attack

            Card preconditionCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(preconditionCard))
            {
                yield return new KomachiReleaseAction(this, Value2);
                yield return new ApplyVengefulSpiritAction(this, selector.SelectedEnemy, Value1);
                yield return BuffAction<Amulet>(1);
            }

            // End if has no spirits to speak of
            if (!(selector.SelectedEnemy.HasStatusEffect<KomachiModVengefulSpiritSe>() || 
                selector.SelectedEnemy.HasStatusEffect<KomachiModLonelyBoundSpiritSe>()))
            {
                yield break;
            }

            Debug.Log("Before scythe of exorcism precon");
            MiniSelectCardInteraction precon2 = (MiniSelectCardInteraction) BoomCondition();
            yield return new InteractionAction(precon2);

            Card detonateChoice = precon2.SelectedCard;
            if (detonateChoice != null && detonateChoice.ChoiceCardIndicator != 1)
            {
                var detonation = new DetonateVengefulSpiritAction(this,selector.SelectedEnemy);
                yield return detonation;
                if (detonation.Args.amountDetonated >= 4)
                {
                    yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<KomachiModSpiderLily>() });
                }
            }
            yield break;
        }
    }
}


