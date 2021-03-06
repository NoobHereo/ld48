using UnityEngine;
using game.Items;
using UnityEngine.UI;

namespace game.Objects
{
    public class Enemy : MonoBehaviour
    {
        private Transform target;
        private EnemyAnimator animator;
        public WorldController WorldController;
        private Rigidbody2D rb;
        public Slider HPSlider;
        public GameObject[] Items;
        private SoundManager soundManager;
        public AudioClip HitSFX;
        public AudioClip DeathSFX;

        public float Speed = 150f;
        public int Health = 100;
        public bool PredictiveMovement = false;
        public int DMG = 10;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            soundManager = GameObject.FindGameObjectWithTag("SoundManager").gameObject.GetComponent<SoundManager>();
            animator = GetComponent<EnemyAnimator>();
            rb = GetComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            HPSlider.minValue = 0;
            HPSlider.maxValue = Health;
            HPSlider.value = Health;
        }

        private void Update()
        {
            if (OutOfBounds())
                Die(false);

            if (Health <= 0 && !OutOfBounds())
                Die(true);

            var tRB = target.gameObject.GetComponent<Rigidbody2D>();
            Player player = target.GetComponent<Player>();

            Vector3 pos = tRB.position;
            Vector3 vel = tRB.velocity;

            float dist = (pos - transform.position).magnitude;

            if (Vector2.Distance(transform.localPosition, target.localPosition) < 10 && !player.IsDead)
            {
                animator.UpdateSprite(EnemySpriteState.Moving);

                if (PredictiveMovement)
                    gameObject.transform.position = Vector2.MoveTowards(transform.position, pos + (dist / Speed) * vel, Speed * Time.deltaTime);
                else
                    gameObject.transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
            }
            else if (player.IsDead)
            {
                Destroy(gameObject, 1f);
            }
            else
            {
                animator.UpdateSprite(EnemySpriteState.Idle);
            }
        }

        public void TakeDamage(int damage)
        {
            soundManager.PlaySFX(HitSFX);
            Health -= damage;
            HPSlider.value = Health;
            if (Health <= 0 && !OutOfBounds())
                Die(true);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();                
                player.TakeDamage(DMG);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        public void Die(bool loot)
        {
            WorldController.OnWorldEntityDeath();
            soundManager.PlaySFX(DeathSFX);
            if (loot)
            {
                int randLoot = Random.Range(0, Items.Length);
                var lootGo = Instantiate(Items[randLoot]);
                lootGo.transform.position = transform.position;
            }
            Destroy(gameObject);
        }

        public bool OutOfBounds()
        {
            if (transform.position.x < 5.6f || transform.position.x > 24f || transform.position.y < -23.6 || transform.position.y > -6)
                return true;
            else
                return false;
        }

    }
}