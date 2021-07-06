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

            Vector4 screenPos = new Vector4(330, 0, 0, 1);

            float w = documentSize.x;
            float h = documentSize.y;
            Matrix4x4 translation = Matrix4x4.Translate(new Vector4(w / 2, h / 2, 0, 1) * -1);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector4(1, -1, 1, 1) / scaleFactor);

            // translate -> scale
            SodaLogger.WriteLine((scale * translation).ToString());
            SodaLogger.WriteLine((scale * translation * screenPos).ToString());
            Vector3 localPos = scale * translation * screenPos;

            // Vector2 localPos = new Vector2(screenPos.x - documentSize.x / 2, -(screenPos.y - documentSize.y / 2)) / scaleFactor;
            rt.localPosition = localPos;
            // Vector2 localPos;
            // Camera uiCamera = Main.services.uiCamera.GetCam();
            // RectTransform parentRt = rt.parent as RectTransform;
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //     parentRt, screenPos, uiCamera, out localPos
            // );
            // SodaLogger.WriteLine("local scale: " + parentRt.GetComponent<CanvasScaler>().scaleFactor);
            SodaLogger.WriteLine("Scale factor: " + scaleFactor);
            // rt.localPosition = localPos + rt.rect.size / 2;
            // rt.anchorMin = rt.anchorMax = new Vector2(0, 0);
            // rt.anchoredPosition = rt.rect.size / 2;
            SodaLogger.WriteLine("Document size: " + documentSize.ToString());
            // Main.services.uiBuilder.AnchorElementToBottomRight(
            //     (RectTransform) smlOptionsButton.transform, new Vector2(0f, 80f)
            // );

            // SodaLogger.WriteLine("Scale factor: " + GameContext.canvasScaleFactor);
            WriteTransformTree(smlOptionsButton.transform.root);
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
                Vector2 localPosition = transform.localPosition;
                Vector2 position = transform.position;

                Vector3[] localCornerList = new Vector3[4];
                (transform as RectTransform).GetWorldCorners(localCornerList);
                string localCorners = "";
                foreach (Vector3 localCorner in localCornerList)
                {
                    localCorners += "(" + localCorner.x + ", " + localCorner.y + ") ";
                }
                // Rect rect = ((RectTransform) transform).rect;
                // Vector2 bottomLeft = uiCamera.WorldToScreenPoint(rect.position);
                // Vector2 bottomLeft = rect.position;
                // Vector2 center = uiCamera.WorldToScreenPoint(rect.center);
                // Vector2 center = rect.center;
                // Vector2 dimensions = new Vector2(rect.width, rect.height);

                SodaLogger.WriteLine(
                    indent + transform.name + ": "
                    // + localCorners
                    + "local: " + localPosition + ", "
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
