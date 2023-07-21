using Assets.Sources.Config;
using System.Runtime.InteropServices;
using UnityEngine;

namespace chuj.xyz
{
    public class UI : MonoBehaviour
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern int ShowCursor(bool show);

        public Vars vars;

        private float delta_time = 0.0f;
        private float fps;

        private Texture2D menuBackground;
        private Texture2D menuForeground;
        private Texture2D menuNormal;
        private Texture2D menuHover;
        private Texture2D boxColor;

        private int menuId = 1337;
        private Rect menuRect;
        private bool drawMenu = false;
        private int tabs;
        private string[] toolbar_strings = { "Aim", "Visuals", "Misc", "Settings" };

        // Aimbot
        public bool aimbotEnabled = false;
        public float aimbotFov = 30f;
        public float aimbotSmooth = 5f;
        public bool aimbotHead = true;
        public bool aimbotChest = false;
        public bool drawAimbotFov = false;
        public bool rcsEnabled = false;

        // Visuals (ESP)
        public bool boxEspEnabled = false;
        public bool snaplinesEnabled = false;
        public bool boneEspEnabled = false;
        public float boxEspRGB = 0f;
        public float boneEspRGB = 0f;

        // Visuals (Chams)
        public bool chamsEnabled = false;
        public bool weaponChamsEnabled = false;
        public bool handChamsEnabled = false;
        public bool chamsFlat = true;
        public bool chamsAdditive = false;
        public bool chamsGlow = false;
        public float chamsRGB = 0f;
        public float weaponRGB = 0f;
        public float handsRGB = 0f;

        // Visuals (Flags)
        public bool nameEspEnabled = false;
        public bool showHealthbar = false;
        

        // Misc
        public bool crosshairEnabled = false;
        public bool viewmodelFovEnabled = false;
        public float viewmodelFovValue = 90f;
        public bool bhopEnabled = false;

        // Settings
        private bool isWaitingForKey = false;
        public string waitingText = "[ . . . ]";
        public string aimbotKey = KeyCode.LeftAlt.ToString();


        private void Start()
        {
            vars = FindObjectOfType<Vars>();

            // Cache menu textures
            menuBackground = vars.CreateTexture(new Color(0f, 0f, 0f, 1f));
            menuForeground = vars.CreateTexture(new Color(0.5f, 0f, 0.6f, 1f));
            menuNormal = vars.CreateTexture(new Color(0.1f, 0.1f, 0.1f, 1f));
            menuHover = vars.CreateTexture(new Color(0.4f, 0.4f, 0.4f, 1f));
            boxColor = vars.CreateTexture(new Color(0.06f, 0.06f, 0.06f, 1f));

            menuRect = new Rect((Screen.width / 2) - 200f, (Screen.height / 2) - 255f, 400f, 510f);

            if (GameSetting.Config != null && GameSetting.Config.RawInput != 0)
            {
                GameSetting.Config.RawInput = 0;
                // This removes the fps cap
                //GameSetting.Config.FrameLimit = 1337;
                //Application.targetFrameRate = 1337;
            }

        }

        private void MainWindow(int id)
        {
            GUISkin customSkin = GUI.skin;

            // Main window
            customSkin.window.font = Resources.Load<Font>("Courier New");
            customSkin.window.normal.background = menuBackground;
            customSkin.window.onNormal.background = menuBackground;
            customSkin.window.focused.background = menuBackground;
            customSkin.window.hover.background = menuBackground;
            customSkin.window.active.background = menuBackground;
            customSkin.window.onActive.background = menuBackground;
            customSkin.window.onFocused.background = menuBackground;
            customSkin.window.onHover.background = menuBackground;
            customSkin.window.normal.textColor = Color.white;
            customSkin.window.onNormal.textColor = Color.white;
            customSkin.window.focused.textColor = Color.white;
            customSkin.window.hover.textColor = Color.white;
            customSkin.window.active.textColor = Color.white;
            customSkin.window.onActive.textColor = Color.white;
            customSkin.window.onFocused.textColor = Color.white;
            customSkin.window.onHover.textColor = Color.white;

            // Buttons
            customSkin.button.font = Resources.Load<Font>("Courier New");
            customSkin.button.normal.background = menuNormal;
            customSkin.button.onNormal.background = menuNormal;
            customSkin.button.focused.background = menuHover;
            customSkin.button.hover.background = menuHover;
            customSkin.button.active.background = menuHover;
            customSkin.button.onActive.background = menuHover;
            customSkin.button.onFocused.background = menuHover;
            customSkin.button.onHover.background = menuHover;
            customSkin.button.normal.textColor = Color.white;
            customSkin.button.onNormal.textColor = Color.white;
            customSkin.button.focused.textColor = Color.white;
            customSkin.button.hover.textColor = Color.white;
            customSkin.button.active.textColor = Color.white;
            customSkin.button.onActive.textColor = Color.white;
            customSkin.button.onFocused.textColor = Color.white;
            customSkin.button.onHover.textColor = Color.white;

            // Labels
            customSkin.label.font = Resources.Load<Font>("Courier New");
            customSkin.label.normal.textColor = Color.white;

            // Checkboxes
            customSkin.toggle.font = Resources.Load<Font>("Courier New");
            customSkin.toggle.normal.background = menuNormal;
            customSkin.toggle.onNormal.background = menuNormal;
            customSkin.toggle.focused.background = menuHover;
            customSkin.toggle.hover.background = menuHover;
            customSkin.toggle.active.background = menuHover;
            customSkin.toggle.onActive.background = menuHover;
            customSkin.toggle.onFocused.background = menuHover;
            customSkin.toggle.onHover.background = menuHover;

            // Sliders
            customSkin.horizontalSlider.font = Resources.Load<Font>("Courier New");
            customSkin.horizontalSlider.normal.background = menuNormal;
            customSkin.horizontalSlider.onNormal.background = menuNormal;
            customSkin.horizontalSlider.focused.background = menuHover;
            customSkin.horizontalSlider.hover.background = menuHover;
            customSkin.horizontalSlider.active.background = menuHover;
            customSkin.horizontalSlider.onActive.background = menuHover;
            customSkin.horizontalSlider.onFocused.background = menuHover;
            customSkin.horizontalSlider.onHover.background = menuHover;
            customSkin.horizontalSliderThumb.font = Resources.Load<Font>("Courier New");
            customSkin.horizontalSliderThumb.normal.background = menuNormal;
            customSkin.horizontalSliderThumb.onNormal.background = menuForeground;
            customSkin.horizontalSliderThumb.focused.background = menuHover;
            customSkin.horizontalSliderThumb.hover.background = menuHover;
            customSkin.horizontalSliderThumb.active.background = menuForeground;
            customSkin.horizontalSliderThumb.onActive.background = menuForeground;
            customSkin.horizontalSliderThumb.onFocused.background = menuForeground;
            customSkin.horizontalSliderThumb.onHover.background = menuForeground;

            // Boxes
            customSkin.box.normal.background = boxColor;

            // Apply the modified GUI skin
            GUI.skin = customSkin;

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.alignment = TextAnchor.MiddleLeft;

            GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.normal.background = menuNormal;
            toggleStyle.onNormal.background = menuForeground;
            toggleStyle.hover.background = menuHover;
            toggleStyle.active.background = menuHover;
            toggleStyle.onHover.background = menuForeground;
            toggleStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.background = menuNormal;
            buttonStyle.onNormal.background = menuForeground;
            buttonStyle.hover.background = menuHover;
            buttonStyle.active.background = menuHover;
            buttonStyle.onHover.background = menuForeground;
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle sliderStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
            sliderStyle.normal.background = menuNormal;
            sliderStyle.onNormal.background = menuForeground;
            sliderStyle.hover.background = menuHover;
            sliderStyle.active.background = menuHover;
            sliderStyle.onHover.background = menuForeground;

            GUILayout.Space(5);
            tabs = GUI.Toolbar(new Rect(20, 20, 380, 25), tabs, toolbar_strings, buttonStyle);
            GUILayout.Space(20);

            switch (tabs)
            {
                case 0: // Aimbot
                    GUILayout.Space(10f);
                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(120f));

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Aimbot", GUILayout.Width(80f));
                    aimbotEnabled = GUILayout.Toggle(aimbotEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Draw FOV", GUILayout.Width(80f));
                    drawAimbotFov = GUILayout.Toggle(drawAimbotFov, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("RCS", GUILayout.Width(80f));
                    rcsEnabled = GUILayout.Toggle(rcsEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("FOV: " + (int)aimbotFov, labelStyle, GUILayout.Width(80f));
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    aimbotFov = GUILayout.HorizontalSlider(aimbotFov, 10f, 180f);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Smooth: " + (int)aimbotSmooth, labelStyle, GUILayout.Width(80f));
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    aimbotSmooth = GUILayout.HorizontalSlider(aimbotSmooth, 1f, 10f);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Target", labelStyle, GUILayout.Width(80f));
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Toggle(aimbotHead, "Head", buttonStyle, GUILayout.Width(60f)))
                    {
                        aimbotHead = true;
                        aimbotChest = false;
                    }
                    if (GUILayout.Toggle(aimbotChest, "Chest", buttonStyle, GUILayout.Width(60f)))
                    {
                        aimbotChest = true;
                        aimbotHead = false;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();


                    GUILayout.EndVertical();
                    break;

                case 1: // Visuals
                    GUILayout.Space(10f);

                    // Box ESP section
                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(80f));
                    GUILayout.BeginHorizontal();

                    GUILayout.Label("Box ESP", GUILayout.Width(80f));
                    boxEspEnabled = GUILayout.Toggle(boxEspEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Snaplines", GUILayout.Width(80f));
                    snaplinesEnabled = GUILayout.Toggle(snaplinesEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Color", GUILayout.Width(80f));

                    Color previewBoxEspColor = Color.HSVToRGB(boxEspRGB, 1f, 1f);
                    GUIStyle boxEspColorPreviewStyle = new GUIStyle(GUI.skin.box);
                    boxEspColorPreviewStyle.normal.background = vars.CreateTexture(previewBoxEspColor);
                    GUILayout.Box(GUIContent.none, boxEspColorPreviewStyle, GUILayout.Width(20f), GUILayout.Height(20f));

                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    boxEspRGB = GUILayout.HorizontalSlider(boxEspRGB, 0f, 1f);
                    GUILayout.EndVertical();


                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);


                    // Bone ESP section
                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(55f));

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Bone ESP", GUILayout.Width(80f));
                    boneEspEnabled = GUILayout.Toggle(boneEspEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Color", GUILayout.Width(80f));

                    Color previewBoneEspColor = Color.HSVToRGB(boneEspRGB, 1f, 1f);
                    GUIStyle boneEspColorPreviewStyle = new GUIStyle(GUI.skin.box);
                    boneEspColorPreviewStyle.normal.background = vars.CreateTexture(previewBoneEspColor);
                    GUILayout.Box(GUIContent.none, boneEspColorPreviewStyle, GUILayout.Width(20f), GUILayout.Height(20f));

                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    boneEspRGB = GUILayout.HorizontalSlider(boneEspRGB, 0f, 1f);
                    GUILayout.EndVertical();


                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);

                    // Chams toggle controls
                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(80f));

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Chams", GUILayout.Width(80f));
                    chamsEnabled = GUILayout.Toggle(chamsEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Color", GUILayout.Width(80f));
                    Color previewChamsColor = Color.HSVToRGB(chamsRGB, 1f, 1f);
                    GUIStyle chamsPreviewStyle = new GUIStyle(GUI.skin.box);
                    chamsPreviewStyle.normal.background = vars.CreateTexture(previewChamsColor);
                    GUILayout.Box(GUIContent.none, chamsPreviewStyle, GUILayout.Width(20f), GUILayout.Height(20f));
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    chamsRGB = GUILayout.HorizontalSlider(chamsRGB, 0f, 1f);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Weapon", GUILayout.Width(80f));
                    weaponChamsEnabled = GUILayout.Toggle(weaponChamsEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Color", GUILayout.Width(80f));
                    Color previewWeaponColor = Color.HSVToRGB(weaponRGB, 1f, 1f);
                    GUIStyle weaponPreviewStyle = new GUIStyle(GUI.skin.box);
                    weaponPreviewStyle.normal.background = vars.CreateTexture(previewWeaponColor);
                    GUILayout.Box(GUIContent.none, weaponPreviewStyle, GUILayout.Width(20f), GUILayout.Height(20f));
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    weaponRGB = GUILayout.HorizontalSlider(weaponRGB, 0f, 1f);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Viewmodel", GUILayout.Width(80f));
                    handChamsEnabled = GUILayout.Toggle(handChamsEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Color", GUILayout.Width(80f));
                    Color previewViewmodelColor = Color.HSVToRGB(handsRGB, 1f, 1f);
                    GUIStyle viewmodelPreviewStyle = new GUIStyle(GUI.skin.box);
                    viewmodelPreviewStyle.normal.background = vars.CreateTexture(previewViewmodelColor);
                    GUILayout.Box(GUIContent.none, viewmodelPreviewStyle, GUILayout.Width(20f), GUILayout.Height(20f));
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    handsRGB = GUILayout.HorizontalSlider(handsRGB, 0f, 1f);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Type", GUILayout.Width(80f));

                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();

                    // Chams type buttons
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Toggle(chamsFlat, "Flat", buttonStyle, GUILayout.Width(60f)))
                    {
                        chamsAdditive = false;
                        chamsGlow = false;
                        chamsFlat = true;
                    }
                    if (GUILayout.Toggle(chamsAdditive, "Additive", buttonStyle, GUILayout.Width(80f)))
                    {
                        chamsFlat = false;
                        chamsGlow = false;
                        chamsAdditive = true;
                    }
                    if (GUILayout.Toggle(chamsGlow, "Glow", buttonStyle, GUILayout.Width(60f)))
                    {
                        chamsFlat = false;
                        chamsAdditive = false;
                        chamsGlow = true;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);

                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(20f));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Name ESP", GUILayout.Width(80f));
                    nameEspEnabled = GUILayout.Toggle(nameEspEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);

                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(20f));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Healthbar", GUILayout.Width(80f));
                    showHealthbar = GUILayout.Toggle(showHealthbar, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                    break;
                case 2: // Misc
                    GUILayout.Space(10f);

                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(20f));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Crosshair", GUILayout.Width(80f));
                    crosshairEnabled = GUILayout.Toggle(crosshairEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);



                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(20f));

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Viewmodel", GUILayout.Width(80f));
                    viewmodelFovEnabled = GUILayout.Toggle(viewmodelFovEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    GUILayout.Label("FOV: " + (int)viewmodelFovValue, labelStyle, GUILayout.Width(80f));
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    viewmodelFovValue = GUILayout.HorizontalSlider(viewmodelFovValue, 60f, 120f);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    GUILayout.Space(5f);

                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(20f));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Bhop", GUILayout.Width(80f));
                    bhopEnabled = GUILayout.Toggle(bhopEnabled, GUIContent.none, buttonStyle, GUILayout.Width(20f), GUILayout.Height(19f));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();


                    break;
                case 3: // Settings
                    GUILayout.Space(10f);
                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(20f));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Aimbot key", GUILayout.Width(80f));
                    
                    if (!isWaitingForKey)
                    {
                        if (GUILayout.Button(aimbotKey, GUILayout.Width(80f)))
                        {
                            aimbotKey = waitingText;
                            isWaitingForKey = true;
                        }
                    }
                    else
                    {
                        GUILayout.Button(waitingText, GUILayout.Width(80f));
                    }
                    GUILayout.FlexibleSpace();
                    if (isWaitingForKey && Event.current.isKey)
                    {
                        KeyCode keyCode = Event.current.keyCode;
                        aimbotKey = keyCode.ToString();
                        isWaitingForKey = false;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.Space(5f);
                    GUILayout.BeginVertical(customSkin.box, GUILayout.Width(400f), GUILayout.Height(20f));
                    GUILayout.BeginHorizontal();
                    
                    if (GUILayout.Button("Unload", GUILayout.Width(80f)))
                    {
                        ShowCursor(false);
                        Loader.Unload();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                    break;
            }

            GUILayout.Space(15f);
            GUI.DragWindow();
        }

        public void Update()
        {
            delta_time += (Time.unscaledDeltaTime - delta_time) * 0.1f;
            fps = 1.0f / delta_time;

            if (!Input.anyKey || !Input.anyKeyDown) return;

            if (Input.GetKeyDown(KeyCode.Insert))
            { 
                drawMenu = !drawMenu;

                // Mouse flickering fix
                if (drawMenu) ShowCursor(true);
                else ShowCursor(false);
            }
        }

        public void OnGUI()
        {
            if (Event.current == null)
                return;
            GUIStyle watermarkStyle = new GUIStyle(GUI.skin.label);
            watermarkStyle.font = Resources.Load<Font>("Courier New");
            watermarkStyle.fontSize = 12;
            watermarkStyle.normal.textColor = Color.white;
            watermarkStyle.normal.background = menuBackground;
            watermarkStyle.alignment = TextAnchor.MiddleCenter;
            Rect label_rect = new Rect(1f, 1f, 200f, 30f);
            Rect bottom_line = new Rect(label_rect.x, label_rect.yMax, label_rect.width, 3f);
            GUI.Label(label_rect, "chuj.xyz  |  fps: " + Mathf.Round(fps), watermarkStyle);
            GUI.DrawTexture(bottom_line, menuForeground);

            if (drawMenu)
            {
                Cursor.visible = true;
                menuRect = GUILayout.Window(menuId, menuRect, MainWindow, "chuj.xyz");
                if (!Input.GetKeyDown(KeyCode.Insert)) { Input.ResetInputAxes(); }
                else { SetCursorPos(Screen.width / 2, Screen.height / 2); }
            }
            
        }
    }
}
