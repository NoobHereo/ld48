using UnityEngine;

namespace game.Objects
{
    public class Enemy : MonoBehaviour
    {
        private Transform target;
        private EnemyAnimator animator;

        public float Speed = 150f;
        public int Health = 100;
        public bool PredictiveMovement = false;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            animator = GetComponent<EnemyAnimator>();
        }

        private void Update()
        {
            if (Health <= 0)
                Destroy(gameObject);

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
        }

    }
}