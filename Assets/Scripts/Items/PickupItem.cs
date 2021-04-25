using UnityEngine;

namespace game.Items
{
    public class PickupItem : MonoBehaviour
    {
        private SpriteRenderer renderer;
        public Pickup Item;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            if (Item != null)
                renderer.sprite = Item.Texture;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}