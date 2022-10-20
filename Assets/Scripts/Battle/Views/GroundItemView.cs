using UnityEngine;


namespace Services
{
    internal sealed class GroundItemView : MonoBehaviour, IView
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Transform Transform => _transform;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        /// <summary>
        /// Moves item from current position
        /// </summary>
        public void RelocateBy(Vector3 deltaPosition)
        {
            _transform.position += deltaPosition;
        }
    }
}