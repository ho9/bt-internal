using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace chuj.xyz
{
    public class Aimbot : MonoBehaviour
    {

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public Vars vars;
        public UI ui;

        private Vector2 aimTarget;
        private Transform bone;
        private Vector3 screenPosition;
        private float actualFov;
        private float closestDist = float.MaxValue;
        private PlayerEntity closestEntity = null;

        private Vector3 cameraPosition;
        private Vector3 crosshairPosition;

        private KeyCode aimbotKeyCode;

        private Material circleMaterial;


        private void Start()
        {
            vars = FindObjectOfType<Vars>();
            ui = FindObjectOfType<UI>();
            circleMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        private void OnRenderObject()
        {
            if (ui.aimbotEnabled && ui.drawAimbotFov)
            {
                float fovRadius = actualFov / 2f;
                Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
                DrawCircle(Color.white, center, fovRadius);
            }
        }


        private void Update()
        {
            actualFov = ui.aimbotFov * (Screen.height / 180f);
            try
            {
                aimbotKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), ui.aimbotKey);
            }
            catch
            {
                aimbotKeyCode = KeyCode.LeftAlt;
            }

            if (!Application.isFocused || !ui.aimbotEnabled || Camera.main == null || vars.playerContext == null || Event.current == null)
                return;

            closestDist = float.MaxValue;
            closestEntity = null;

            cameraPosition = Camera.main.transform.position;
            crosshairPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));

            foreach (PlayerEntity playerEntity in vars.playerContext)
            {

                if (playerEntity == null || playerEntity.isMyPlayer || vars.localPlayerTeam == playerEntity.GetTeam() || playerEntity.IsDead())
                    continue;

                bone = ui.aimbotHead ? playerEntity.GetBone("Bip01_Head") : playerEntity.GetBone("Bip01_Spine");

                if (bone == null || bone.transform == null)
                    continue;

                float distCamera = Vector3.Distance(cameraPosition, bone.transform.position);
                float distCrosshair = Vector3.Distance(crosshairPosition, bone.transform.position);
                float dist = distCamera + distCrosshair;

                if (dist < closestDist)
                {
                    screenPosition = Camera.main.WorldToScreenPoint(bone.transform.position);

                    if (screenPosition.z >= 0.01f)
                    {
                        float screenDist = Mathf.Abs(Vector2.Distance(new Vector2(screenPosition.x, Screen.height - screenPosition.y), new Vector2(Screen.width / 2f, Screen.height / 2f)));

                        if (screenDist < actualFov)
                        {
                            closestDist = dist;
                            closestEntity = playerEntity;
                        }
                    }
                }
            }

            if (closestEntity != null)
            {
                bone = ui.aimbotHead ? closestEntity.GetBone("Bip01_Head") : closestEntity.GetBone("Bip01_Spine");

                if (bone != null && bone.transform != null)
                {
                    screenPosition = Camera.main.WorldToScreenPoint(bone.transform.position);
                    aimTarget = new Vector2(screenPosition.x, Screen.height - screenPosition.y);


                    double DistX = aimTarget.x - Screen.width / 2.0f;
                    double DistY = aimTarget.y - Screen.height / 2.0f;

                    if (ui.rcsEnabled)
                    {
                        double punchYaw = vars.localPlayer.GetPunchYaw();
                        double punchPitch = vars.localPlayer.GetPunchPitch();

                        DistX += punchYaw * 15;
                        DistY += punchPitch * 15;
                    }

                    DistX /= ui.aimbotSmooth;
                    DistY /= ui.aimbotSmooth;

                    if (ui != null && ui.waitingText != null && ui.aimbotKey != ui.waitingText && Input.GetKey(aimbotKeyCode))
                        mouse_event(0x1, (int)DistX, (int)DistY, 0, 0);
                }
            }
        }
        private void DrawCircle(Color color, Vector2 center, float radius)
        {
            GL.PushMatrix();

            if (!circleMaterial.SetPass(0))
            {
                GL.PopMatrix();
                return;
            }

            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);
            GL.Color(color);

            float angleStep = 2f * Mathf.PI / 360f;
            float currentAngle = 0f;

            while (currentAngle < 2f * Mathf.PI)
            {
                float x1 = center.x + Mathf.Sin(currentAngle) * radius;
                float y1 = center.y + Mathf.Cos(currentAngle) * radius;

                currentAngle += angleStep;

                float x2 = center.x + Mathf.Sin(currentAngle) * radius;
                float y2 = center.y + Mathf.Cos(currentAngle) * radius;

                GL.Vertex3(x1, Screen.height - y1, 0f);
                GL.Vertex3(x2, Screen.height - y2, 0f);
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}
