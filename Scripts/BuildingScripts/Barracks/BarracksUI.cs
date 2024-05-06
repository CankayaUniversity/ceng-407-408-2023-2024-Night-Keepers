using UnityEngine;

namespace NightKeepers
{
    public class BarracksUI : MonoBehaviour
    {
        [SerializeField] private Barracks _selectedBarrack;
        [SerializeField] private RectTransform queueHolder;

        private void Start()
        {
            //foreach (RectTransform image in queueHolder)
            //{
            //    _queueImageList.Add(image.GetComponent<Image>());
            //    image.gameObject.SetActive(false);
            //}
        }

        private void OnEnable()
        {
            BarracksButton.onButtonPressed += OnButtonPressed;
            Barracks.onListUpdated += OnListUpdated;
        }

        private void OnDisable()
        {
            BarracksButton.onButtonPressed -= OnButtonPressed;
            Barracks.onListUpdated -= OnListUpdated;
        }

        private void OnButtonPressed(Unit unitToProduce)
        {
            ProduceUnit(unitToProduce);
        }

        // should update the UI when a different barrack is selected not on validate this causes an error right now at the start because it runs too early
        private void OnValidate()
        {
            //Debug.Log("Barrack has been changed in the inspector! Or something else.");
            //OnListUpdated();
        }

        private void OnListUpdated()
        {
            UpdateImages();
        }

        private void UpdateImages()
        {
            // temporary. Normally units needs their image to put insted of colors. Needs pooling.

            foreach(RectTransform image in queueHolder)
            {
                Destroy(image.gameObject);
            }

            foreach(Unit unit in _selectedBarrack.GetProductionList())
            {
                Instantiate(unit.UnitData.UnitImagePrefab, queueHolder);
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