using UnityEngine;

namespace NightKeepers
{
    public class SpawnPositionIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform arrowTransform;
        public Transform ObjectToFollow;

        private Vector2 _screenBounds;

        private void Update()
        {
            if (!BuildingManager.Instance.isTownHallPlaced) return;

            Vector3 direction = ObjectToFollow.position - arrowTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float adjustedAngle = angle + 90;
            arrowTransform.rotation = Quaternion.Euler(0, 0, adjustedAngle);
        }

        private void LateUpdate()
        {
            if (!BuildingManager.Instance.isTownHallPlaced) return;

            Vector3 screenPosition = UnityEngine.Camera.main.WorldToScreenPoint(ObjectToFollow.position);
            if (screenPosition.z > 0)
            {
                screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
                screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);
                arrowTransform.position = screenPosition;
                arrowTransform.gameObject.SetActive(true);
            }
            else 
            {
                arrowTransform.gameObject.SetActive(false);
            }
        }
    }
}