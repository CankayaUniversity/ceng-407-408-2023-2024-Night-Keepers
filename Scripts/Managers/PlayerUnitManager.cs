using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NightKeepers
{
    public class PlayerUnitManager : Singleton<PlayerUnitManager>
    {
        private Dictionary<Unit, int> _unitDictionary = new Dictionary<Unit, int>();
        private Dictionary<Unit, int> _placedUnitDictionary = new Dictionary<Unit, int>();
        private List<GameObject> _placedUnitList = new List<GameObject>();

        [SerializeField] private RectTransform _readyUnitHolder;
        [SerializeField] private List<GameObject> _unitButtons;

        private void OnEnable()
        {
            PlayerUnit.OnPlacedUnitDied += OnPlacedUnitDied;
            TimeManager.OnDayArrived += OnDayArrived;
        }

        private void OnDisable()
        {
            PlayerUnit.OnPlacedUnitDied -= OnPlacedUnitDied;
            TimeManager.OnDayArrived += OnDayArrived;
        }

        private void Start()
        {
            foreach (Transform unitButton in _readyUnitHolder)
            {
                _unitButtons.Add(unitButton.gameObject);
                unitButton.gameObject.SetActive(false);
            }
        }

        private void OnPlacedUnitDied(Unit unit)
        {
            if (_placedUnitDictionary.ContainsKey(unit))
            {
                _placedUnitDictionary[unit]--;
                _placedUnitList.Remove(unit.gameObject);
            }
        }

        private void OnDayArrived()
        {
            foreach (var kvp in _placedUnitDictionary)
            {
                if (_unitDictionary.ContainsKey(kvp.Key))
                {
                    _unitDictionary[kvp.Key] += kvp.Value;

                    UpdateButtons(kvp.Key);
                }
            }
            foreach (GameObject unit in _placedUnitList)
            {
                Destroy(unit);
            }
            _placedUnitList.Clear();
            _placedUnitDictionary.Clear();
        }

        public void AddUnitToReadyList(Unit unit)
        {
            if (_unitDictionary.ContainsKey(unit))
            {
                _unitDictionary[unit]++;
            }
            else
            {
                _unitDictionary[unit] = 1;
            }

            UpdateButtons(unit);
        }

        private void UpdateButtons(Unit unit)
        {
            foreach (GameObject unitButton in _unitButtons)
            {
                var selectUnitButton = unitButton.GetComponent<SelectUnitButton>();
                if (unit.UnitData.UnitButtonPrefab.name == unitButton.name)
                {
                    unitButton.SetActive(true);
                    selectUnitButton.UpdateText(GetUnitCount(unit));
                }
            }
        }

        public int GetUnitCount(Unit unit)
        {
            return _unitDictionary.ContainsKey(unit) ? _unitDictionary[unit] : 0;
        }

        public Unit GetUnitByButtonName(string buttonName)
        {
            Unit unit = _unitDictionary.FirstOrDefault(kvp => kvp.Key.UnitData.UnitButtonPrefab.name == buttonName && kvp.Value != 0).Key;
            return unit;
        }

        public void DecreaseUnitCount(Unit unit)
        {
            if (_unitDictionary.ContainsKey(unit))
            {
                _unitDictionary[unit]--;
                UpdateButtons(unit);

                if (_placedUnitDictionary.ContainsKey(unit))
                {
                    _placedUnitDictionary[unit]++;
                }
                else
                {
                    _placedUnitDictionary[unit] = 1;
                }
            }
        }

        public void AddToPlacedUnitList(GameObject unitObject)
        {
            _placedUnitList.Add(unitObject);
        }
    }
}