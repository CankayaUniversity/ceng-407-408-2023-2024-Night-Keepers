using NightKeepers.Research;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static NightKeepers.Research.Canvas;

namespace NightKeepers.Research
{
    public class Buttons : MonoBehaviour
    {

        [SerializeField] private TMP_Text resourceValue;
        private Upgrades _upgrades;

        private void Awake()
        {
            
            transform.Find("MeleeUnitT1").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.MeleeUnitsBuff));
            transform.Find("MeleeUnitT2").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.MeleeUnitsBuff2));
            transform.Find("RangeUnit").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.RangeUnitsBuff));
            transform.Find("BuildingsBuff").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.BuildingsBuff));
            transform.Find("OthersBuff").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.OthersBuff));
        }

        public void SetActiveSkills(Upgrades upgrades)
        {
            this._upgrades = upgrades;
            
        }
    }
}
