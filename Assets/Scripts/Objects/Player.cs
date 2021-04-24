using System.Collections;
using UnityEngine;

namespace game.Objects
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Camera cam;
        private PlayerAnimator animator;
        private BoxCollider2D collider;
        public Sprite ProjTexture;
        public WorldController WorldController;

        public bool Attacking = false;
        public float Speed = 100f;
        private float lastShoot;
        public float Cooldown = 1f; // Seconds
        public int CurrentLevel = 0;
        public int DMG = 100;

        public SpriteState LastDir;
        public virtual ProjectileParameters ProjectileParameters { get; protected set; }

        private void Start()
        {
            configComponenets();
        }

        private void configComponenets()
        {
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.AddComponent<Rigidbody2D>();

            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<PlayerAnimator>();
            collider = GetComponent<BoxCollider2D>();

            animator.player = this;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            cam = UnityEngine.Camera.main.GetComponent<Camera>();
            cam.SetTarget(transform);
            collider.size = new Vector2(0.5f, 0.5f);
            collider.isTrigger = false;
            loadWOrld(CurrentLevel);
        }

        private void loadWOrld(int id)
        {
            transform.position = new Vector2(15f, -15f);
            WorldController.LoadMap(id);
            CurrentLevel = id;
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var horizontalAbs = Mathf.Abs(horizontal);
            var vertical = Input.GetAxisRaw("Vertical");
            var verticalAbs = Mathf.Abs(vertical);

            if (Input.GetAxisRaw("Fire1") > 0.1f)
            {
                Vector2 dir = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Quaternion spriteRotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);

                ProjectileParameters = new ProjectileParameters()
                {
                    Texture = ProjTexture,
                    Speed = 15f,
                    Damage = DMG,
                    OrbitSpeed = 25f,
                    OrbitLength = 0.5f,
                    OrbitOffset = 0
            };

                var proj = ProjectileParameters.Clone();
                proj.Position = transform.position;
                proj.Rotatoin = rotation;
                
                if (Time.time - lastShoot > Cooldown)
                {
                    Projectile.InstantiateProjectile(proj, spriteRotation);
                    lastShoot = Time.time;
                }
            }

            if (horizontalAbs < 0.1f && verticalAbs < 0.1f)
            {
                rb.velocity = Vector2.zero;
                animator.UpdateSprite(LastDir);
            }

            if (horizontalAbs > verticalAbs)
            {
                animator.UpdateSprite(horizontal > 0 ? SpriteState.Right : SpriteState.Left);
                LastDir = horizontal > 0 ? SpriteState.Right : SpriteState.Left;
            }

            rb.velocity = new Vector2(horizontal * Speed * Time.fixedDeltaTime, vertical * Speed * Time.fixedDeltaTime);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Enemy" && Attacking)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.TakeDamage(DMG);
                var rb = enemy.GetComponent<Rigidbody2D>();
                Vector2 forceVec = new Vector2(5f, 5f);
                rb.AddForce(forceVec);
            }
        }
    }
}