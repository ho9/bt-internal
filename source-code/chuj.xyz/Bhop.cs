using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace chuj.xyz
{
    public class Bhop : MonoBehaviour
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public Vars vars;
        public UI ui;

        private bool isJumping = false;

        private void Start()
        {
            vars = FindObjectOfType<Vars>();
            ui = FindObjectOfType<UI>();
        }

        private void Update()
        {
            if (ui != null && vars != null && vars.localPlayer != null && vars.localPlayer.isFirstPersonView && ui.bhopEnabled && Input.GetKey(KeyCode.Space) && Application.isFocused)
            {
                if (vars.localPlayer.IsOnGround() && !vars.localPlayer.IsDead())
                {
                    isJumping = true;
                    StartCoroutine(PerformJump());
                }
            }
            else
            {
                isJumping = false;
            }
        }

        private System.Collections.IEnumerator PerformJump()
        {
            while (isJumping)
            {
                keybd_event(0x20, 0x45, 0x1 | 0x2, UIntPtr.Zero);
                yield return new WaitForSeconds(0.001f);
                keybd_event(0x20, 0x45, 0x1, UIntPtr.Zero);
                yield return new WaitForSeconds(0.001f);
            }
        }
    }
}

