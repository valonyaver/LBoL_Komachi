using KomachiMod.BattleActions;
using KomachiMod.Source.BattleActions.EventManager;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KomachiMod.StatusEffects
{
    public sealed class KomachiModButtonTestSeDef : KomachiStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Positive;
            return config;
        }
    }

    [EntityLogic(typeof(KomachiModButtonTestSeDef))]
    public sealed class KomachiModButtonTestSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            CreateButton();
        }


        void CreateButton()
        {
            // Unit view is the unit model for the owner
            UnitView ownerView = (UnitView)Owner.View;
            // Widget is the status effect view
            StatusEffectWidget statusEffectWidget = GameObject.FindObjectsOfType<StatusEffectWidget>().FirstOrDefault();
            // Create a GameObject for the button
            GameObject buttonObject = GameObject.Instantiate(new GameObject("Test button"), statusEffectWidget.transform);

            // Add a RectTransform component (required for UI elements)
            RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();

            // Set the button's size (width, height)
            rectTransform.sizeDelta = new Vector2(100, 100);

            // Center the button in the canvas
            rectTransform.anchoredPosition = Vector2.zero;

            // Add an Image component (this makes the button visible)
            Image buttonImage = buttonObject.AddComponent<Image>();

            // Set a default color for the button
            buttonImage.color = Color.blue;

            // Add a Button component
            Button button = buttonObject.AddComponent<Button>();

            // Add a Text child GameObject for the button label
            GameObject textObject = new GameObject("ButtonText");
            textObject.transform.SetParent(buttonObject.transform);

            // Add and configure the Text component
            TextMeshProUGUI buttonText = textObject.AddComponent<TextMeshProUGUI>();
            buttonText.text = "Click Me!";
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.color = Color.white;

            // Set the Text's RectTransform to stretch and fill the button
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;


            textRect.localPosition = Vector2.zero;

            // Set the button's parent
            buttonObject.transform.SetParent(statusEffectWidget.transform);
            button.gameObject.AddComponent<ScenePositionTier>();
            button.gameObject.AddComponent<CanvasGroup>();

            Debug.Log($"The parent object's position is at {statusEffectWidget.transform.position}");
            Debug.Log($"The position is at {button.transform.position}");
            Debug.Log($"The effect is {statusEffectWidget.StatusEffect}");
            Debug.Log($"Its sprite renderer is {statusEffectWidget.GetComponent<SpriteRenderer>()}");


            // Add the click listener
            button.onClick.AddListener(() => OnButtonClick());
        }

        void OnButtonClick()
        {
            ((UnitView)Owner.View).Chat("Mimimimimi zzzzzzzzzz", 2);
        }
    }
}