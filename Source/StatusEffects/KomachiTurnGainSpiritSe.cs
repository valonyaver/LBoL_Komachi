using System.Collections.Generic;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiTurnGainSpiritSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(Spirit) };
            return config;
        }
    }

    [EntityLogic(typeof(KomachiTurnGainSpiritSeDef))]
    public sealed class KomachiTurnGainSpiritSe : StatusEffect
    {

		protected override void OnAdded(Unit unit)
		{
			base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
		}

		private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
		{
            //At the start of the Player's turn, gain Spirit.
			yield return BuffAction<Spirit>(base.Level, 0, 0, 0, 0.2f);
            //This is equivalent to:
            //yield return new ApplyStatusEffectAction<KomachiTurnGainSpiritSe>(base.Owner, base.Level, 0, 0, 0, 0.2f);
			yield break;
		}
    }
}