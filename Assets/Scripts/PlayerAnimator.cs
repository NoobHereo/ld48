using UnityEngine;
using game.Objects;

namespace game
{
    public enum SpriteState
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

        private SpriteState state = SpriteState.Down;

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

        public void UpdateSprite(SpriteState spriteState)
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

                case SpriteState.IdleRight:
                    renderer.sprite = MoveRight[0];
                    renderer.flipX = false;
                    break;
                case SpriteState.IdleLeft:
                    renderer.sprite = MoveRight[0];
                    renderer.flipX = true;
                    break;


                case SpriteState.Right:
                    if (animationCount >= _rightLength)
                        animationCount = 0;
                    renderer.sprite = MoveRight[animationCount];
                    renderer.flipX = false;
                    break;
                case SpriteState.Left:
                    if (animationCount >= _rightLength)
                        animationCount = 0;
                    renderer.sprite = MoveRight[animationCount];
                    renderer.flipX = true;
                    break;             
            }
        }
    }
}