using HarmonyLib;
using KomachiMod.Source.BattleActions.Helpers;
using KomachiMod.StatusEffects;
using LBoL.Core.Battle.BattleActionRecord;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KomachiMod.Source.Patching
{
    [HarmonyPatch(typeof(UnitInfoWidget))]
    public static class UnitInfoWidgetDistancePreviewPatch
    {
        private static readonly ConditionalWeakTable<UnitInfoWidget, KomachiDistancePreviewWidget> 
            Previews = new ConditionalWeakTable<UnitInfoWidget, KomachiDistancePreviewWidget>();

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(UnitInfoWidget __instance)
        {
            GameObject go = new GameObject("KomachiDistancePreview", typeof(RectTransform));
            go.transform.SetParent(__instance.intentionRoot, worldPositionStays: false);

            KomachiDistancePreviewWidget preview = go.AddComponent<KomachiDistancePreviewWidget>();
            preview.Initialize(__instance.intentionTemplate.text);

            Previews.Add(__instance, preview);
        }

        [HarmonyPatch("set_Unit")]
        [HarmonyPostfix]
        static void SetUnitPostfix(UnitInfoWidget __instance, Unit value)
        {
            if (Previews.TryGetValue(__instance, out var preview))
                preview.SetUnit(value);
        }
    }

    public sealed class KomachiDistancePreviewWidget : MonoBehaviour
    {
        private TextMeshProUGUI[] _labels;
        private CanvasGroup _canvasGroup;
        private Image _raycastImage;
        private EnemyUnit _enemy;
        private KomachiModDistanceSe _distance;
        private Action<ActionRecord> _resolvedHandler;
        private Action<EnemyUnit> _intentionsHandler;
        private bool _dirty = true;

        private static readonly Color CurrentColor = Color.white;
        private static readonly Color OtherColor = new Color(0.6f, 0.6f, 0.6f, 1f);


        public string TooltipDescription { get; private set; } = string.Empty;
        public string TooltipTitle { get; private set; } = "Distance";
        public void Initialize(TextMeshProUGUI template)
        {
            const float chipSize = 34f;
            const float gap = 40f;
            float totalWidth = 5 * chipSize + 4 * gap;

            RectTransform selfRt = (RectTransform)transform;
            selfRt.anchorMin = new Vector2(0.5f, 1f);
            selfRt.anchorMax = new Vector2(0.5f, 1f);
            selfRt.pivot = new Vector2(0.5f, 0.5f);
            selfRt.anchoredPosition = new Vector2(0f, 150f);
            selfRt.sizeDelta = new Vector2(totalWidth, chipSize);

            _raycastImage = gameObject.AddComponent<Image>();
            _raycastImage.color = new Color(0f, 0f, 0f, 0f); // invisible, but raycastable so hover works
            _raycastImage.raycastTarget = true;

            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            _labels = new TextMeshProUGUI[5];
            float startX = -totalWidth / 2f + chipSize / 2f;
            for (int i = 0; i < 5; i++)
            {
                TextMeshProUGUI label = UnityEngine.Object.Instantiate(template, transform); // parented to self now, not intentionRoot
                label.gameObject.SetActive(true);
                label.gameObject.name = $"DistancePreview_{i + 1}";

                RectTransform rt = (RectTransform)label.transform;
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(startX + i * (chipSize + gap), 0f);
                rt.sizeDelta = new Vector2(chipSize, chipSize);

                label.text = string.Empty;
                _labels[i] = label;
            }

            _resolvedHandler = _ => _dirty = true;
            ActionRecord.ActionResolvedHandler += _resolvedHandler;

            KomachiDistancePreviewTooltipSource tooltip = gameObject.AddComponent<KomachiDistancePreviewTooltipSource>();
            tooltip.widget = this;
        }

        public void SetUnit(Unit unit)
        {
            if (_enemy != null && _intentionsHandler != null)
                _enemy.IntentionsChanged -= _intentionsHandler;

            _enemy = unit as EnemyUnit;
            _distance = null; 
            if (_enemy != null)
            {
                _intentionsHandler = _ => _dirty = true;
                _enemy.IntentionsChanged += _intentionsHandler;
            }

            _dirty = true;
        }

        private void Update()
        {
            if (!_dirty) return;
            _dirty = false;
            Refresh();
        }

        private void Refresh()
        {
            if (_distance == null && _enemy != null)
                _enemy.TryGetStatusEffect(out _distance);

            bool hasAttackDamage = _enemy != null && _enemy.HasAnyAttackTypeIntention();

            if (_distance == null || !hasAttackDamage)
            {
                SetVisible(false);
                return;
            }

            SetVisible(true);
            int currentLevel = _distance.Level;
            var preview = KomachiModDistanceSe.PreviewDamageByDistanceLevel(_enemy);

            for (int level = 1; level <= 5; level++)
            {
                TextMeshProUGUI label = _labels[level - 1];
                label.text = preview[level].ToString();
                label.color = (level == currentLevel) ? CurrentColor : OtherColor;
            }

            TooltipDescription = _distance.previewDescription;
            TooltipTitle = _distance.Name;
        }

        private void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1f : 0f;
            _canvasGroup.blocksRaycasts = visible;
            _raycastImage.raycastTarget = visible;
        }

        private void OnDestroy()
        {
            if (_resolvedHandler != null)
                ActionRecord.ActionResolvedHandler -= _resolvedHandler;
            if (_enemy != null && _intentionsHandler != null)
                _enemy.IntentionsChanged -= _intentionsHandler;
        }
    }

    [RequireComponent(typeof(KomachiDistancePreviewWidget))]
    public sealed class KomachiDistancePreviewTooltipSource : TooltipSource
    {
        private static readonly TooltipPosition[] Positions = new TooltipPosition[1]
        {
        new TooltipPosition(TooltipDirection.Left, TooltipAlignment.Max)
        };

        public KomachiDistancePreviewWidget widget;

        public override RectTransform TargetRectTransform => (RectTransform)widget.transform;
        public override TooltipPosition[] TooltipPositions => Positions;
        public override string Title => widget.TooltipTitle;
        public override string Description => widget.TooltipDescription;
    }
}
