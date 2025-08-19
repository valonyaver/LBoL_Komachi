using KomachiMod.Config;
using KomachiMod.Localization;
using LBoL.Core;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace KomachiMod.Source.Enemies.Intentions
{
    public sealed class KomachiBossDisplaceIntentionDef : IntentionTemplate
    {
        // Token: 0x060001EA RID: 490 RVA: 0x00008B77 File Offset: 0x00006D77
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        // Token: 0x060001EB RID: 491 RVA: 0x00008B84 File Offset: 0x00006D84
        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.IntentionBatchLoc.AddEntity(this);
        }

        // Token: 0x060001EC RID: 492 RVA: 0x00008BA4 File Offset: 0x00006DA4
        public override IntentionImages LoadSprites()
        {
            return new IntentionImages
            {
                main = ResourcesHelper.TryGetIntention("UnknownIntention", null)
            };
        }

        
    }

    [EntityLogic(typeof(KomachiBossDisplaceIntentionDef))]
    public sealed class KomachiBossDisplaceIntention : Intention
    {
        public override IntentionType Type
        {
            get
            {
                return IntentionType.Unknown;
            }
        }

        protected override string GetBaseDescription()
        {
            if (displacementAmount == 0) return BaseDescription;
            else if (displacementAmount > 0) return PositiveDisplacementDescription;
            else return NegativeDisplacementDescription;
        }

        public string PositiveDisplacementDescription => LocalizeProperty("PositiveDisplacementDescription", true, true);
        public string NegativeDisplacementDescription => LocalizeProperty("NegativeDisplacementDescription", true, true);

        public string DisplacementText
        {
            get
            {
                if (displacementAmount != 0)
                {
                    return displacementAmount.ToString();
                }

                return null;
            }
        }
        public int displacementAmount;

        /// <summary>
        /// Creates a displace intention. The intention description will change depending on whether the amount is positive or negative.
        /// If the amount is zero, then it will just say that the amount is unknown.
        /// </summary>
        /// <param name="displacementAmount"></param>
        /// <returns></returns>
        public static Intention Intention(int displacementAmount = 0)
        {
            KomachiBossDisplaceIntention displaceIntention = TypeFactory<Intention>.CreateInstance<KomachiBossDisplaceIntention>();
            displaceIntention.displacementAmount = displacementAmount;
            return displaceIntention;
        }
    }
}
