using UnityEngine;
using UnityEngine.UI;

namespace NightKeepers
{
    public class FloatingHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        private UnityEngine.Camera mainCamera;

        private void Start()
        {
            mainCamera = UnityEngine.Camera.main;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }

        public void UpdateHealthBar(float maxValue, float currentValue)
        {
            _slider.value = currentValue / maxValue;
        }
    }
}