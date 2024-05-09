using NightKeepers.Research;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static NightKeepers.Research.Canvas;
using static UnityEditor.ObjectChangeEventStream;

namespace NightKeepers.Research
{
    public class Buttons : MonoBehaviour
    {
        public Upgrades _upgrades;
        public List<BuildingUI> builduis;
        public TMP_Text researchText;
        public GameObject[] buildingsArray;
        public void Awake()
        {
            buildingsArray[0].GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Lumberjack1, researchText));
            buildingsArray[1].GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Lumberjack2, researchText));
            buildingsArray[2].GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Farm, researchText));
            buildingsArray[3].GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.IronMine, researchText));
            buildingsArray[4].GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.StoneMine, researchText));
            buildingsArray[5].GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Wall, researchText));
            /* transform.Find("Lumberjack1").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Lumberjack1,researchText));
             transform.Find("Lumberjack2").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Lumberjack2,researchText));
             transform.Find("Farm").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Farm,researchText));
             transform.Find("IronMine").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.IronMine,researchText));
             transform.Find("StoneMine").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.StoneMine,researchText));
             transform.Find("Wall").GetComponent<Button>().onClick.AddListener(() => _upgrades.TryUnlock(Upgrades.ResearchUpgrades.Wall,researchText));  */
        }

        public void SetActiveSkills(Upgrades upgrades)
        {
            this._upgrades = upgrades;
            foreach(var buildUI in builduis)
                {
                buildUI.upgrades = upgrades;
            }
            
        }
    }
}

