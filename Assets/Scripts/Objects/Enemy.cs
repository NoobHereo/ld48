using UnityEngine;

namespace game.Objects
{
    public class Enemy : MonoBehaviour
    {
        private Transform target;
        public float Speed = 150f;
        public int Health = 100;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (Health <= 0)
                Destroy(gameObject);

            gameObject.transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

    }
}