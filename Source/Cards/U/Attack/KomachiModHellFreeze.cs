using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace KomachiMod.Cards
{
    public sealed class KomachiModHellFreezeDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(400);

            config.ImageId = "KomachiAttackB";

            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Black };
            config.Cost = new ManaGroup() { Blue = 1, Black = 1 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;

            config.Damage = 14;
            config.UpgradedDamage = 20;

            // Frost armor gain
            config.Value1 = 1;

            // Per how much spirits detonated
            config.Value2 = 3;
            config.UpgradedValue2 = 2;

            config.RelativeEffects = new List<string>() 
            { nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword), nameof(FrostArmor), nameof(Cold) };
            config.UpgradedRelativeEffects = new List<string>() 
            { nameof(KomachiModVengefulSpiritSe), nameof(KomachiDetonationKeyword), nameof(FrostArmor), nameof(Cold) };


            config.Illustrator = "文鳥";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModHellFreezeDef))]
    public sealed class KomachiModHellFreeze : KomachiCard
    {

        public override Interaction Precondition()
        {
            // Create list for interaction
            List<Card> list = new List<Card>();
            // make the 2 cards
            KomachiModHellFreeze noBoom = Library.CreateCard<KomachiModHellFreeze>();
            KomachiModHellFreeze boom = Library.CreateCard<KomachiModHellFreeze>();
            // indicate them
            noBoom.ChoiceCardIndicator = 1; // uses extra description 1
            boom.ChoiceCardIndicator = 2; // uses extra description 2
            // dk what these do tbh.
            noBoom.SetBattle(base.Battle);
            boom.SetBattle(base.Battle);
            // add em to the list
            list.Add(noBoom);
            list.Add(boom);
            return new MiniSelectCardInteraction(list);
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            yield return DebuffAction<KomachiModHellFreezeSe>(selector.SelectedEnemy, 1);

            if (selector.SelectedEnemy.HasStatusEffect<KomachiModVengefulSpiritSe>())
            {
                yield break;
            }
            Card choiceCard = KomachiModUtility.GetPreconditionCard(precondition);
            if (choiceCard != null && choiceCard.ChoiceCardIndicator != 1)
            {
                var detonation = new DetonateVengefulSpiritAction(this, selector.SelectedEnemy);
                yield return detonation;
                int enemySpiritsCount = detonation.Args.amountDetonated;
                yield return BuffAction<FrostArmor>(enemySpiritsCount * Value1 / Value2);
            }
            yield break;
        }
    }
}


