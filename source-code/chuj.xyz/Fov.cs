using UnityEngine;

namespace chuj.xyz
{
    public class Fov : MonoBehaviour
    {
        public UI ui;
        public Vars vars;

        private void Start()
        {
            ui = FindObjectOfType<UI>();
            vars = FindObjectOfType<Vars>();
        }

        private void Update() {

            if (ui != null && ui.viewmodelFovEnabled && vars.localPlayer.firstPersonUnityObjects.HandGameObject.transform.localPosition != null)
            {
                vars.localPlayer.firstPersonUnityObjects.HandGameObject.transform.localPosition = new Vector3(ui.viewmodelFovX, ui.viewmodelFovY, ui.viewmodelFovZ);
            }
        }
    }
}