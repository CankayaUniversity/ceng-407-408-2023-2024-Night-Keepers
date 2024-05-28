using UnityEngine;
using UnityEngine.AI;

namespace NightKeepers
{
    public class PlayerUnitPlacer : MonoBehaviour
    {
        private Unit _unitReferance;

        private GameObject _spawnedUnitObject;
        private NavMeshAgent _spawnedUnitAgent;
        private Unit _spawnedUnit;

        private bool _isPlacingUnit = false;

        private void OnEnable()
        {
            SelectUnitButton.OnUnitSelected += OnUnitSelectedHandler;
        }

        private void OnDisable()
        {
            SelectUnitButton.OnUnitSelected -= OnUnitSelectedHandler;
        }

        private void OnUnitSelectedHandler(GameObject unitObject)
        {
            _unitReferance = unitObject.GetComponent<Unit>();
            _spawnedUnitObject = Instantiate(unitObject);
            _spawnedUnitAgent = _spawnedUnitObject.GetComponent<NavMeshAgent>();
            _spawnedUnit = _spawnedUnitObject.GetComponent<Unit>();
            _isPlacingUnit = true;
        }

        private void Update()
        {
            if (_isPlacingUnit)
            {
                UpdateSpawnedUnitPosition();

                if (Input.GetMouseButtonDown(0))
                {
                    PlaceUnit();
                }
            }
        }

        private void UpdateSpawnedUnitPosition()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tile")))
            {
                _spawnedUnitObject.transform.position = hit.point + Vector3.up;
            }
        }

        private void PlaceUnit()
        {
            _isPlacingUnit = false;
            _spawnedUnit.enabled = true;
            _spawnedUnitAgent.enabled = true;
            PlayerUnitManager.Instance.DecreaseUnitCount(_unitReferance);
            PlayerUnitManager.Instance.AddToPlacedUnitList(_spawnedUnitObject);
        }
    }
}