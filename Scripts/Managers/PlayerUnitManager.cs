using System.Collections.Generic;
using UnityEngine;

namespace NightKeepers
{
    public class PlayerUnitManager : Singleton<PlayerUnitManager>
    {
        [SerializeField] private List<Unit> _readyUnitList = new List<Unit>();

        [SerializeField] private RectTransform _readyUnitHolder;

        public void AddUnitToReadyList(Unit unit)
        {
            _readyUnitList.Add(unit);
            Instantiate(unit.UnitData.UnitImagePrefab, _readyUnitHolder);
        }
    }
}