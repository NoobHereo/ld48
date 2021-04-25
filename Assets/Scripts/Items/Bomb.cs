using UnityEngine;
using System.Collections;

namespace game.Items
{
    public class Bomb : MonoBehaviour
    {
        public float Countdown = 2f;
        public float ImpactRadius;
        public LayerMask TargetLayer;
        public Pickup Item;
        private SpriteRenderer renderer;
        public float Force;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = Item.Texture;
            StartCoroutine(ExplosionCountdown());
        }

        private IEnumerator ExplosionCountdown()
        {
            yield return new WaitForSeconds(Countdown);
            Explode();
        }

        public void Explode()
        {
            Debug.Log("Boom!");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.localPosition, ImpactRadius, TargetLayer);

            foreach (Collider2D obj in colliders)
            {
                Vector2 blastDir = obj.transform.position - transform.position;
                // Debug.Log("Blasting away entity: " + obj.gameObject.name);
                obj.GetComponent<Rigidbody2D>().AddForce(blastDir * Force, ForceMode2D.Force);                
            }

            Destroy(gameObject);
        }

    }
}