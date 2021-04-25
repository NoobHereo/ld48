using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace game.Objects
{
    public class Player : MonoBehaviour
    {

        //============= COMPONENTS =============//
        private Rigidbody2D rb;
        private Camera cam;
        private PlayerAnimator animator;
        private BoxCollider2D _collider;
        public virtual ProjectileParameters ProjectileParameters { get; protected set; }

        //============= PROPERTIES =============//
        public WorldController WorldController;
        public Sprite ProjTexture;
        public PlayerSpriteState LastDir;
        public bool Attacking = false;
        private float lastShoot;
        public float Cooldown = 1f; // Seconds
        public int CurrentLevel = 0;
        public bool OnTrapDoor = false;
        private bool gamePaused = false;

        //============= UI =============//
        public GameObject HurtOverlay;
        public TextMeshProUGUI TrapDoorText;
        public TrapDoor LastTrapDoor;
        public Slider HPSlider;
        public TextMeshProUGUI PauseText;

        //============= STATS =============//
        public int HP = 100;
        public float Speed = 100f;
        public int DMG = 100;

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
            _collider = GetComponent<BoxCollider2D>();

            animator.player = this;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            cam = UnityEngine.Camera.main.GetComponent<Camera>();
            cam.SetTarget(transform);
            _collider.size = new Vector2(0.5f, 0.5f);
            _collider.isTrigger = false;

            HPSlider.minValue = 0;
            HPSlider.maxValue = HP;
            HPSlider.value = HP;

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

            if (HP <= 0)
                OnDeath();

            if (Input.GetKeyDown(KeyCode.E) && OnTrapDoor && !gamePaused)
            {
                TrapDoorText.gameObject.SetActive(false);
                LastTrapDoor.Lock();
                loadWOrld(CurrentLevel + 1);
                WorldController.StartWave();
            }

            if (Input.GetKeyDown(KeyCode.P))
                PauseGame();

            if (Input.GetAxisRaw("Fire1") > 0.1f && !gamePaused)
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

            if (horizontalAbs < 0.1f && verticalAbs < 0.1f && !gamePaused)
            {
                rb.velocity = Vector2.zero;
                animator.UpdateSprite(LastDir);
            }

            if (horizontalAbs > verticalAbs && !gamePaused)
            {
                animator.UpdateSprite(horizontal > 0 ? PlayerSpriteState.Right : PlayerSpriteState.Left);
                LastDir = horizontal > 0 ? PlayerSpriteState.Right : PlayerSpriteState.Left;
            }

            rb.velocity = new Vector2(horizontal * Speed * Time.fixedDeltaTime, vertical * Speed * Time.fixedDeltaTime);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Trigger enter!");
            if (collision.tag == "Enemy" && Attacking)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                
            }           
        }

        public void TakeDamage(int dmg)
        {
            if (HP <= 0)
                Debug.Log("Player ded..");
            
            HP -= dmg;
            HPSlider.value = HP;
            bool dying = HP <= 50 ? true : false;
            dispatchHurtOverlay(dying);
        }

        private void dispatchHurtOverlay(bool dying)
        {
            HurtOverlay.SetActive(true);
            if (!dying)
                HurtOverlay.GetComponent<Animation>().Play();
        }

        public void DispatchTrapdoorUI(bool visible, TrapDoor trapdoor)
        {
            LastTrapDoor = trapdoor;
            TrapDoorText.text = "PRESS [E] TO ENTER";
            TrapDoorText.gameObject.SetActive(visible);
        }

        public void OnDeath()
        {
            SceneManager.LoadScene(2);
        }

        public void PauseGame()
        {
            if (gamePaused)
            {
                Time.timeScale = 1;
                PauseText.gameObject.SetActive(false);
                gamePaused = false;
            }
            else
            {
                Time.timeScale = 0;
                PauseText.gameObject.SetActive(true);
                gamePaused = true;
            }
        }
    }
}