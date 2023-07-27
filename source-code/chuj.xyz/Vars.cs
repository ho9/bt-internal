using UnityEngine;

namespace chuj.xyz
{
    public class Vars : MonoBehaviour
    {
        public PlayerContext playerContext;
        public PlayerEntity localPlayer;
        public int localPlayerTeam;

        private void Update()
        {
            playerContext = Contexts.sharedInstance.player;

            foreach (PlayerEntity playerEntity in playerContext)
            {
                    if (playerEntity != null && playerEntity.isMyPlayer)
                    {
                    localPlayer = playerEntity;
                    localPlayerTeam = playerEntity.GetTeam();
                }
            }
        }
        public Texture2D CreateTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}