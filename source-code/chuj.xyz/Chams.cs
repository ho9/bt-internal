using UnityEngine;
using Assets.Sources.Components.Player.UnityObjects;

namespace chuj.xyz
{
    public class Chams : MonoBehaviour
    {
        public Vars vars;
        public UI ui;

        private Material material;

        private Material glowPlayerMaterial;
        private Material glowHandsMaterial;
        private Material glowWeaponMaterial;

        private Texture2D playerTexture;
        private Texture2D handsTexture;
        private Texture2D weaponTexture;

        private void Start()
        {
            vars = FindObjectOfType<Vars>();
            ui = FindObjectOfType<UI>();

            material = new Material(Shader.Find("Standard"));
            glowPlayerMaterial = new Material(Shader.Find("SSJJ/XRay"));
            glowHandsMaterial = new Material(Shader.Find("SSJJ/XRay"));
            glowWeaponMaterial = new Material(Shader.Find("SSJJ/XRay"));

            glowPlayerMaterial.SetColor("_RimColor", Color.HSVToRGB(ui.chamsRGB, 1f, 1f));
            glowPlayerMaterial.SetFloat("_RimIntensity", 0.9f);
            glowHandsMaterial.SetColor("_RimColor", Color.HSVToRGB(ui.handsRGB, 1f, 1f));
            glowHandsMaterial.SetFloat("_RimIntensity", 0.9f);
            glowWeaponMaterial.SetColor("_RimColor", Color.HSVToRGB(ui.weaponRGB, 1f, 1f));
            glowWeaponMaterial.SetFloat("_RimIntensity", 0.9f);

            playerTexture = vars.CreateTexture(Color.HSVToRGB(ui.chamsRGB, 1f, 1f));
            handsTexture = vars.CreateTexture(Color.HSVToRGB(ui.handsRGB, 1f, 1f));
            weaponTexture = vars.CreateTexture(Color.HSVToRGB(ui.weaponRGB, 1f, 1f));
        }

        private void Update()
        {
            if (vars == null || ui == null)
                return;

            if (ui.chamsFlat)
                material.shader = Shader.Find("Standard");
            else if (ui.chamsAdditive)
                material.shader = Shader.Find("SSJJ/UvHuxi");

            if (ui.chamsGlow)
            {
                glowPlayerMaterial.SetColor("_RimColor", Color.HSVToRGB(ui.chamsRGB, 1f, 1f));
                glowHandsMaterial.SetColor("_RimColor", Color.HSVToRGB(ui.handsRGB, 1f, 1f));
                glowWeaponMaterial.SetColor("_RimColor", Color.HSVToRGB(ui.weaponRGB, 1f, 1f));
            }

            playerTexture.SetPixel(0, 0, Color.HSVToRGB(ui.chamsRGB, 1f, 1f));
            handsTexture.SetPixel(0, 0, Color.HSVToRGB(ui.handsRGB, 1f, 1f));
            weaponTexture.SetPixel(0, 0, Color.HSVToRGB(ui.weaponRGB, 1f, 1f));
            playerTexture.Apply();
            handsTexture.Apply();
            weaponTexture.Apply();


            if (!Application.isFocused || Camera.main == null || vars == null || vars.playerContext == null || ui == null || Event.current == null)
                return;

            foreach (PlayerEntity playerEntity in vars.playerContext)
            {
                if (playerEntity == null)
                    continue;

                SkinnedMeshRenderer playerRenderer = playerEntity.GetSkinnedMeshRender();
                SkinnedMeshRenderer[] playerWeaponRenderer = playerEntity.GetWeaponSkinnedMeshRenderer();

                // Player
                if (ui.chamsEnabled && playerEntity != null && !playerEntity.isMyPlayer && playerRenderer != null && vars.localPlayerTeam != playerEntity.GetTeam())
                {
                    if (ui.chamsGlow && playerRenderer.material != glowPlayerMaterial)
                        playerRenderer.material = glowPlayerMaterial;

                    else if (playerRenderer.material != material || playerRenderer.material.mainTexture != playerTexture)
                        playerRenderer.material = material;
                        playerRenderer.material.mainTexture = playerTexture;

                }

                // Player weapons
                if (ui.chamsEnabled && playerEntity != null && !playerEntity.isMyPlayer && playerWeaponRenderer != null && vars.localPlayerTeam != playerEntity.GetTeam())
                {
                    foreach (SkinnedMeshRenderer weaponRenderer in playerWeaponRenderer)
                    {
                        if (weaponRenderer != null)
                        {
                            if (ui.chamsGlow && playerRenderer.material != glowPlayerMaterial)
                                weaponRenderer.material = glowPlayerMaterial;

                            else if (playerRenderer.material != material || playerRenderer.material.mainTexture != playerTexture)
                                weaponRenderer.material = material;
                                weaponRenderer.material.mainTexture = playerTexture;

                        }
                    }
                }

                if (playerEntity != null && playerEntity.isMyPlayer && playerEntity.hasFirstPersonUnityObjects)
                {
                    FirstPersonUnityObjectsComponent firstPersonUnityObjects = playerEntity.firstPersonUnityObjects;

                    // Localplayer hands
                    if (firstPersonUnityObjects != null)
                    {
                        foreach (SkinnedMeshRenderer handsRenderer in firstPersonUnityObjects.HandSkinnedMeshRenderers)
                        {
                            if (ui.handChamsEnabled && handsRenderer != null)
                            {
                                if (ui.chamsGlow && handsRenderer.material != glowHandsMaterial)
                                    handsRenderer.material = glowHandsMaterial;

                                else if (handsRenderer.material != material || handsRenderer.material.mainTexture != handsTexture)
                                    handsRenderer.material = material;
                                    handsRenderer.material.mainTexture = handsTexture;

                            }
                        }

                        // Localplayer weapons
                        foreach (SkinnedMeshRenderer weaponRenderer in firstPersonUnityObjects.WeaponSkinnedMeshRenderers)
                        {
                            if (ui.weaponChamsEnabled && weaponRenderer != null)
                            {
                                if (ui.chamsGlow && weaponRenderer.material != glowWeaponMaterial)
                                    weaponRenderer.material = glowWeaponMaterial;

                                else if (weaponRenderer.material != material || weaponRenderer.material.mainTexture != weaponTexture)
                                    weaponRenderer.material = material;
                                    weaponRenderer.material.mainTexture = weaponTexture;

                            }
                        }
                    }
                }
            }
        }
    }
}
