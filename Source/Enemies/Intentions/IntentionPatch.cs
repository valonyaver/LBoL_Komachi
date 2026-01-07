using HarmonyLib;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using LBoL.Presentation.UI.Widgets;
using UnityEngine;

namespace KomachiMod.Source.Enemies.Intentions
{

    [HarmonyPatch(typeof(IntentionWidget), nameof(IntentionWidget.UpdateProperties))]
    public class IntentionWidget_UpdateProperties_Patch
    {
        static void Postfix(IntentionWidget __instance)
        {
            // 1. Get the private _intention field from the widget
            // (AccessTools is a Harmony helper for private fields)
            Intention intention = Traverse.Create(__instance).Field("_intention").GetValue<Intention>();

            if (intention is KomachiBossDisplaceIntention displaceIntention)
            {
                // 2. Get the private text field
                var textMesh = Traverse.Create(__instance).Field("text").GetValue<TMPro.TextMeshProUGUI>();

                // 4. Update the UI
                textMesh.gameObject.SetActive(true);
                textMesh.text = displaceIntention.DisplacementText;

                // Optional: Set a specific color. 
                // Using CountDownColor (blue-ish) as an example, or define your own.
                textMesh.color = IntentionWidget.CountDownColor;
            }
        }
    }
}
