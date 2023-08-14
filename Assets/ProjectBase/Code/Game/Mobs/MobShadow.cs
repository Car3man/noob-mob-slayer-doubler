using UnityEngine;

namespace Game.Mobs
{
    public class MobShadow : MonoBehaviour
    {
        private Quaternion _initialRotation;
        private Vector3 _initialScale;

        private void Awake()
        {
            _initialRotation = transform.rotation;
            _initialScale = transform.localScale;
        }

        private void Update()
        {
            transform.rotation = _initialRotation;
            transform.localScale = _initialScale;
        }
    }
}
