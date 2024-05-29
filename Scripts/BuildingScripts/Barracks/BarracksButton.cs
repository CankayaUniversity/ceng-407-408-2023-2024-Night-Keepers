using System;
using UnityEngine;
using static BuildingData;

namespace NightKeepers
{
    public class BarracksButton : MonoBehaviour
    {
        public static event Action<Unit> onButtonPressed;
        private ResourceManagement resourceManagement;

        private void Start()
        {
            resourceManagement = FindObjectOfType<ResourceManagement>();
        }

        public void SendPrefabToBarracks(Unit _unitToProduce)
        {
            if (resourceManagement != null)
            {
                if (HasEnoughResources(_unitToProduce))
                {
                    DeductResources(_unitToProduce);
                    onButtonPressed?.Invoke(_unitToProduce);
                }
                else
                {
                    Debug.Log("Not enough resources to produce unit!");
                }
            }
        }

        private bool HasEnoughResources(Unit _unitToProduce)
        {
            ResourceCost cost = _unitToProduce.UnitData.Cost;
            return resourceManagement.resources.Wood >= cost.wood &&
                   resourceManagement.resources.Stone >= cost.stone &&
                   resourceManagement.resources.Iron >= cost.iron &&
                   resourceManagement.resources.Food >= cost.food;
        }

        private void DeductResources(Unit _unitToProduce)
        {
            ResourceCost cost = _unitToProduce.UnitData.Cost;
            resourceManagement.resources.Wood -= cost.wood;
            resourceManagement.resources.Stone -= cost.stone;
            resourceManagement.resources.Iron -= cost.iron;
            resourceManagement.resources.Food -= cost.food;

            resourceManagement.UpdateText(); // Kaynak metinlerinin güncellenmesi
        }
    }
}
