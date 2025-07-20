using LBoLEntitySideloader.Attributes;
using LBoL.Core.StatusEffects;
using UnityEngine;

namespace KomachiMod.StatusEffects
{
    //Empty status effect that is purely used to define a new pseudo-keyword. 
    //See /DirResources/StatusffectsEn.yaml for the keyword.
    public sealed class KomachiAutoDiscardKeywordDef : KomachiStatusEffectTemplate
    {
        //Keywords don't have sprites.
        public override Sprite LoadSprite() => null;

    }

    [EntityLogic(typeof(KomachiAutoDiscardKeywordDef))]
    public sealed class KomachiAutoDiscardKeyword : StatusEffect
    {
    }
}

