using System;
using TMPro;
using UnityEngine;

namespace NightKeepers
{
    public class SelectUnitButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _numberOfUnitsText;

        public static event Action<GameObject> OnUnitSelected;

        public void UpdateText(int numberOfUnits)
        {
            _numberOfUnitsText.text = numberOfUnits.ToString();
        }

        public void SelectUnit()
        {
            Unit unit = PlayerUnitManager.Instance.GetUnitByButtonName(gameObject.name);

            if (unit != null)
            {
                OnUnitSelected?.Invoke(unit.gameObject);
            }
            else
            {
                Debug.Log("No More Unit Left");
            }
        }
    }
}