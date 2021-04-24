using System.Collections;
using UnityEngine;

namespace game.Objects
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb;
        private game.Objects.Camera cam;
        public float Speed = 100f;
        private PlayerAnimator animator;
        private BoxCollider2D collider;
        public bool Attacking = false;
        public Sprite ProjTexture;

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
            cam = UnityEngine.Camera.main.GetComponent<Camera>();
            cam.SetTarget(transform);
            collider.size = new Vector2(0.5f, 0.5f);
            collider.isTrigger = false;
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
                    Damage = 100,
                    OrbitSpeed = 25f,
                    OrbitLength = 0.5f,
                    OrbitOffset = 0
            };

                var proj = ProjectileParameters.Clone();
                proj.Position = transform.position;
                proj.Rotatoin = rotation;
                Projectile.InstantiateProjectile(proj, spriteRotation);
            }

            if (horizontalAbs < 0.1f && verticalAbs < 0.1f)
            {
                rb.velocity = Vector2.zero;
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
            }
        }
    }
}