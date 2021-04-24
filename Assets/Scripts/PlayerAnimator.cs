using UnityEngine;
using game.Objects;

namespace game
{
    public enum PlayerSpriteState
    {
        IdleRight,
        IdleLeft,
        IdleUp,
        IdleDown,
        Right,
        Left,
        Up,
        Down
    }

    public class PlayerAnimator : MonoBehaviour
    {
        private SpriteRenderer renderer;
        public Player player;

        private PlayerSpriteState state = PlayerSpriteState.Down;

        public Sprite[] MoveRight;

        private int _rightLength;

        private int animationCount;
        private float timer = 0;
        public float AnimationSpeed = 0.5f;


        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            _rightLength = MoveRight.Length;
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