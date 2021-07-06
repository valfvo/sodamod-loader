using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using SodaLogger = SodaMod.IO.Logger;

namespace SodaMod.ModLoader
{
    [HarmonyPatch(typeof(UIMenuTitle), "Awake")]
    public static class UIMenuTitle_Awake_Patch
    {
        private static Matrix4x4 translationMatrix;
        private static Matrix4x4 scaleMatrix;

        public static void Postfix(UIMenuTitle __instance)
        {
            Sprite smlOptionsSprite = Main.services.bundleManager.Get<Sprite>("ui", "btn_options");

            Button smlOptionsButton = Main.services.uiBuilder.AddImageButtonTo(
                __instance.transform, smlOptionsSprite, 0f, 0f
            );
            smlOptionsButton.onClick.AddListener(new UnityAction(SmlOptionsClickListener));
            smlOptionsButton.name = "SML Button";

            RectTransform rt = smlOptionsButton.transform as RectTransform;
            RectTransform root = rt.root as RectTransform;
            float scaleFactor = root.GetComponent<CanvasScaler>().scaleFactor;
            Vector2 documentSize = root.rect.size;

            Vector4 screenPosition = new Vector4(330, 0, 0, 1);

            float w = documentSize.x;
            float h = documentSize.y;
            translationMatrix = Matrix4x4.Translate(new Vector4(w / 2, h / 2, 0, 1) * -1);
            scaleMatrix = Matrix4x4.Scale(new Vector4(1, -1, 1, 1) / scaleFactor);

            // translate -> scale
            Vector3 localPosition = scaleMatrix * translationMatrix * screenPosition;
            rt.localPosition = localPosition;

            SodaLogger.WriteLine("Scale factor: " + scaleFactor);
            SodaLogger.WriteLine("Document size: " + documentSize.ToString());

            WriteTransformTree(root);
        }

        private static void SmlOptionsClickListener()
        {
            SodaLogger.WriteLine("SML button clicked");
            // TODO: fetch mod list from server, then display it
        }

        private static void WriteTransformTree(Transform transform, int depth = 0)
        {
            RectTransform rt = transform as RectTransform;
            if (!(rt is null))
            {
                string indent = new string(' ', depth);

                Camera uiCamera = Main.services.uiCamera.GetCam();
                Vector4 localPosition = rt.localPosition;
                localPosition.w = 1;
                Vector2 clientPosition = scaleMatrix * translationMatrix * localPosition;

                SodaLogger.WriteLine(
                    indent + transform.name + ": "
                    // + localCorners
                    + "local: " + localPosition + ", "
                    + "client: " + clientPosition + ", "
                    // + "rt: " + rt.anchorMin + ", "
                    // + "bottomLeft: " + bottomLeft.ToString() + ", "
                    // + "center: " + center.ToString() + ", "
                    // + "dimensions: " + dimensions.ToString()
                );

                foreach (Transform child in transform)
                {
                    WriteTransformTree(child, depth + 1);
                }
            }
        }
    }
}
