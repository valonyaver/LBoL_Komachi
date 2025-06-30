using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using KomachiMod.GunName;
using LBoL.Core.StatusEffects;
using KomachiMod.StatusEffects;

using KomachiMod.BattleActions;
using LBoL.EntityLib.StatusEffects.Basic;

namespace KomachiMod.KomachiUlt
{
    public sealed class KomachiModUltBDef : KomachiUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 0;
            // Applied spirits
            config.Value1 = 5;
            // Add the relative status effects in the description box.   
            config.RelativeEffects = new List<string>() 
            { 
                nameof(KomachiModVengefulSpiritSe),
                nameof(KomachiDetonationKeyword),
                nameof(KomachiModDivineSpiritSe)
            };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModUltBDef))]
    public sealed class KomachiModUltB : UltimateSkill
    {
        public KomachiModUltB()
        {
            base.TargetType = TargetType.AllEnemies;
            //Need guns to show the application of spirits on enemies. Possible candidates: 6181, 7001, 7031, 7121
            // also ill write the coin throw guns here just because im too lazy to go there: 23031, 22030, 20021, 801/821
            base.GunName = GunNameID.GetGunFromId(7121);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(Owner, nameof(KomachiModUltB));
            foreach (var enemy in selector.GetEnemies(Battle))
            {
                var amulet = enemy.GetStatusEffect<Amulet>();
                if (amulet != null)
                {
                    yield return new RemoveStatusEffectAction(amulet);
                }
                yield return PerformAction.Gun(Owner, enemy, GunName, 0.5f);
                yield return new ApplyVengefulSpiritAction(enemy, Value1);
            }
            int divineSpiritAmount = 0;
            foreach (var enemy in selector.GetEnemies(Battle))
            {
                var detonateAction = new DetonateVengefulSpiritAction(enemy);
                yield return detonateAction;
                divineSpiritAmount += detonateAction.Args.amountDetonated;
            }
            yield return new ApplyStatusEffectAction<KomachiModDivineSpiritSe>(Owner, divineSpiritAmount);
        }
    }
}
