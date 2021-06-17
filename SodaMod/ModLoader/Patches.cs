using HarmonyLib;
using SodaMod.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using SodaLogger = SodaMod.IO.Logger;

namespace SodaMod.ModLoader
{
    [HarmonyPatch(typeof(UIMenuTitle), "Awake")]
    public static class UIMenuTitle_Awake_Patch
    {
        public static void Postfix(UIMenuTitle __instance)
        {
            Sprite smlOptionsSprite = Main.services.bundleManager.Get<Sprite>("ui", "btn_options");

            Button smlOptionsButton = Main.services.uiBuilder.AddImageButtonTo(
                __instance.transform, smlOptionsSprite, 0f, 0f
            );
            smlOptionsButton.onClick.AddListener(new UnityAction(SmlOptionsClickListener));

            Main.services.uiBuilder.AnchorElementToBottomRight(
                (RectTransform) smlOptionsButton.transform, new Vector2(0f, 80f)
            );
        }

        private static void SmlOptionsClickListener()
        {
            SodaLogger.WriteLine("SML button clicked");
            // TODO: fetch mod list from server, then display it
        }
    }
}
