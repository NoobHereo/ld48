using UnityEngine;

namespace game
{
    public enum SpriteState
    {
        Idle,
        Right,
        Left,
        Up,
        Down
    }

    public class PlayerAnimator : MonoBehaviour
    {
        private SpriteRenderer renderer;

        private SpriteState state = SpriteState.Down;

        public Sprite[] MoveRight;
        public Sprite[] MoveUp;
        public Sprite[] MoveDown;

        private int _rightLength;
        private int _upLength;
        private int _downLength;

        private int animationCount;
        private float timer = 0;
        public float AnimationSpeed = 0.5f;


        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            _rightLength = MoveRight.Length;
            _upLength = MoveUp.Length;
            _downLength = MoveDown.Length;            
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
                case SpriteState.Idle:
                    renderer.sprite = MoveDown[0];
                    renderer.flipX = false;
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

                case SpriteState.Up:
                    if (animationCount >= _upLength)
                        animationCount = 0;
                    renderer.sprite = MoveUp[animationCount];
                    renderer.flipX = false;
                    break;

                case SpriteState.Down:
                    if (animationCount >= _downLength)
                        animationCount = 0;
                    renderer.sprite = MoveDown[animationCount];
                    renderer.flipX = false;
                    break;
            }
        }

    }
}