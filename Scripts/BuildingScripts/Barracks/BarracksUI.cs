using UnityEngine;

namespace NightKeepers
{
    public class BarracksUI : MonoBehaviour
    {
        [SerializeField] private Barracks _selectedBarrack;
        [SerializeField] private RectTransform queueHolder;
        [SerializeField] private GameObject UIHolder;

        private void Start()
        {
            UIHolder.SetActive(false);
        }

        private void OnEnable()
        {
            BarracksButton.onButtonPressed += OnButtonPressed;
            Barracks.onListUpdated += OnListUpdated;
            SelectionManager.onBuildingSelected += OnBuildingSelected;
        }

        private void OnDisable()
        {
            BarracksButton.onButtonPressed -= OnButtonPressed;
            Barracks.onListUpdated -= OnListUpdated;
            SelectionManager.onBuildingSelected -= OnBuildingSelected;
        }

        private void OnBuildingSelected(FunctionalBuilding barracks)
        {
            if (barracks.GetType() == typeof(Barracks))
            {
                _selectedBarrack = (Barracks)barracks;
                UIHolder.SetActive(true);
                OnListUpdated();
            }
            else{
                _selectedBarrack = null;
                UIHolder.SetActive(false);
            }
        }

        private void OnButtonPressed(Unit unitToProduce)
        {
            ProduceUnit(unitToProduce);
        }

        private void OnListUpdated()
        {
            UpdateImages();
        }

        private void UpdateImages()
        {
            // Needs pooling.

            foreach(RectTransform image in queueHolder)
            {
                Destroy(image.gameObject);
            }

            foreach(Unit unit in _selectedBarrack.GetProductionList())
            {
                Instantiate(unit.UnitData.UnitButtonPrefab, queueHolder);
            }
        }

        private void ProduceUnit(Unit unitToProduce)
        {
            if (_selectedBarrack.GetCurrentNumberOfProductions() < 5)
            {
                _selectedBarrack.InsertUnitToList(unitToProduce);
                UpdateImages();
            }
        }
    }
}