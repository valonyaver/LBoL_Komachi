using KomachiMod.Config;
using KomachiMod.Localization;
using LBoL.Core;
using LBoL.Core.Intentions;
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
    public sealed class KomachiBossDistanceTaxIntentionDef : IntentionTemplate
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
                main = ResourcesHelper.TryGetIntention(nameof(NegativeEffectIntention), null)
            };
        }

        
    }

    [EntityLogic(typeof(KomachiBossDistanceTaxIntentionDef))]
    public sealed class KomachiBossDistanceTaxIntention : Intention
    {
        public override IntentionType Type
        {
            get
            {
                return IntentionType.NegativeEffect;
            }
        }

        public string PositiveDistanceDescription => LocalizeProperty("PositiveDistanceDescription", true, true);
        public string NegativeDistanceDescription => LocalizeProperty("NegativeDistanceDescription", true, true);
        public int distanceLevel;

        /// <summary>
        /// Creates a displace intention. The intention description will change depending on whether the amount is positive or negative.
        /// If the amount is zero, then it will just say that the amount is unknown.
        /// </summary>
        /// <param name="distanceLevel"></param>
        /// <returns></returns>
        public static Intention Intention(int distanceLevel = 0)
        {
            KomachiBossDistanceTaxIntention distanceTax = TypeFactory<Intention>.CreateInstance<KomachiBossDistanceTaxIntention>();
            distanceTax.distanceLevel = distanceLevel;
            return distanceTax;
        }
    }
}
