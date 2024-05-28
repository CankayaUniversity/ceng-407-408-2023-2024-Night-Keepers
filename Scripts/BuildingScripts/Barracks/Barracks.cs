using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NightKeepers
{
    public class Barracks : FunctionalBuilding
    {
        private List<Unit> _unitProductionList = new List<Unit>();

        private bool _isInProduction;
        private int _currentNumberOfProductions = 0;

        public static event Action onListUpdated;

        public void InsertUnitToList(Unit unitToProduce)
        {
            _unitProductionList.Add(unitToProduce);
            onListUpdated?.Invoke();
            _currentNumberOfProductions++;

            if (!_isInProduction)
            {
                StartCoroutine(ProduceUnit());
            }
        }

        public void TransferUnitFromProductionToReady()
        {
            Unit unit = _unitProductionList[0];
            _unitProductionList.RemoveAt(0);
            onListUpdated?.Invoke();
            _currentNumberOfProductions--;
            PlayerUnitManager.Instance.AddUnitToReadyList(unit);
            //return unit;
        }

        IEnumerator ProduceUnit()
        {
            _isInProduction = true;
            while (_unitProductionList.Count > 0)
            {
                yield return new WaitForSeconds(_unitProductionList[0].UnitData.ProductionTime);
                TransferUnitFromProductionToReady();
            }
            _isInProduction = false;
        }

        public List<Unit> GetProductionList()
        {
            return _unitProductionList;
        }

        public int GetCurrentListCount()
        {
            return _unitProductionList.Count;
        }

        public Unit GetLastElementOfList()
        {
            return _unitProductionList[^1];
        }

        public int GetCurrentNumberOfProductions()
        {
            return _currentNumberOfProductions;
        }
    }
}