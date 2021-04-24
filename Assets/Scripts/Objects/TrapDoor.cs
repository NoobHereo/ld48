using UnityEngine;

namespace game.Objects
{
    public class TrapDoor : MonoBehaviour
    {
        private SpriteRenderer renderer;
        public Sprite Locked;
        public Sprite Unlocked;
        private bool unlocked = false;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        public void Unlock()
        {
            renderer.sprite = Unlocked;
            unlocked = true;
        }

        public void Lock()
        {
            renderer.sprite = Locked;
            unlocked = false;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && unlocked)
            {
                Player player = collision.GetComponent<Player>();
                player.OnTrapDoor = true;
                player.DispatchTrapdoorUI(true, this);
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player" && unlocked)
            {
                Player player = collision.GetComponent<Player>();
                player.OnTrapDoor = false;
                player.DispatchTrapdoorUI(false, this);
            }
        }
    }
}