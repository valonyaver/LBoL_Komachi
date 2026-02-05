using KomachiMod.Config;
using KomachiMod.ImageLoader;
using KomachiMod.Source.GunName.GunTests;
using LBoL.ConfigData;
using LBoL.Presentation.Effect;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KomachiMod.Source.GunName.Bullets
{
    //public sealed class KomachiModTestBullet1Def : BulletTemplate
    //{
    //    public override IdContainer GetId()
    //    {
    //        return KomachiDefaultConfig.DefaultID(this);
    //    }

    //    public override BulletConfig MakeConfig()
    //    {
    //        BulletConfig config = DefaultConfig();
    //        config.Name = "KomachiTestBullet";
    //        config.Widget = "KomachiModTrueDanma";

    //        Debug.Log($"Logging the config for {config.Name}");
    //        return config;
    //    }
    //}

    //public sealed class KomachiModTrueDanmaDef : EffectTemplate
    //{
    //    public override IdContainer GetId()
    //    {
    //        return KomachiDefaultConfig.DefaultID(this);
    //    }

    //    public override EffectWidgetData LoadEffectData()
    //    {
    //        Debug.Log($"Loading the truedanma image");
    //        // The name of your image file (without extension or path)
    //        string imageName = "True";

    //        // The GameObject that will hold your sprite
    //        GameObject effectGameObject = KomachiImageLoader.LoadEffectGameObject(imageName);

    //        // Create a queue with default properties for the particle system
    //        Queue<EffectWidgetData.ExtraElementProperties> particleProps = new Queue<EffectWidgetData.ExtraElementProperties>();
    //        particleProps.Enqueue(new EffectWidgetData.ExtraElementProperties
    //        {
    //            changeColor = true, // Set to true if you want to be able to tint the effect
    //            dieType = EffectWidget.DieType.Inactivate,
    //            lowPerformance = false
    //        });

    //        // Create the EffectWidgetData with your GameObject
    //        return new EffectWidgetData(
    //            effectGameObject,
    //            particleProps,
    //            trailRendererProperties: null,
    //            sortingLayer: EffectWidgetData.SortingLayer.Bullet,
    //            nameOverwrite: UniqueId
    //        );
    //    }

    //    public override EffectConfig MakeConfig()
    //    {
    //        string effectName = GetId().ToString();
    //        string effectPath = "Effects/" + effectName;
    //        float lifeTime = 0f;

    //        EffectConfig config = new EffectConfig(
    //            Name: effectName,
    //            Path: effectPath,
    //            Life: lifeTime
    //        );

    //        return config;
    //    }
    //}
}
