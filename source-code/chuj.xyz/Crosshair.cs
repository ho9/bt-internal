using UnityEngine;

namespace chuj.xyz
{
    public class Crosshair : MonoBehaviour
    {
        public UI ui;

        private void Start ()
        {
            ui = FindObjectOfType<UI>();
        }

        private void OnGUI()
        {
            float centerX = Screen.width / 2f;
            float centerY = Screen.height / 2f;

            // Main crosshair
            GUIStyle crosshairStyle = new GUIStyle(GUI.skin.label);
            crosshairStyle.fontSize = 30;
            crosshairStyle.alignment = TextAnchor.MiddleCenter;
            crosshairStyle.normal.textColor = Color.white;

            // Black outline
            GUIStyle outlineStyle = new GUIStyle(GUI.skin.label);
            outlineStyle.fontSize = 30;
            outlineStyle.alignment = TextAnchor.MiddleCenter;
            outlineStyle.normal.textColor = Color.black;

            if (ui == null || !ui.crosshairEnabled || Event.current == null)
                return;

            // Draw black outline
            GUI.Label(new Rect(centerX - 16f, centerY - 15f, 30f, 30f), "+", outlineStyle);
            GUI.Label(new Rect(centerX - 14f, centerY - 15f, 30f, 30f), "+", outlineStyle);
            GUI.Label(new Rect(centerX - 15f, centerY - 16f, 30f, 30f), "+", outlineStyle);
            GUI.Label(new Rect(centerX - 15f, centerY - 14f, 30f, 30f), "+", outlineStyle);

            // Draw main crosshair
            GUI.Label(new Rect(centerX - 15f, centerY - 15f, 30f, 30f), "+", crosshairStyle);
        }
    }
}