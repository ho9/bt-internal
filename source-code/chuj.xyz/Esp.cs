using UnityEngine;

namespace chuj.xyz
{
    public class Esp : MonoBehaviour
    {
        public Vars vars;
        public UI ui;

        private Texture2D outlineTexture;
        private Texture2D boxEspTexture;
        private Texture2D boneEspTexture;


        private Texture2D greenTexture;
        private Texture2D yellowTexture;
        private Texture2D orangeTexture;
        private Texture2D redTexture;

        private string playerName;

        Transform rootBone;

        private void Start()
        {
            vars = FindObjectOfType<Vars>();
            ui = FindObjectOfType<UI>();
            outlineTexture = vars.CreateTexture(Color.black);

            greenTexture = vars.CreateTexture(Color.green);
            yellowTexture = vars.CreateTexture(Color.yellow);
            orangeTexture = vars.CreateTexture(new Color(1f, 0.55f, 0f, 1f));
            redTexture = vars.CreateTexture(Color.red);

        }

        private void Update()
        {
            if (!Application.isFocused || Camera.main == null || vars == null || vars.playerContext == null || ui == null)
                return;

            boxEspTexture = vars.CreateTexture(Color.HSVToRGB(ui.boxEspRGB, 1f, 1f));
            boneEspTexture = vars.CreateTexture(Color.HSVToRGB(ui.boneEspRGB, 1f, 1f));
        }

        private void OnGUI()
        {

            if (!ui.boxEspEnabled && !ui.snaplinesEnabled && !ui.boneEspEnabled && !ui.nameEspEnabled && !ui.showHealthbar)
                return;

            if (!Application.isFocused || Camera.main == null || vars == null || vars.playerContext == null || ui == null || Event.current == null)
                return;

            foreach (PlayerEntity playerEntity in vars.playerContext)
            {
                if (playerEntity == null || playerEntity.isMyPlayer || vars.localPlayerTeam == playerEntity.GetTeam() || playerEntity.IsDead())
                    continue;

                rootBone = playerEntity.GetBone("Bip01");
                Transform headBone = playerEntity.GetBone("Bip01_Head");
                Transform leftFootBone = playerEntity.GetBone("Bip01_L_Foot");
                Transform rightFootBone = playerEntity.GetBone("Bip01_R_Foot");
                Transform leftHandBone = playerEntity.GetBone("Bip01_L_Hand");
                Transform rightHandBone = playerEntity.GetBone("Bip01_R_Hand");

                if (headBone == null || rootBone == null || leftFootBone == null || rightFootBone == null || leftHandBone == null || rightHandBone == null)
                    continue;

                Vector3 headPosition = Camera.main.WorldToScreenPoint(headBone.position);
                Vector3 rootPosition = Camera.main.WorldToScreenPoint(rootBone.position);

                if (rootPosition.z >= 150f)
                {
                    float boxWidth = Mathf.Abs(rootPosition.y - headPosition.y) * 1.5f;
                    float boxHeight = Mathf.Abs(rootPosition.y - headPosition.y) * 3f;

                    Vector2 bottomLeft = new Vector2(rootPosition.x - boxWidth / 2f, rootPosition.y - boxHeight / 2f);
                    Vector2 bottomRight = new Vector2(rootPosition.x + boxWidth / 2f, rootPosition.y - boxHeight / 2f);
                    Vector2 topLeft = new Vector2(rootPosition.x - boxWidth / 2f, rootPosition.y + boxHeight / 2f);
                    Vector2 topRight = new Vector2(rootPosition.x + boxWidth / 2f, rootPosition.y + boxHeight / 2f);

                    if (ui.boxEspEnabled)
                    {
                        DrawLine(topLeft, topRight, outlineTexture, boxEspTexture, 3f, 1f);
                        DrawLine(topRight, bottomRight, outlineTexture, boxEspTexture, 3f, 1f);
                        DrawLine(bottomRight, bottomLeft, outlineTexture, boxEspTexture, 3f, 1f);
                        DrawLine(bottomLeft, topLeft, outlineTexture, boxEspTexture, 3f, 1f);
                    }

                    if (ui.snaplinesEnabled)
                    {
                        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height);
                        Vector2 topCenter = new Vector2((topLeft.x + topRight.x) / 2f, topLeft.y);
                        DrawLine(screenCenter, topCenter, outlineTexture, boxEspTexture, 3f, 1f);
                    }

                    if (ui.boneEspEnabled)
                    {
                        DrawBoneLines(headBone);
                        DrawBoneLines(leftFootBone);
                        DrawBoneLines(rightFootBone);
                        DrawBoneLines(leftHandBone);
                        DrawBoneLines(rightHandBone);
                    }

                    if (ui.nameEspEnabled)
                    {
                        Vector2 labelPosition = new Vector2(bottomLeft.x, Screen.height - bottomLeft.y + 2f);
                        Vector2 labelSize = new Vector2(2000f, 20f);
                        GUIStyle outlineStyle = new GUIStyle(GUI.skin.label);
                        outlineStyle.font = Resources.Load<Font>("Courier New");
                        outlineStyle.fontSize = 12;
                        outlineStyle.alignment = TextAnchor.MiddleLeft;
                        outlineStyle.normal.textColor = Color.black;
                        outlineStyle.wordWrap = false;
                        GUIStyle mainStyle = new GUIStyle(GUI.skin.label);
                        mainStyle.font = Resources.Load<Font>("Courier New");
                        mainStyle.fontSize = 12;
                        mainStyle.alignment = TextAnchor.MiddleLeft;
                        mainStyle.normal.textColor = Color.white;
                        mainStyle.wordWrap = false;
                        if (!playerEntity.IsBot())
                        { playerName = playerEntity.basicInfo.PlayerName.ToString(); }
                        else
                        { playerName = "p2c user"; }
                        GUI.Label(new Rect(labelPosition + new Vector2(2f, 2f), labelSize), playerName, outlineStyle);
                        GUI.Label(new Rect(labelPosition, labelSize), playerName, mainStyle);
                    }

                    if (ui.showHealthbar)
                    {
                        float healthBarHeight = Mathf.Abs(bottomLeft.y - topLeft.y);
                        Vector2 healthBarPosition = new Vector2(bottomLeft.x - 6f, Screen.height - bottomLeft.y);

                        float healthPercentage = (float)playerEntity.GetHpPercent() / 1;

                        float healthBarWidth = 2f;
                        float healthBarFillHeight = healthPercentage * healthBarHeight;

                        Texture2D healthBarTexture;

                        if (healthPercentage > 0.75f)
                            healthBarTexture = greenTexture;
                        else if (healthPercentage > 0.5f)
                            healthBarTexture = yellowTexture;
                        else if (healthPercentage > 0.25f)
                            healthBarTexture = orangeTexture;
                        else
                            healthBarTexture = redTexture;

                        GUI.DrawTexture(new Rect(healthBarPosition.x, healthBarPosition.y - healthBarHeight, healthBarWidth + 1f, healthBarHeight + 1f), outlineTexture, ScaleMode.StretchToFill, true, 0);
                        GUI.DrawTexture(new Rect(healthBarPosition.x, healthBarPosition.y - healthBarFillHeight, healthBarWidth, healthBarFillHeight), healthBarTexture, ScaleMode.StretchToFill, true, 0);
                    }

                }
            }
        }

        private void DrawLine(Vector2 start, Vector2 end, Texture2D outlineTexture, Texture2D espTexture, float outlineThickness, float espThickness)
        {
            Vector2 screenStart = new Vector2(start.x, Screen.height - start.y);
            Vector2 screenEnd = new Vector2(end.x, Screen.height - end.y);

            Vector2 line = screenEnd - screenStart;
            float angle = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            float length = line.magnitude;

            GUIUtility.RotateAroundPivot(angle, screenStart);
            GUI.DrawTexture(new Rect(screenStart.x, screenStart.y - outlineThickness / 2f, length, outlineThickness), outlineTexture, ScaleMode.StretchToFill, true, 0);
            GUI.DrawTexture(new Rect(screenStart.x, screenStart.y - espThickness / 2f, length, espThickness), espTexture, ScaleMode.StretchToFill, true, 0);
            GUIUtility.RotateAroundPivot(-angle, screenStart);
        }

        private void DrawBoneLines(Transform bone)
        {
            if (bone == null || bone.parent == null || bone.parent == rootBone)
                return;

            Vector3 bonePosition = Camera.main.WorldToScreenPoint(bone.position);
            Vector3 parentPosition = Camera.main.WorldToScreenPoint(bone.parent.position);

            DrawLine(bonePosition, parentPosition, outlineTexture, boneEspTexture, 3f, 1f);
            DrawBoneLines(bone.parent);
        }
    }
}
