using RuntimeInspectorNamespace;
using UnityEngine;

namespace chuj.xyz
{
    public class Fov : MonoBehaviour
    {
        Camera viewmodelFov;
        public UI ui;

        private void Start()
        {
            ui = FindObjectOfType<UI>();
        }

        private void Update() {
            try
            {
                viewmodelFov = GameObject.Find("HandCamera").GetComponent<Camera>();
            }
            catch
            {
                viewmodelFov = null;
            }

            if (viewmodelFov != null && ui.viewmodelFovEnabled && !viewmodelFov.IsNull() && viewmodelFov.isActiveAndEnabled && viewmodelFov.fieldOfView != ui.viewmodelFovValue)
            {
                viewmodelFov.fieldOfView = ui.viewmodelFovValue;
            }
        }
    }
}