using Cysharp.Threading.Tasks;
using KomachiMod.Localization;
using LBoL.ConfigData;
using LBoL.EntityLib.EnemyUnits.Normal.Guihuos;
using LBoL.Presentation;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace KomachiMod.Source.Model
{
    /// <summary>
    /// Template for unit models.
    /// Required for proper localization of enemies, even if they reuse existing game models.
    /// Must have the same name as the enemy class.
    /// If using new spines or pngs later, probably copy what komachi model does.
    /// </summary>
    public class KomachiUnitModelTemplate : UnitModelTemplate
    {
        /// <summary>
        /// Name of the model.
        /// </summary>
        public virtual string model_name => nameof(GuihuoBlue);

        public override IdContainer GetId()
        {
            return GetType().Name;
        }

        public override UnitModelConfig MakeConfig()
        {
            UnitModelConfig config = UnitModelConfig.FromName(model_name).Copy();
            return config;
        }

        public override UniTask<Sprite> LoadSpellSprite()
        {
            return ResourcesHelper.LoadSpellPortraitAsync(model_name);
        }

        public override ModelOption LoadModelOptions()
        {
            //Load the character's spine.
            return new ModelOption(ResourcesHelper.LoadSpineUnitAsync(model_name));
        }
        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.UnitModelBatchLoc.AddEntity(this);
        }
    }
}
