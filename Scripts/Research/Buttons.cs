using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace NightKeepers.Research
{
    public class Buttons : MonoBehaviour
    {
        public Upgrades _upgrades;
        public List<BuildingUI> builduis;
        public TMP_Text researchText;
        public GameObject[] buildingsArray;

        private void Awake()
        {
            buildingsArray[0]?.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(Upgrades.ResearchUpgrades.House));
            buildingsArray[1]?.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(Upgrades.ResearchUpgrades.Fishing));
            buildingsArray[2]?.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(Upgrades.ResearchUpgrades.Lumberjack1));
            buildingsArray[3]?.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(Upgrades.ResearchUpgrades.StoneMine));
            buildingsArray[4]?.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(Upgrades.ResearchUpgrades.Wall));
        }

        private void OnButtonClick(Upgrades.ResearchUpgrades upgrade)
        {
            if (_upgrades != null && researchText != null)
            {
                _upgrades.TryUnlock(upgrade, researchText);
            }
        }

        public void SetActiveSkills(Upgrades upgrades)
        {
            this._upgrades = upgrades;

            BuildingManager.Instance.SetUpgrades(upgrades);

            foreach (var buildUI in builduis)
            {
                buildUI.upgrades = upgrades;
            }
        }
    }
}
