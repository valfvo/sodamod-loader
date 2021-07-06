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
        internal static float scaleFactor;
        internal static float documentWidth;
        internal static float documentHeight;
        internal static Matrix4x4 clientToUnityMatrix;
        internal static Matrix4x4 unityToClientMatrix;
        private static Vector4 defaultScale = new Vector4(1, -1, 1, 1);

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
            scaleFactor = root.GetComponent<CanvasScaler>().scaleFactor;
            updateDocumentSize(root.rect.size);

            Vector2 center = rt.rect.size * (scaleFactor / 2);
            Vector4 clientPosition = new Vector4(center.x, center.y, 0, 1);

            Matrix4x4 scaleMatrix = Matrix4x4.Scale(defaultScale / scaleFactor);
            // translate -> scale
            Vector3 unityPosition = scaleMatrix * clientToUnityMatrix * clientPosition;
            rt.localPosition = unityPosition;

            SodaLogger.WriteLine("Scale factor: " + scaleFactor);
            SodaLogger.WriteLine("Document size: (" + documentWidth + ", " + documentHeight + ")");

            WriteTransformTree(root);
        }

        private static void SmlOptionsClickListener()
        {
            SodaLogger.WriteLine("SML button clicked");
            // TODO: fetch mod list from server, then display it
        }

        private static void updateDocumentSize(Vector2 size)
        {
            documentWidth = size.x;
            documentHeight = size.y;

            clientToUnityMatrix = Matrix4x4.Translate(
                new Vector4(-documentWidth / 2, -documentHeight / 2, 0, 1)
            );
            unityToClientMatrix = Matrix4x4.Translate(
                new Vector4(documentWidth / 2, documentHeight / 2, 0, 1)
            );
        }

        private static void WriteTransformTree(Transform transform, int depth = 0)
        {
            RectTransform rt = transform as RectTransform;
            if (!(rt is null))
            {
                string indent = new string(' ', depth);

                Camera uiCamera = Main.services.uiCamera.GetCam();
                Vector4 unityPosition = rt.localPosition;
                unityPosition.w = 1;

                Matrix4x4 scaleMatrix = Matrix4x4.Scale(defaultScale * scaleFactor);
                // scale -> translate
                Vector2 clientPosition = unityToClientMatrix * scaleMatrix * unityPosition;

                SodaLogger.WriteLine(
                    indent + transform.name + ": "
                    + "client: " + clientPosition + ", "
                    + "unity: " + unityPosition + ", "
                );

                foreach (Transform child in transform)
                {
                    WriteTransformTree(child, depth + 1);
                }
            }
        }
    }
}
