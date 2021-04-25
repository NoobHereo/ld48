using UnityEngine;
using game.Objects;

namespace game.Items
{
    public class Teleport : MonoBehaviour
    {
        public Player player;
        private bool teleportDone = false;

        private void Update()
        {
            var mousePos = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
            if (Input.GetAxisRaw("Fire1") > 0.1f && !teleportDone)
            {
                DoTeleport();
            }
        }

        private void DoTeleport()
        {
            if (transform.position.x < 5.6f || transform.position.x > 24f || transform.position.y < -23.6 || transform.position.y > -6)
            {
                Debug.Log("Out of bounds: " + transform.position);
                return;
            }
            else
            {
                teleportDone = true;
                player.transform.position = new Vector2(transform.position.x, transform.position.y);
                player.Teleporting = false;
                Destroy(gameObject);
            }
        }
    }
}