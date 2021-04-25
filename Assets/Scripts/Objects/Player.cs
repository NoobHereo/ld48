using TMPro;
using game.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace game.Objects
{
    public enum PlayerStats
    {
        HP,
        DMG,
        HP_REGEN,
        BOMB,
        SPD,
        DPS
    }

    public class Player : MonoBehaviour
    {

        //============= COMPONENTS =============//
        private Rigidbody2D rb;
        private Camera cam;
        private PlayerAnimator animator;
        private BoxCollider2D _collider;
        public virtual ProjectileParameters ProjectileParameters { get; protected set; }
        public GameObject BombPrefab;
        public GameObject TeleAimPrefab;
        public SoundManager SoundManager;
        public GameObject CustomizationWindow;

        //============= AUDIO =============//
        public AudioClip PlayerHitSFX;
        public AudioClip PlayerDeathSFX;
        public AudioClip PauseSFX;
        public AudioClip UnpauseSFX;

        //============= PROPERTIES =============//
        public WorldController WorldController;
        public Sprite ProjTexture;
        public PlayerSpriteState LastDir;
        public bool Attacking = false;
        private float lastShoot;      
        public int CurrentLevel = 0;
        public bool OnTrapDoor = false;
        private bool gamePaused = false;
        public int Bombs = 1;
        public bool IsAdmin;
        public bool IsDead = false; // pepsi
        public bool Teleporting = false;
        private bool deathTrigger = false;
        private bool customizationOpen = false;
        private float abilityCharge = 10;
        public ProjectileCfg CurrentProj;

        //============= UI =============//
        public GameObject HurtOverlay;
        public TextMeshProUGUI TrapDoorText;
        public TrapDoor LastTrapDoor;
        public Slider HPSlider;
        public TextMeshProUGUI PauseText;

        public TextMeshProUGUI BombCountText;
        public TextMeshProUGUI SPDText;
        public TextMeshProUGUI DMGText;
        public TextMeshProUGUI DPSText;

        public Slider AbilityRechargeBar;

        //============= STATS =============//
        public int HP = 100;
        public int MaxHP = 100;
        public float Speed = 100f;
        public int DMG;
        public float DPS; // Seconds

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

            HP = MaxHP;
            HPSlider.minValue = 0;
            HPSlider.maxValue = HP;
            HPSlider.value = HP;

            BombCountText.text = Bombs.ToString();
            DPSText.text = DPS.ToString();
            DMGText.text = DMG.ToString();
            SPDText.text = Speed.ToString();

            AbilityRechargeBar.minValue = 0;
            AbilityRechargeBar.maxValue = 10;
            AbilityRechargeBar.value = 10;

            DPS = CurrentProj.DPS;
            DMG = CurrentProj.DMG;

            loadWOrld(CurrentLevel);
        }

        private void loadWOrld(int id)
        {
            OnTrapDoor = false;
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

            if (abilityCharge < 10)
            {
                abilityCharge += Time.deltaTime;
                AbilityRechargeBar.value = abilityCharge;
            }

            if (HP <= 0 && !deathTrigger)
            {
                deathTrigger = true;
                IsDead = true;
                rb.velocity = Vector2.zero;
                animator.UpdateSprite(PlayerSpriteState.Death);
                OnDeath();
            }
            else if (!deathTrigger && !IsDead)
            {
                if (Input.GetKeyDown(KeyCode.E) && OnTrapDoor && !gamePaused)
                {
                    TrapDoorText.gameObject.SetActive(false);
                    LastTrapDoor.Lock();
                    loadWOrld(CurrentLevel + 1);
                    WorldController.StartWave();
                }

                if (Input.GetKeyDown(KeyCode.C) && !gamePaused)
                {
                    if (customizationOpen)
                    {
                        customizationOpen = false;
                        CustomizationWindow.SetActive(customizationOpen);
                    }
                    else
                    {
                        customizationOpen = true;
                        CustomizationWindow.SetActive(customizationOpen);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q) && Bombs > 0)
                {
                    var bombGO = Instantiate(BombPrefab);
                    bombGO.transform.position = transform.localPosition;
                    IncreaseStat(PlayerStats.BOMB, -1);
                }

                if (Input.GetKeyDown(KeyCode.Space) && abilityCharge >= 10f)
                {
                    Teleporting = true;
                    abilityCharge = 0;
                    AbilityRechargeBar.value = abilityCharge;
                    var teleAimGO = Instantiate(TeleAimPrefab);
                    teleAimGO.transform.position = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    teleAimGO.GetComponent<Teleport>().player = this;
                }

                if (Input.GetKeyDown(KeyCode.P))
                    PauseGame();

                if (Input.GetAxisRaw("Fire1") > 0.1f && !gamePaused && !Teleporting)
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

                    if (Time.time - lastShoot > DPS)
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

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Pickup")
            {
                PickupItem item = collision.GetComponent<PickupItem>();
                PickupItem(item);
                item.Remove();
            }           
        }

        public void TakeDamage(int dmg)
        {
            if (HP <= 0)
            {
                IsDead = true;
                OnDeath();               
            }

            SoundManager.PlaySFX(PlayerHitSFX);
            HP -= dmg;
            HPSlider.value = HP;
            bool dying = HP <= 50 ? true : false;
            dispatchHurtOverlay(dying);
        }

        private void dispatchHurtOverlay(bool dying)
        {
            SoundManager.PlaySFX(PlayerHitSFX);
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
            SoundManager.PlaySFX(PlayerDeathSFX);
            StartCoroutine(LoadDeathScreen());
        }

        private IEnumerator LoadDeathScreen()
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(2);
        }

        public void PauseGame()
        {
            if (gamePaused)
            {
                Time.timeScale = 1;
                PauseText.gameObject.SetActive(false);
                gamePaused = false;
                SoundManager.PlaySFX(UnpauseSFX);
                SoundManager.ResumeMusic();
            }
            else
            {
                Time.timeScale = 0;
                PauseText.gameObject.SetActive(true);
                gamePaused = true;
                SoundManager.PlaySFX(PauseSFX);
                SoundManager.PauseMusic();
            }
        }

        private void PickupItem(PickupItem pickup)
        {
            switch(pickup.Item.ItemStat)
            {
                case PlayerStats.HP:
                case PlayerStats.HP_REGEN:
                    IncreaseStat(PlayerStats.HP, 25f);
                    break;

                case PlayerStats.DMG:
                    IncreaseStat(PlayerStats.DMG, 1f);
                    break;

                case PlayerStats.DPS:
                    IncreaseStat(PlayerStats.DPS, 0.002f);
                    break;

                case PlayerStats.SPD:
                    IncreaseStat(PlayerStats.SPD, 5f);
                    break;

                case PlayerStats.BOMB:
                    IncreaseStat(PlayerStats.BOMB, 1f);
                    break;
            }
        }

        public void IncreaseStat(PlayerStats stat, float gain)
        {
            switch (stat)
            {
                case PlayerStats.HP:
                case PlayerStats.HP_REGEN:
                    if (HP >= MaxHP)
                        break;
                    else if (HP + gain > MaxHP)
                    {
                        HP = MaxHP;
                        HPSlider.value = HP;
                    }
                    else
                    {
                        HP += (int)gain;
                        HPSlider.value = HP;
                    }
                    break;

                case PlayerStats.DMG:
                    DMG += (int)gain;
                    DMGText.text = DMG.ToString();
                    break;

                case PlayerStats.DPS:
                    DPS -= gain;
                    DPSText.text = DPS.ToString();
                    break;

                case PlayerStats.SPD:
                    Speed += gain;
                    SPDText.text = Speed.ToString();
                    break;

                case PlayerStats.BOMB:
                    Bombs += (int)gain;
                    BombCountText.text = Bombs.ToString();
                    break;
            }

            StatDataManager.Singleton.ItemPicked();
        }
    }
}