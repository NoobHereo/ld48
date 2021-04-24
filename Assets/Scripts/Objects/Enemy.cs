using UnityEngine;

namespace game.Objects
{
    public class Enemy : MonoBehaviour
    {
        private Transform target;
        private EnemyAnimator animator;
        public WorldController WorldController;

        public float Speed = 150f;
        public int Health = 100;
        public bool PredictiveMovement = false;
        public int DMG = 10;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            animator = GetComponent<EnemyAnimator>();
        }

        private void Update()
        {
            if (Health <= 0)
                Die();

            var tRB = target.gameObject.GetComponent<Rigidbody2D>();

            Vector3 pos = tRB.position;
            Vector3 vel = tRB.velocity;

            float dist = (pos - transform.position).magnitude;


            if (Vector2.Distance(transform.localPosition, target.localPosition) < 10)
            {
                animator.UpdateSprite(EnemySpriteState.Moving);

                if (PredictiveMovement)
                    gameObject.transform.position = Vector2.MoveTowards(transform.position, pos + (dist / Speed) * vel, Speed * Time.deltaTime);
                else
                    gameObject.transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
            }
            else
            {
                animator.UpdateSprite(EnemySpriteState.Idle);
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
                Die();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Player player = collision.GetComponent<Player>();
                player.TakeDamage(DMG);
            }
        }

        public void Die()
        {
            WorldController.OnWorldEntityDeath();           
            Destroy(gameObject);
        }

    }
}