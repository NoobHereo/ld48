using UnityEngine;

namespace game.Objects
{
    [SerializeField]
    public class ProjectileParameters
    {
        public Sprite Texture;
        public Vector2 Position;
        public Quaternion Rotatoin = Quaternion.identity;
        public float Speed = 25f;
        public int Damage = 10;

        public float OrbitSpeed = 5f;
        public float OrbitLength = 10f;
        public float OrbitOffset = 0;

        public ProjectileParameters Clone()
        {
            return new ProjectileParameters()
            {
                Texture = Texture,
                Position = Position,
                Rotatoin = Rotatoin,
                Speed = Speed,
                Damage = Damage,
                OrbitSpeed = OrbitSpeed,
                OrbitLength = OrbitLength,
                OrbitOffset = OrbitOffset
            };
        }
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour
    {
        public ProjectileParameters Parameters;
        public static int SortingLayer = UnityEngine.SortingLayer.NameToID("Objects");
        private float timer = 0;
        
        public static GameObject InstantiateProjectile(ProjectileParameters parameters, Quaternion rotation)
        {
            var projParent = new GameObject("PProjFirePoint");
            projParent.transform.localPosition = parameters.Position;
            projParent.transform.rotation = parameters.Rotatoin;

            var projGO = new GameObject("PlayerProj", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D));

            var renderer = projGO.GetComponent<SpriteRenderer>();
            renderer.sprite = parameters.Texture;
            renderer.sortingLayerID = SortingLayer;

            var collider = projGO.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(0.5f, 0.5f);
            collider.isTrigger = true;

            var rb = projGO.GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.rotation = 0;
            rb.gravityScale = 0;

            var projectile = projGO.AddComponent<Projectile>();
            var paraClone = parameters.Clone();

            projectile.Parameters = paraClone;
            projGO.transform.rotation = rotation;
            projGO.transform.localPosition = projParent.transform.position;
            projGO.transform.SetParent(projParent.transform);
            Destroy(projParent, 3f);
            return projParent;
        }

        private void Update()
        {
            timer += Time.deltaTime * Parameters.OrbitSpeed;
            transform.localPosition = new Vector2(transform.localPosition.x + (Parameters.Speed * Time.deltaTime), Mathf.Sin(timer + Parameters.OrbitOffset) * Parameters.OrbitLength);
        }
    }
}