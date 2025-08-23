using KomachiMod.Config;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KomachiMod.Source.GunName.GunTests
{
    public class KomachiGunTemplate : GunTemplate
    {
        public override IdContainer GetId()
        {
            return KomachiDefaultConfig.DefaultID(this);
        }

        public override GunConfig MakeConfig()
        {
            return DefaultGunConfig();
        }

        //
        // Summary:
        //     Id : the most important parameter. Maps gun to one or more Pieces,
        //     Name : technically the Id of the GunCOnfig but in reality just a cosmetic name,
        //     Spell : ,
        //     Sequence : Sequence Id,
        //     Animation : "shoot1", "shoot2", "shoot3" or "shoot4",
        //     ForceHitTime: ,
        //     ForceHitAnimation : ,
        //     ForceShowEndStartTime : ,
        //     Shooter : always "Direct"?,
        //     ShakePower : ,
        public static GunConfig DefaultGunConfig()
        {
            return new GunConfig(
                Id: 120000, 
                Name: "", 
                Spell: "", 
                Sequence: "", 
                Animation: "shoot1", 
                ForceHitTime: null, 
                ForceHitAnimation: false, 
                ForceHitAnimationSpeed: 0f, 
                ForceShowEndStartTime: null, 
                Shooter: "Direct", 
                ShakePower: 1f);
        }
    }
}
