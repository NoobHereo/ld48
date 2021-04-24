using UnityEngine;

namespace game.Objects
{
    public class TrapDoor : MonoBehaviour
    {
        private SpriteRenderer renderer;
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

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && unlocked)
            {
                Debug.Log("Trapdoor is ready to be entered.");
            }
        }

    }
}