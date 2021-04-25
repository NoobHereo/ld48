using UnityEngine;
using game.Objects;

namespace game
{
    public enum PlayerSpriteState
    {
        IdleRight,
        IdleLeft,    
        Right,
        Left,
        Death
    }

    public class PlayerAnimator : MonoBehaviour
    {
        private SpriteRenderer renderer;
        public Player player;

        private PlayerSpriteState state = PlayerSpriteState.IdleRight;

        public Sprite[] MoveRight;
        public Sprite[] DeathAnim;

        private int _rightLength;
        private int _deathLength;

        private int animationCount;
        private float timer = 0;
        public float AnimationSpeed = 0.5f;


        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            _rightLength = MoveRight.Length;
            _deathLength = DeathAnim.Length;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer < AnimationSpeed)
                return;
            timer = 0;
            Animate();
            animationCount++;
        }

        public void UpdateSprite(PlayerSpriteState spriteState)
        {
            // Debug.Log("Setting sprite state: " + spriteState);
            if (spriteState == state)
                return;
            animationCount = 0;
            state = spriteState;
            Animate();
        }

        private void Animate()
        {
            switch(state)
            {

                case PlayerSpriteState.Death:
                    bool animDone = false;
                    AnimationSpeed = 0.35f;
                    if (animationCount >= _deathLength)
                    {
                        renderer.sprite = DeathAnim[1];
                        animDone = true;
                    }

                    if(!animDone)
                        renderer.sprite = DeathAnim[animationCount];

                    renderer.flipX = false;
                    break;

                case PlayerSpriteState.IdleRight:
                    renderer.sprite = MoveRight[0];
                    renderer.flipX = false;
                    break;
                case PlayerSpriteState.IdleLeft:
                    renderer.sprite = MoveRight[0];
                    renderer.flipX = true;
                    break;


                case PlayerSpriteState.Right:
                    if (animationCount >= _rightLength)
                        animationCount = 0;
                    renderer.sprite = MoveRight[animationCount];
                    renderer.flipX = false;
                    break;
                case PlayerSpriteState.Left:
                    if (animationCount >= _rightLength)
                        animationCount = 0;
                    renderer.sprite = MoveRight[animationCount];
                    renderer.flipX = true;
                    break;             
            }
        }
    }
}