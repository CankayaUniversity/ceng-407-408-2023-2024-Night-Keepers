using UnityEngine;

namespace NightKeepers
{
    public class DayNightUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform dayNightIndicator;

        private void FixedUpdate()
        {
            if (TimeManager.Instance.isTimeStarted)
            {
                float progressionRatio = TimeManager.Instance.GetProgressionRatio();
                float rotationAngle;

                if (TimeManager.Instance.IsDay)
                {
                    rotationAngle = Mathf.Lerp(180f, 0f, progressionRatio);
                }
                else
                {
                    rotationAngle = Mathf.Lerp(0f, -180f, progressionRatio);
                }
                dayNightIndicator.rotation = Quaternion.Euler(0, 0, rotationAngle);
            }
        }
    }
}