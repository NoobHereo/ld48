using UnityEngine;

namespace game.Objects
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb;
        private game.Objects.Camera cam;
        public float Speed = 100f;

        private void Start()
        {
            configComponenets();
        }

        private void configComponenets()
        {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            cam = UnityEngine.Camera.main.GetComponent<Camera>();
            cam.SetTarget(transform);
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            rb.velocity = new Vector2(horizontal * Speed * Time.fixedDeltaTime, vertical * Speed * Time.fixedDeltaTime);
        }
    }
}