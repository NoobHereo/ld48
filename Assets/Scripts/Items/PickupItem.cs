using UnityEngine;

namespace game.Items
{
    public class PickupItem : MonoBehaviour
    {
        private SpriteRenderer renderer;
        public Pickup Item;
        private SoundManager soundManager;
        public AudioClip SFX;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            soundManager = GameObject.FindGameObjectWithTag("SoundManager").gameObject.GetComponent<SoundManager>();

            if (Item != null)
                renderer.sprite = Item.Texture;

            Destroy(gameObject, 10f);
        }

        public void Remove()
        {
            soundManager.PlaySFX(SFX);
            Destroy(gameObject);
        }
    }
}