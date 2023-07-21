using UnityEngine;

namespace chuj.xyz
{
    public class Loader
    {
        private static GameObject gameObject;

        public static void Init()
        {
            gameObject = new GameObject();
            
            gameObject.AddComponent<Vars>();
            gameObject.AddComponent<UI>();
            gameObject.AddComponent<Chams>();
            gameObject.AddComponent<Esp>();
            gameObject.AddComponent<Fov>();
            gameObject.AddComponent<Aimbot>();
            gameObject.AddComponent<Crosshair>();
            gameObject.AddComponent<Bhop>();


            Object.DontDestroyOnLoad(Loader.gameObject);
        }
        
        public static void Unload()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
