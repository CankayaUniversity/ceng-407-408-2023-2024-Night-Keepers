using System;
using UnityEngine;

namespace NightKeepers
{
    public class BarracksButton : MonoBehaviour
    {
        public static event Action<Unit> onButtonPressed;

        public void SendPrefabToBarracks(Unit _unitToProduce)
        {
            onButtonPressed?.Invoke(_unitToProduce);
        }
    }
}