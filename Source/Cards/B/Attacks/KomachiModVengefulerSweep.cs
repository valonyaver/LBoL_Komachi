using KomachiMod.BattleActions;
using KomachiMod.Cards.Template;
using KomachiMod.GunName;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.Source.StatusEffects.Spirits;
using KomachiMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using static LBoL.Core.GameMap;

namespace KomachiMod.Cards
{
    public sealed class KomachiModVengefulerSweepDef : KomachiCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            // Possible candidates: 7131 (Fire from behind), 39051 (Big Reimu hectagon), 7061, 4721
            config.GunName = GunNameID.GetGunFromId(7080);
            config.GunNameBurst = GunNameID.GetGunFromId(7081);

            // config.ImageId = nameof(KomachiModAttackB);

            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1, Any = 2 };
            config.Rarity = Rarity.Uncommon;

            config.Type = CardType.Attack;
            config.TargetType = TargetType.AllEnemies;

            config.Damage = 14;
            config.UpgradedDamage = 18;

            // Spirits inflicted normally
            config.Value1 = 4;
            config.UpgradedValue1 = 6;

            // Extreme spirits inflicted
            config.Value2 = 12;
            config.UpgradedValue2 = 18;

            config.RelativeKeyword = Keyword.Accuracy;
            config.UpgradedRelativeKeyword = Keyword.Accuracy;

            config.RelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(KomachiModReleaseKeyword), nameof(KomachiModVengefulSpiritSe) };


            config.Illustrator = "ダバデぃ";

            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    
    [EntityLogic(typeof(KomachiModVengefulerSweepDef))]
    public sealed class KomachiModVengefulerSweep : KomachiCard
    {
        public string ExtraDescription4 => LocalizeProperty("ExtraDescription4", decorated: true, required: false);
        public override bool Triggered => KomachiModUtility.CanReleaseSpirits(Battle.Player, releaseCost1);
        public int attackTimes = 2;
        public int releaseCost1 = 4;
        public int releaseCost2 = 12;


        public string GunName2 = GunNameID.GetGunFromId(4002);
        public DamageInfo Damage2 => DamageInfo.Attack(RawDamage2, true);
        public int RawDamage2 => ConfigDamage2 + AdditionalDamage + DeltaDamage;
        public int BaseDamage2 => 12;
        public int UpgradedBaseDamage2 => 15;
        public int ConfigDamage2
        {
            get
            {
                int num;
                if (!IsUpgraded)
                {
                    num = BaseDamage2; // If not upgraded, use Damage2
                }
                else
                {
                    num = UpgradedBaseDamage2; // If upgraded, use UpgradedDamage2
                }
                return num;
            }
        }

        public override Interaction Precondition()
        {
            return KomachiModUtility.ChooseRelease(this, releaseCost1, releaseCost2);
        }

        protected override string GetBaseDescription()
        {
            string text = string.Empty;
            switch (ChoiceCardIndicator)
            {
                case 1:
                    text = RawExtraDescription1;
                    break;
                case 2:
                    text = RawExtraDescription2;
                    break;
                case 3:
                    text = RawExtraDescription3;
                    break;
                case 4:
                    // There is no upgraded extra description 4
                    text = ExtraDescription4;
                    break;
                default:
                    {
                        if (Battle != null)
                        {
                            text = (IsUpgraded ? (UpgradedBaseDescription ?? base.BaseDescription) : base.BaseDescription);
                            break;
                        }

                        object obj;
                        if (!IsUpgraded)
                        {
                            obj = NonbattleBaseDescription ?? base.BaseDescription;
                        }
                        else
                        {
                            obj = UpgradedNonbattleBaseDescription ?? UpgradedBaseDescription;
                            if (obj == null)
                            {
                                obj = NonbattleBaseDescription ?? base.BaseDescription;
                            }
                        }

                        text = (string)obj;
                        break;
                    }
            }

            return FollowByDetailIcon(text);
        }
        

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card releaseChoice = KomachiModUtility.GetPreconditionCard(precondition);
            if (KomachiModUtility.ChoseRelease(releaseChoice, releaseCost1, releaseCost2, out int costResult))
            {
                // If the first release choice
                if (releaseChoice.ChoiceCardIndicator == 1)
                {
                    // Pay the tax
                    yield return new KomachiReleaseAction(this, costResult);

                    // Choose whether to make the attack accurate or to apply more spirits
                    Card accurateChoice = Library.CreateCard<KomachiModVengefulerSweep>(IsUpgraded);
                    accurateChoice.ChoiceCardIndicator = 3;
                    accurateChoice.SetBattle(Battle);
                    Card spiritsChoice = Library.CreateCard<KomachiModVengefulerSweep>(IsUpgraded);
                    spiritsChoice.ChoiceCardIndicator = 4;
                    spiritsChoice.SetBattle(Battle);
                    List<Card> choices = new List<Card>() { accurateChoice, spiritsChoice };
                    MiniSelectCardInteraction chooseEffect = new MiniSelectCardInteraction(choices);
                    yield return new InteractionAction(chooseEffect);

                    // Initialize variables for the choice
                    bool isAccurate = false;
                    int vengefulSpiritsAmount = Value1;
                    // Apply choice on variables
                    if (chooseEffect.SelectedCard.ChoiceCardIndicator == 4)
                    {
                        vengefulSpiritsAmount = vengefulSpiritsAmount * 2;
                    }
                    else
                    {
                        isAccurate = true;
                    }

                    // Get all enemies and either attack them accurately or doubly apply the spirits on em.
                    EnemyUnit[] enemies = selector.GetEnemies(Battle);
                    if (enemies.Length != 0)
                    {
                        yield return new DamageAction(Battle.Player, enemies, Damage2, GunName);
                        foreach (var enemy in enemies)
                        {
                            yield return new ApplyVengefulSpiritAction(this, enemy, vengefulSpiritsAmount);
                        }
                    }
                }
                // If you chose the second one.
                else
                {
                    yield return new KomachiReleaseAction(this, costResult);
                    base.CardGuns = new Guns(base.GunName, 1, true);
                    CardGuns.Add(GunName2);
                    EnemyUnit[] enemies = selector.GetEnemies(Battle);
                    if (enemies.Length != 0)
                    {
                        foreach (GunPair gunPair in base.CardGuns.GunPairs)
                        {
                            yield return new DamageAction(Battle.Player, enemies, Damage2, gunPair.GunName, gunPair.GunType);
                        }
                        foreach (var enemy in enemies)
                        {
                            yield return new ApplyVengefulSpiritAction(this, enemy, Value2);
                        }
                    }
                }
            }
            else // If no release, attack and apply as usual.
            {
                yield return AttackAction(selector);
                foreach (var enemy in Battle.AllAliveEnemies)
                {
                    yield return new ApplyVengefulSpiritAction(this, enemy, Value1);
                }
            }
            yield break;
        } 
    }
}


