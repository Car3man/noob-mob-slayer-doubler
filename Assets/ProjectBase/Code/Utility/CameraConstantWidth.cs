using UnityEngine;

namespace Utility
{
    [ExecuteAlways]
    public class CameraConstantWidth : MonoBehaviour
    {
        [SerializeField] private float initialOrthographicSize = 3f;
        [SerializeField] private float initialFov = 50f;
        [SerializeField] private Vector2 defaultResolution = new(192, 1080);
        [SerializeField] [Range(0f, 1f)] public float widthOrHeight;

        private Camera _camera;
        private Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = GetComponent<Camera>();
                }
                return _camera;
            }
        }

        private void Update()
        {
            if (Camera.orthographic)
            {
                float constantWidthSize = initialOrthographicSize * (GetTargetAspect() / Camera.aspect);
                Camera.orthographicSize = Mathf.Lerp(constantWidthSize, initialOrthographicSize, widthOrHeight);
            }
            else
            {
                float constantWidthFov = CalculateVerticalFov(GetHorizontalFov(), Camera.aspect);
                Camera.fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, widthOrHeight);
            }
        }

        private float CalculateVerticalFov(float horizontalFovInDegree, float aspectRatio)
        {
            float horizontalFovInRadians = horizontalFovInDegree * Mathf.Deg2Rad;
            float verticalFovInRadians = 2 * Mathf.Atan(Mathf.Tan(horizontalFovInRadians / 2) / aspectRatio);
            return verticalFovInRadians * Mathf.Rad2Deg;
        }

        private float GetTargetAspect()
        {
            return defaultResolution.x / defaultResolution.y;
        }
        
        private float GetHorizontalFov()
        {
            return CalculateVerticalFov(initialFov, 1 / GetTargetAspect());
        }
    }
}