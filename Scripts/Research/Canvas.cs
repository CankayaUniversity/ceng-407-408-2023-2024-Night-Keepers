using UnityEngine;
using TMPro;
using System.Collections;

namespace NightKeepers.Research
{
    public class Canvas : MonoBehaviour
    {
        public TMP_Text researchText;
        private Upgrades _upgrades;
        private int cost;
        [SerializeField] GameObject researchUI;
        [SerializeField] private Buttons buttons;
        private bool isResearchBuildingConstructed = false;

        public enum CanvasButtons
        {
            House,
            Fishing,
            Farm,
            MainHall,
            StoneMine,
            Barracks,
            ResearchBuilding,
            Wall,
            Others
        }

        void Start()
        {
            researchText.text = "0";
            StartCoroutine(UpdateText());
            BuildingManager.OnBuildingPlaced += BuildingManager_OnBuildingPlaced;

            _upgrades = FindObjectOfType<Upgrades>();

            BuildingManager.Instance.SetUpgrades(_upgrades);

            if (buttons != null)
            {
                buttons.SetActiveSkills(_upgrades);
            }
        }

        private void OnDestroy()
        {
            BuildingManager.OnBuildingPlaced -= BuildingManager_OnBuildingPlaced;
        }

        private void BuildingManager_OnBuildingPlaced()
        {
            if (BuildingManager.Instance.GetCurrentBuildingType() == BuildingData.BuildingType.ResearchBuilding)
            {
                isResearchBuildingConstructed = true;
            }
        }

        IEnumerator UpdateText()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                if (isResearchBuildingConstructed)
                {
                    researchText.text = (int.Parse(researchText.text) + Random.Range(1, 4)).ToString();
                }
                else
                {
                    researchText.text = "0";
                }
            }
        }

        private void Awake()
        {
            _upgrades = new Upgrades();
            _upgrades.OnResearchUnlocked += Upgrades_OnResearchUnlocked;
        }

        public void openResearchUI()
        {
            researchUI.SetActive(true);
        }

        public void closeResearchUI()
        {
            researchUI.SetActive(false);
        }

        private void Upgrades_OnResearchUnlocked(object sender, Upgrades.OnResearchUnlockedEventArgs e)
        {
            int currentResearchValue;
            int.TryParse(researchText.text, out currentResearchValue);
            switch (e.researchUpgrades)
            {
                case Upgrades.ResearchUpgrades.House:
                    if (currentResearchValue >= 20)
                    {
                        researchText.text = (currentResearchValue - 20).ToString();
                        Debug.Log("House = Activated");
                    }
                    else
                    {
                        Debug.Log("Insufficient research value to unlock House.");
                    }
                    break;
                case Upgrades.ResearchUpgrades.Fishing:
                    if (currentResearchValue >= 50)
                    {
                        researchText.text = (currentResearchValue - 50).ToString();
                        Debug.Log("FishingHouse = Activated");
                    }
                    else
                    {
                        Debug.Log("Insufficient research value to unlock FishingHouse.");
                    }
                    break;
                case Upgrades.ResearchUpgrades.Lumberjack1:
                    if (currentResearchValue >= 10)
                    {
                        researchText.text = (currentResearchValue - 10).ToString();
                        Debug.Log("Lumberjack1 = Activated");
                    }
                    else
                    {
                        Debug.Log("Insufficient research value to unlock Lumberjack1.");
                    }
                    break;
                case Upgrades.ResearchUpgrades.StoneMine:
                    if (currentResearchValue >= 10)
                    {
                        researchText.text = (currentResearchValue - 10).ToString();
                        Debug.Log("StoneMine = Activated");
                    }
                    else
                    {
                        Debug.Log("Insufficient research value to unlock StoneMine.");
                    }
                    break;
                case Upgrades.ResearchUpgrades.Wall:
                    if (currentResearchValue >= 10)
                    {
                        researchText.text = (currentResearchValue - 10).ToString();
                        Debug.Log("Wall = Activated");
                    }
                    else
                    {
                        Debug.Log("Insufficient research value to unlock Wall.");
                    }
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
    }

}
