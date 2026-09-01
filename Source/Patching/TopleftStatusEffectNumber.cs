using HarmonyLib;
using LBoL.Core.Battle.BattleActionRecord;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Adventures.FirstPlace;
using LBoL.Presentation.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

namespace KomachiMod.Source.Patching
{
    public interface IHasTopLeftText
    {
        string TopLeftText { get; }
    }

    [HarmonyPatch(typeof(StatusEffectWidget))]
    public static class StatusEffectWidgetTopLeftTextPatch
    {
        private sealed class WidgetState
        {
            public TextMeshProUGUI Label;
            public Action<ActionRecord> ResolvedHandler;
            public Action PropertyChangedHandler;
            public StatusEffect SubscribedEffect;
            public bool Dirty = true;
        }

        private static readonly ConditionalWeakTable<StatusEffectWidget, WidgetState> States = new ConditionalWeakTable<StatusEffectWidget, WidgetState>();

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(StatusEffectWidget __instance)
        {
            TextMeshProUGUI topLeftText = UnityEngine.Object.Instantiate(__instance.upText, __instance.upText.transform.parent);
            topLeftText.name = "TopLeftText";
            topLeftText.outlineColor = Color.black;
            RectTransform rt = (RectTransform)topLeftText.transform;
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(-10f, 5f);
            topLeftText.text = string.Empty;

            WidgetState state = new WidgetState { Label = topLeftText };
            state.ResolvedHandler = _ => state.Dirty = true;
            state.PropertyChangedHandler = () => state.Dirty = true;
            ActionRecord.ActionResolvedHandler += state.ResolvedHandler;

            States.Add(__instance, state);
        }

        [HarmonyPatch("AddHandlers")]
        [HarmonyPostfix]
        static void AddHandlersPostfix(StatusEffectWidget __instance, StatusEffect effect)
        {
            if (States.TryGetValue(__instance, out var state) && effect is IHasTopLeftText)
            {
                effect.PropertyChanged += state.PropertyChangedHandler;
                state.SubscribedEffect = effect;
                state.Dirty = true;
            }
        }

        [HarmonyPatch("RemoveHandlers")]
        [HarmonyPostfix]
        static void RemoveHandlersPostfix(StatusEffectWidget __instance, StatusEffect effect)
        {
            if (States.TryGetValue(__instance, out var state) && state.SubscribedEffect == effect)
            {
                effect.PropertyChanged -= state.PropertyChangedHandler;
                state.SubscribedEffect = null;
            }
        }

        [HarmonyPatch("LateUpdate")]
        [HarmonyPostfix]
        static void LateUpdatePostfix(StatusEffectWidget __instance)
        {
            if (!States.TryGetValue(__instance, out var state) || !state.Dirty)
                return;

            state.Dirty = false;
            state.Label.text = __instance._statusEffect is IHasTopLeftText hasTopLeftText
                ? hasTopLeftText.TopLeftText
                : string.Empty;
        }

        [HarmonyPatch("OnDestroy")]
        [HarmonyPostfix]
        static void OnDestroyPostfix(StatusEffectWidget __instance)
        {
            if (!States.TryGetValue(__instance, out var state))
                return;

            ActionRecord.ActionResolvedHandler -= state.ResolvedHandler;
            if (state.SubscribedEffect != null)
            {
                state.SubscribedEffect.PropertyChanged -= state.PropertyChangedHandler;
                state.SubscribedEffect = null;
            }

            States.Remove(__instance);
        }
    }
}
