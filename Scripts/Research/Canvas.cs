using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace NightKeepers.Research
{
    public class Canvas : MonoBehaviour
    {
        public  TMP_Text text;
        private Upgrades _upgrades;      
        public enum CanvasButtons
        {
            MeleeUnits,
            MeleeUnits2,
            RangeUnits,
            Buildings,
            Others
        }
       
        //Do list of CanvasButtons
        
        void Start()
        {
            //text.text will increase every 2 seconds by 1-4.
            text.text = text.GetComponent<TMP_Text>().text;
            StartCoroutine(UpdateText());
        }

        IEnumerator UpdateText()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                text.text = (int.Parse(text.text) + Random.Range(1, 4)).ToString();
            }
        }
        private void Awake()
        {
            _upgrades = new Upgrades();
            _upgrades.OnResearchUnlocked += Upgrades_OnResearchUnlocked;
        }

        private void Upgrades_OnResearchUnlocked(object sender, Upgrades.OnResearchUnlockedEventArgs e)
        {
            switch (e.researchUpgrades)
            {
                case Upgrades.ResearchUpgrades.MeleeUnitsBuff:
                    Debug.Log("MeleeUnitsBuff = Activated");
                    break;
                case Upgrades.ResearchUpgrades.MeleeUnitsBuff2: 
                    Debug.Log("MeleeUnitsBuff2 = Activated");
                    break;
                case Upgrades.ResearchUpgrades.RangeUnitsBuff:
                    Debug.Log("RangeUnitsBuff = Activated");
                    break;
                case Upgrades.ResearchUpgrades.BuildingsBuff:
                    Debug.Log("BuildingsBuff = Activated");
                    break;
                case Upgrades.ResearchUpgrades.OthersBuff:
                    Debug.Log("OthersBuff = Activated");
                    break;

            }
        }

        public Upgrades GetUpgrades()
        {
            return _upgrades;
        }

        public bool MeleeUnitsBuffActive()
        {
            return _upgrades.IsUnlocked(Upgrades.ResearchUpgrades.MeleeUnitsBuff);
        }
        public bool MeleeUnitsBuff2Active()
        {
            return _upgrades.IsUnlocked(Upgrades.ResearchUpgrades.MeleeUnitsBuff2);
        }
        public bool RangeUnitsBuffActive()
        {
            return _upgrades.IsUnlocked(Upgrades.ResearchUpgrades.RangeUnitsBuff);
        }
        public bool BuildingsBuffActive()
        {
            return _upgrades.IsUnlocked(Upgrades.ResearchUpgrades.BuildingsBuff);
        }
        public bool OthersBuffActive()
        {
            return _upgrades.IsUnlocked(Upgrades.ResearchUpgrades.OthersBuff);
        }

    }
}
