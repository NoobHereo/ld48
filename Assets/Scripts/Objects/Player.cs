using UnityEngine;

namespace game.Objects
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb;
        private game.Objects.Camera cam;
        public float Speed = 100f;
        private PlayerAnimator animator;

        private void Start()
        {
            configComponenets();
        }

        private void configComponenets()
        {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<PlayerAnimator>();
            rb.gravityScale = 0;
            cam = UnityEngine.Camera.main.GetComponent<Camera>();
            cam.SetTarget(transform);
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var horizontalAbs = Mathf.Abs(horizontal);
            var vertical = Input.GetAxisRaw("Vertical");
            var verticalAbs = Mathf.Abs(vertical);

            if (horizontalAbs < 0.1f && verticalAbs < 0.1f)
            {
                rb.velocity = Vector2.zero;
                animator.UpdateSprite(SpriteState.Idle);
            }

            if (horizontalAbs > verticalAbs)
            {
                animator.UpdateSprite(horizontal > 0 ? SpriteState.Right : SpriteState.Left);
            }
            else
            {
                animator.UpdateSprite(vertical > 0 ? SpriteState.Up : SpriteState.Down);
            }

            rb.velocity = new Vector2(horizontal * Speed * Time.fixedDeltaTime, vertical * Speed * Time.fixedDeltaTime);
        }
    }
}