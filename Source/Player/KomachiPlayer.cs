using Cysharp.Threading.Tasks;
using KomachiMod.ImageLoader;
using KomachiMod.Localization;
//using DG.Tweening;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.Presentation.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
//using KomachiMod.BattleActions;

namespace KomachiMod
{
    public sealed class KomachiModDef : PlayerUnitTemplate
    {        
        public UniTask<Sprite>? LoadSpellPortraitAsync { get; private set; }

        public override IdContainer GetId()
        {
            return BepinexPlugin.modUniqueID;
        }

        public override LocalizationOption LoadLocalization()
        {
            return KomachiLocalization.PlayerUnitBatchLoc.AddEntity(this);
        }

        public override EikiSummonInfo AssociateEikiSummon()
        {
            return new EikiSummonInfo(typeof(Enemies.KomachiMod));
        }

        public override PlayerImages LoadPlayerImages()
        {
            return KomachiImageLoader.LoadPlayerImages(BepinexPlugin.playerName);
        }

        public override PlayerUnitConfig MakeConfig()
        {
            return KomachiModLoadouts.playerUnitConfig;
        }

        [EntityLogic(typeof(KomachiModDef))]
        public sealed class KomachiMod : PlayerUnit 
        {
            public string EikiDialogue
            {
                get
                {
                    return this.LocalizeProperty("EikiDialogue", true, true);
                }
            }
            protected override void OnEnterBattle(BattleController battle)
            {
                foreach(var enemy in battle.AllAliveEnemies)
                {
                    if (enemy.GetType() == typeof(Siji))
                    {
                        UnitView view = GetView<UnitView>();
                        view.Chat(EikiDialogue, 2, LBoL.Presentation.UI.Widgets.ChatWidget.CloudType.LeftTalk);
                    }
                }
            }
        }
    }
}