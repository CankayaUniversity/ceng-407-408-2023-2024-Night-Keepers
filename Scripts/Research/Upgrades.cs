using NightKeepers;
using NightKeepers.Research;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Lumin;
using UnityEngine.TextCore.Text;
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
            Lumberjack2,
            Lumberjack1,
            Lumberjack,
            Farm,
            StoneMine,
            IronMine,
            Wall,
            OthersBuff
        }

        public List<ResearchUpgrades> unlockedUpgrades;

        public Upgrades()
        {
            unlockedUpgrades = new List<ResearchUpgrades>();
        }
        public void UnlockUpgrades(ResearchUpgrades upgrades)
        {
            if (!IsUnlocked(upgrades))
            {

                unlockedUpgrades.Add(upgrades);
                OnResearchUnlocked?.Invoke(this, new OnResearchUnlockedEventArgs { researchUpgrades = upgrades });
            }

        }

        public bool IsUnlocked(ResearchUpgrades upgrades)
        {
            Debug.Log($"List of characters: [{string.Join(", ", unlockedUpgrades)}]");
            Debug.Log(unlockedUpgrades.Count);
            return unlockedUpgrades.Contains(upgrades);
        }

        public ResearchUpgrades GetResearchRequirement(ResearchUpgrades upgrades)
        {
            switch (upgrades)
            {

                case ResearchUpgrades.Lumberjack2:                  
                        return ResearchUpgrades.Lumberjack1;                           
            }
            return ResearchUpgrades.None;
        }
        public bool TryUnlock(ResearchUpgrades upgrades, TMP_Text researchText)
        {
            ResearchUpgrades requirement = GetResearchRequirement(upgrades);
            int currentResearchValue;
            int.TryParse(researchText.text, out currentResearchValue);
            //Debug.Log("vALUE :" + currentResearchValue);
            if (requirement != ResearchUpgrades.None)
            {
                //Debug.Log("vALUE :" + currentResearchValue);
                if (IsUnlocked(requirement))
                {                   
                    if (upgrades == ResearchUpgrades.Lumberjack2 && currentResearchValue >= 50)
                    {
                        Debug.Log("vALUE :" + currentResearchValue);
                        UnlockUpgrades(upgrades);
                        Debug.Log($"Upgrade {upgrades} unlocked!");
                        return true;
                    }
                    
                    else
                    {
                        Debug.Log("Insufficient ResearchPoint");
                        return false;
                    }
                }
                else
                {
                    Debug.Log($"You need to unlock {requirement} first");
                    return false;
                }
            }
            else
            {
                if (upgrades == ResearchUpgrades.Lumberjack1 && currentResearchValue >= 20)
                {
                    Debug.Log("VALUE :" + currentResearchValue);
                    UnlockUpgrades(upgrades);
                    Debug.Log("VALUE1 :" + currentResearchValue);
                    Debug.Log($"Upgrade {upgrades} unlocked!");
                    return true;
                }
                else if (upgrades == ResearchUpgrades.StoneMine && currentResearchValue >= 10)
                {
                    Debug.Log("vALUE :" + currentResearchValue);
                    UnlockUpgrades(upgrades);
                    Debug.Log($"Upgrade {upgrades} unlocked!");
                    return true;
                }
                else if (upgrades == ResearchUpgrades.IronMine && currentResearchValue >= 10)
                {
                    Debug.Log("vALUE :" + currentResearchValue);
                    UnlockUpgrades(upgrades);
                    Debug.Log($"Upgrade {upgrades} unlocked!");
                    return true;
                }
                else if (upgrades == ResearchUpgrades.Farm && currentResearchValue >= 10)
                {
                    Debug.Log("vALUE :" + currentResearchValue);
                    UnlockUpgrades(upgrades);
                    Debug.Log($"Upgrade {upgrades} unlocked!");
                    return true;
                }
                else
                {
                    Debug.Log("Insufficient ResearchPoint");
                    return false;
                }
            }
        }

        /* public bool TryUnlock(ResearchUpgrades upgrades, TMP_Text researchText)
         {

             ResearchUpgrades requirement = GetResearchRequirement(upgrades);
             int currentResearchValue;
             int.TryParse(researchText.text, out currentResearchValue);

             if (requirement != ResearchUpgrades.None)
             {
                 if (IsUnlocked(requirement))
                 {
                     if(upgrades == ResearchUpgrades.Lumberjack1)
                     {
                         if(currentResearchValue >= 20)
                         {
                             UnlockUpgrades(upgrades);
                             Debug.Log($"List of characters: [{string.Join(", ", unlockedUpgrades)}]");
                             return true;
                         }
                         else
                         {
                             Debug.Log("Insufficient Point");
                             return false;
                         }
                     }
                     else if(upgrades == ResearchUpgrades.Lumberjack2)
                     {
                         if (currentResearchValue >= 50)
                         {
                             UnlockUpgrades(upgrades);
                             Debug.Log($"List of characters: [{string.Join(", ", unlockedUpgrades)}]");
                             return true;
                         }
                         else
                         {
                             Debug.Log("Insufficient Point");
                             return false;
                         }
                     }
                     else if (upgrades == ResearchUpgrades.StoneMine)
                     {
                         if (currentResearchValue >= 10)
                         {
                             UnlockUpgrades(upgrades);
                             Debug.Log($"List of characters: [{string.Join(", ", unlockedUpgrades)}]");
                             return true;
                         }
                         else
                         {
                             Debug.Log("Insufficient Point");
                             return false;
                         }
                     }
                     else if (upgrades == ResearchUpgrades.IronMine)
                     {
                         if (currentResearchValue >= 10)
                         {
                             UnlockUpgrades(upgrades);
                             Debug.Log($"List of characters: [{string.Join(", ", unlockedUpgrades)}]");
                             return true;
                         }
                         else
                         {
                             Debug.Log("Insufficient Point");
                             return false;
                         }
                     }
                     else if (upgrades == ResearchUpgrades.Farm)
                     {
                         if (currentResearchValue >= 10)
                         {
                             UnlockUpgrades(upgrades);
                             Debug.Log($"List of characters: [{string.Join(", ", unlockedUpgrades)}]");
                             return true;
                         }
                         else
                         {
                             Debug.Log("Insufficient Point");
                             return false;
                         }
                     }

                 }
                 else
                 {
                     Debug.Log("You need to unlock " + requirement + " first");
                     return false;
                 }
             }
             else
             {            
                     UnlockUpgrades(upgrades);
                     return true;  
             }

         }*/


    }

}
