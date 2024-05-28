using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseManager : Singleton<PlayerBaseManager>
{
    [SerializeField]
    private List<GameObject> _playerBaseList = new List<GameObject>();

    private void OnEnable()
    {
        BuildingManager.OnMainBuildingPlaced += OnMainBuildingPlaced;
    }

    private void OnDisable()
    {
        BuildingManager.OnMainBuildingPlaced -= OnMainBuildingPlaced;
    }

    private void OnMainBuildingPlaced(GameObject mainBuilding)
    {
        _playerBaseList.Add(mainBuilding);
    }

    private void Start()
    {
        //_playerBaseList.Add(GameObject.Find("PlayerMainBuilding"));
    }

    public Vector3 GetSelectedBasePosition()
    {
        if (_playerBaseList.Count > 0 & _playerBaseList[0] != null)
        {
            // for now it only picks the first base in the list later we will have to create a logic to pick one and spawn enemies according to that and pick the base according to that
            return _playerBaseList[0].transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}