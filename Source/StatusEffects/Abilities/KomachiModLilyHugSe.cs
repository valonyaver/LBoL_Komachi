using KomachiMod.BattleActions;
using KomachiMod.Cards;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModLilyHugSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        { 
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = LBoL.Base.StatusEffectType.Positive;

            config.RelativeEffects = new List<string>() { nameof(Amulet) };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModLilyHugSeDef))]
    public sealed class KomachiModLilyHugSe : StatusEffect
    {

        // Token: 0x06000025 RID: 37 RVA: 0x00002470 File Offset: 0x00000670
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        // Token: 0x06000026 RID: 38 RVA: 0x0000248F File Offset: 0x0000068F
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd && args.Card is KomachiModSpiderLily)
            {
                yield return BuffAction<Amulet>(Level);
            }
            yield break;
        }
    }
}