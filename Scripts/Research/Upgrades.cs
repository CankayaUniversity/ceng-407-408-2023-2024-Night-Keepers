using NightKeepers;
using NightKeepers.Research;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static NightKeepers.Research.Canvas;

namespace NightKeepers.Research
{
    public class Upgrades : MonoBehaviour
    {
        public event EventHandler<OnResearchUnlockedEventArgs> OnResearchUnlocked;

        public class OnResearchUnlockedEventArgs : EventArgs
        {
            public ResearchUpgrades researchUpgrades;
        }
        public enum ResearchUpgrades
        {
            None,
            MeleeUnitsBuff,
            MeleeUnitsBuff2,
            RangeUnitsBuff,
            BuildingsBuff,
            OthersBuff
        }

        public List<ResearchUpgrades> unlockedUpgrades;
        public Upgrades()
        {
            unlockedUpgrades = new List<ResearchUpgrades>();
        }
        private void UnlockUpgrades(ResearchUpgrades upgrades)
        {
            if (!IsUnlocked(upgrades))
            {
                unlockedUpgrades.Add(upgrades);
                OnResearchUnlocked?.Invoke(this, new OnResearchUnlockedEventArgs { researchUpgrades = upgrades });
            }          
        }
        public bool IsUnlocked(ResearchUpgrades upgrades)
        {
            return unlockedUpgrades.Contains(upgrades);
        }
       public ResearchUpgrades GetResearchRequirement(ResearchUpgrades upgrades)
        {
            switch (upgrades)
            {
                case ResearchUpgrades.MeleeUnitsBuff2:
                    return ResearchUpgrades.MeleeUnitsBuff;              
            }
            return ResearchUpgrades.None;
        }
        public bool TryUnlock(ResearchUpgrades upgrades)
        {
            ResearchUpgrades requirement = GetResearchRequirement(upgrades);
            if (requirement != ResearchUpgrades.None)
            {
                if (IsUnlocked(requirement))
                {
                    UnlockUpgrades(upgrades);
                    return true;
                }
                else
                    Debug.Log("You need to unlock " + requirement + " first");
                    return false;
            }else
            {
                UnlockUpgrades(upgrades);
                return true;
            }
        }

    }

}





