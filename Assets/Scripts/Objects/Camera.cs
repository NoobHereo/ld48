using UnityEngine;

namespace game.Objects
{
    public class Camera : MonoBehaviour
    {
        private Transform target;
        private bool targetSet = false;
        public Vector3 Offset;

        public void SetTarget(Transform target)
        {
            if (target != null)
            {
                this.target = target;
                targetSet = true;
            }
        }

        private void LateUpdate()
        {
            if (targetSet)
            {
                transform.position = target.localPosition - Offset;
            }
        }
    }
}