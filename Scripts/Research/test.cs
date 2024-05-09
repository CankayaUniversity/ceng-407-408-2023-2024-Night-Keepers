using NightKeepers.Research;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NightKeepers.Research
{
    public class test : MonoBehaviour
    {
        [SerializeField] private Research.Canvas canvas;
        [SerializeField] private Buttons buttons;

        private void Start()
        {
            buttons.SetActiveSkills(canvas.GetUpgrades());
        }
    }

}
