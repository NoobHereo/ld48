using UnityEngine;
using game.Objects;

namespace game
{
    public enum EnemySpriteState
    {
        Moving,
        Idle
    }

    public class EnemyAnimator : MonoBehaviour
    {
        private SpriteRenderer renderer;
        private EnemySpriteState state = EnemySpriteState.Idle;
        public Sprite[] MoveAnim;
        private int _moveLength;
        private int animationCount;
        private float timer = 0;
        public float AnimationSpeed = 0.5f;


        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            _moveLength = MoveAnim.Length;
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

        public void UpdateSprite(EnemySpriteState spriteState)
        {            
            if (spriteState == state)
                return;
            animationCount = 0;
            state = spriteState;
            Animate();
        }

        private void Animate()
        {
            switch (state)
            {

                case EnemySpriteState.Idle:
                    renderer.sprite = MoveAnim[0];
                    renderer.flipX = false;
                    break;

                case EnemySpriteState.Moving:
                    if (animationCount >= _moveLength)
                        animationCount = 0;

                    renderer.sprite = MoveAnim[animationCount];
                    renderer.flipX = false;
                    break;
            }
        }
    }
}