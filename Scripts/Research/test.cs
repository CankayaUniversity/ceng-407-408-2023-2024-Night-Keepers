using NightKeepers.Research;
using UnityEngine;

namespace NightKeepers.Research
{
    public class test : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Buttons buttons;

        private void Start()
        {
            if (canvas != null)
            {
                buttons.SetActiveSkills(canvas.GetUpgrades());
            }

        }
    }
}
