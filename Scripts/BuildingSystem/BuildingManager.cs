using NightKeepers;
using NightKeepers.Research;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private List<Building> buildings;
    [SerializeField] private List<GameObject> previews;
    private List<Building> buildingPreviews = new List<Building>();
    private List<MeshRenderer> meshRendererPreviews = new List<MeshRenderer>();
    [SerializeField] private Material validPreviewMaterial;
    [SerializeField] private Material invalidPreviewMaterial;
    [SerializeField] private Material buildingPlacementMaterial;
    public static event Action<GameObject> OnMainBuildingPlaced;
    public static event Action OnBuildingPlaced;
    private BuildingData.BuildingType buildingType;
    private Vector2Int gridPosition;
    private bool isRotated = false;
    private int buildingNumber;
    public bool isPlaced = false;
    bool isPlaceBuilding = false;
    bool isBuildingMode = true;
    public bool isTownHallPlaced = false;
    public int sameTileCount { get; private set; }
    private float buildingMultiplier;
    private float deleteCooldown;

    [SerializeField] private LayerMask layerMask;

    private Upgrades upgrades;

    private Dictionary<int, List<Material>> buildingMaterials = new Dictionary<int, List<Material>>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var preview in previews)
        {
            buildingPreviews.Add(preview.GetComponentInChildren<Building>());
        }
        int i = 0;
        foreach (var buildingPreview in buildingPreviews)
        {
            meshRendererPreviews.Add(buildingPreviews[i++].GetComponentInChildren<MeshRenderer>());
        }
        int j = 0;
        foreach (var building in buildings)
        {
            List<Material> materials = new List<Material>();
            foreach (var material in building.GetComponentInChildren<MeshRenderer>().sharedMaterials)
            {
               materials.Add(material);
            }
            buildingMaterials.Add(j, materials);
            j++;
        }
    }

    private void Start()
    {
        foreach (var preview in previews)
        {
            preview.transform.localScale = new Vector3(preview.transform.localScale.x * GridManager.Instance.cellSize / 10, preview.transform.localScale.y * GridManager.Instance.cellSize / 10, preview.transform.localScale.z * GridManager.Instance.cellSize / 10);
            preview.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        SelectBuilding();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layerMask))
        {
            gridPosition = GridManager.Instance._grid.WorldToGridPosition(raycastHit.point);
            if (!TimeManager.Instance.IsDay)
            {
                previews[buildingNumber].transform.position = new Vector3(-100f, 0, 0);
                return;
            }
            if (isBuildingMode && GridManager.Instance._grid.IsInDimensions(gridPosition))
            {
                isPlaceBuilding = true;
                PreviewBuilding(gridPosition);
            }
            else
            {
                previews[buildingNumber].transform.position = Vector3.zero;
            }
        }
        else
        {
            isPlaceBuilding = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRotated = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isBuildingMode = false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildingMode = true;
        }

        if (isPlaceBuilding && isBuildingMode)
        {
            if (!TimeManager.Instance.IsDay)
            {
                return;
            }
            PlaceBuilding(gridPosition);
        }
    }

    private void SelectBuilding()
    {
        switch (buildingType)
        {
            case BuildingData.BuildingType.StoneMine:
                isPlaced = false;
                buildingNumber = 0;
                buildingMultiplier = 4f;
                BuildingPreviewsActivate(buildingNumber);
                break;

            case BuildingData.BuildingType.IronMine:
                isPlaced = false;
                buildingNumber = 1;
                buildingMultiplier = 4f;
                BuildingPreviewsActivate(buildingNumber);
                break;

            case BuildingData.BuildingType.Lumberjack:
                isPlaced = false;
                buildingNumber = 2;
                buildingMultiplier = 4f;
                BuildingPreviewsActivate(buildingNumber);
                break;

            case BuildingData.BuildingType.TownHall:
                isPlaced = false;
                buildingNumber = 3;
                buildingMultiplier = 10f;
                BuildingPreviewsActivate(buildingNumber);
                break;

            case BuildingData.BuildingType.Farm:
                isPlaced = false;
                buildingNumber = 4;
                buildingMultiplier = 4f;
                BuildingPreviewsActivate(buildingNumber);
                break;
            case BuildingData.BuildingType.Wall:
                isPlaced = false;
                buildingNumber = 5;
                buildingMultiplier = 4f;
                BuildingPreviewsActivate(buildingNumber);
                break;
            case BuildingData.BuildingType.House:
                isPlaced = false;
                buildingNumber = 6;
                buildingMultiplier = 2f;
                BuildingPreviewsActivate(buildingNumber);
                break;
            case BuildingData.BuildingType.ResearchBuilding:
                isPlaced = false;
                buildingNumber = 7;
                buildingMultiplier = 10f;
                BuildingPreviewsActivate(buildingNumber);
                break;
            case BuildingData.BuildingType.Barracks:
                isPlaced = false;
                buildingNumber = 8;
                buildingMultiplier = 4f;
                BuildingPreviewsActivate(buildingNumber);
                break;
        }
    }

    private void BuildingPreviewsActivate(int active)
    {
        for (int i = 0; i < previews.Count; i++)
        {
            if (i == active)
            {
                previews[i].SetActive(true);
            }
            else
            {
                previews[i].SetActive(false);
            }
        }
    }

    private void PreviewBuilding(Vector2Int gridPosition)
    {
        if (!isPlaced)
        {
            if (TryBuild(buildings[buildingNumber], buildings[buildingNumber].buildingData.GetGridPositionList(gridPosition, buildings[buildingNumber].direction)))
            {
                var yourMaterials = new Material[]
                {
                    validPreviewMaterial, validPreviewMaterial
                };

                meshRendererPreviews[buildingNumber].materials = yourMaterials;
            }
            else
            {
                var yourMaterials = new Material[]
                {
                    invalidPreviewMaterial, invalidPreviewMaterial
                };

                meshRendererPreviews[buildingNumber].materials = yourMaterials;
            }

            Vector2Int rotationOffset = buildingPreviews[buildingNumber].buildingData.GetRotationOffset(buildingPreviews[buildingNumber].direction);
            Vector3 instantiatedBuildingWorldPosition = GridManager.Instance._grid.GridToWorldPosition(gridPosition) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * GridManager.Instance.cellSize;
            previews[buildingNumber].transform.position = instantiatedBuildingWorldPosition;
            if (isRotated)
            {
                buildingPreviews[buildingNumber].direction = BuildingData.GetNextDir(buildingPreviews[buildingNumber].direction);
                previews[buildingNumber].transform.rotation = Quaternion.Euler(0, buildingPreviews[buildingNumber].buildingData.GetRotationAngle(buildingPreviews[buildingNumber].direction), 0);
                isRotated = false;
            }
        }
    }

    public Vector2Int GetPreviewPosition()
    {
        return GridManager.Instance._grid.WorldToGridPosition(previews[buildingNumber].transform.position);
    }

    private void PlaceBuilding(Vector2Int gridPosition)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            List<Vector2Int> gridPositionList = buildings[buildingNumber].buildingData.GetGridPositionList(gridPosition, buildingPreviews[buildingNumber].direction);
            if (!isTownHallPlaced && buildingNumber != 3)
            {
                Debug.Log("You must place the TownHall first.");
                return;
            }

            var currentBuildingType = buildings[buildingNumber].buildingData.buildingTypes;

            if (!IsBuildingResearchUnlocked(currentBuildingType))
            {
                Debug.Log("You need to unlock the research for this building first.");
                return;
            }

            if (TryBuild(buildings[buildingNumber], gridPositionList))
            {
                foreach (var position in gridPositionList)
                {
                    if (GridManager.Instance._grid[position].building == null)
                    {
                        continue;
                    }
                    
                    if (GridManager.Instance._grid[position].building.buildingType == BuildingData.BuildingType.Environment)
                    {
                        DeleteBuilding(position);
                    }
                }
                Building instantiatedBuilding = Instantiate(
                    buildings[buildingNumber],
                    previews[buildingNumber].transform.position,
                    Quaternion.Euler(0, buildings[buildingNumber].buildingData.GetRotationAngle(buildingPreviews[buildingNumber].direction), 0));

                foreach (Vector2Int position in gridPositionList)
                {
                    Tile tile = new Tile()
                    {
                        building = instantiatedBuilding,
                        tileType = GridManager.Instance._grid[position].tileType,
                    };
                    GridManager.Instance._grid[position] = tile;
                }
                float counter = instantiatedBuilding.buildingData.buildingTime;
                int materialCount = buildingMaterials[buildingNumber].Count;
                float phaseTime = counter / materialCount;
                MeshRenderer meshRenderer = instantiatedBuilding.GetComponentInChildren<MeshRenderer>();

                sameTileCount = CountSameTiles(gridPosition, buildings[buildingNumber].buildingData.placableTileTypes[0]);

                OnBuildingPlaced?.Invoke();

                StartCoroutine(BuildCoroutine(instantiatedBuilding, buildingMultiplier));

                RM.Instance.SetBuildingData(buildings[buildingNumber].buildingData);
                Debug.Log($"Building placed: {buildings[buildingNumber].buildingData.name}");

                if (buildingNumber == 3)
                {
                    isTownHallPlaced = true;
                    TimeManager.Instance.isTimeStarted = true;
                    OnMainBuildingPlaced?.Invoke(instantiatedBuilding.gameObject);
                }
            }
        }
    }
    private bool IsBuildingResearchUnlocked(BuildingData.BuildingType buildingType)
    {
        if (buildingType == BuildingData.BuildingType.Lumberjack || buildingType == BuildingData.BuildingType.TownHall || buildingType == BuildingData.BuildingType.Farm || buildingType == BuildingData.BuildingType.ResearchBuilding)
        {
            return true;
        }

        var requiredResearchUpgrade = GetResearchUpgradeForBuilding(buildingType);
        return upgrades != null && upgrades.IsUnlocked(requiredResearchUpgrade);
    }

    private IEnumerator BuildCoroutine(Building instantiatedBuilding, float buildingMultiplier)
    {
        float time = 0;
        float buildTime = instantiatedBuilding.buildingData.buildingTime;
        MeshRenderer meshRenderer = instantiatedBuilding.GetComponentInChildren<MeshRenderer>();
        deleteCooldown = buildTime;
        while (time <= buildTime)
        {
            time += Time.fixedDeltaTime;
            if (deleteCooldown > 0)
            {
                deleteCooldown -= Time.fixedDeltaTime;
            }
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                meshRenderer.materials[i].SetFloat("_DissolveTime", time * buildingMultiplier);
            }
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Building construction complete.");
    }



    private int CountSameTiles(Vector2Int gridPosition, TileType tileType)
    {
        int count = 0;
        Vector2Int[] directions = {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        foreach (var direction in directions)
        {
            Vector2Int neighborPosition = gridPosition + direction;
            if (GridManager.Instance._grid.IsInDimensions(neighborPosition) && GridManager.Instance._grid[neighborPosition].tileType == tileType)
            {
                count++;
            }
        }

        return count;
    }

    public void SetBuildingType(BuildingData.BuildingType buildingType)
    {
        this.buildingType = buildingType;
    }

    public BuildingData.BuildingType GetCurrentBuildingType()
    {
        return buildingType;
    }

    private bool TryBuild(Building building, List<Vector2Int> gridPositionList)
    {
        int localSameTileCount = 0;
        foreach (Vector2Int position in gridPositionList)
        {
            if (!GridManager.Instance._grid.IsInDimensions(position))
            {
                return false;
            }
            if (GridManager.Instance._grid[position].building != null && GridManager.Instance._grid[position].building.buildingType != BuildingData.BuildingType.Environment)
            {
                return false;
            }
            if (building.buildingData.placableTileTypes[1] == GridManager.Instance._grid[position].tileType)
            {
                return false;
            }

            if (building.buildingData.placableTileTypes[0] == GridManager.Instance._grid[position].tileType)
            {
                localSameTileCount++;
            }
        }

        if (buildingNumber == 3 && isTownHallPlaced)
        {
            return false;
        }

        if (localSameTileCount == 0)
        {
            return false;
        }

        isPlaced = true;
        return true;
    }

    private Upgrades.ResearchUpgrades GetResearchUpgradeForBuilding(BuildingData.BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingData.BuildingType.House:
                return Upgrades.ResearchUpgrades.House;
            case BuildingData.BuildingType.FishingHouse:
                return Upgrades.ResearchUpgrades.Fishing;
            case BuildingData.BuildingType.StoneMine:
                return Upgrades.ResearchUpgrades.StoneMine;
            case BuildingData.BuildingType.Wall:
                return Upgrades.ResearchUpgrades.Wall;
            default:
                return Upgrades.ResearchUpgrades.None;
        }
    }

    public void SetUpgrades(Upgrades upgrades)
    {
        this.upgrades = upgrades;
    }

    public void DeleteBuilding(Vector2Int gridPosition)
    {
        if (GridManager.Instance._grid[gridPosition].building == null)
            return;
        if (GridManager.Instance._grid[gridPosition].building.buildingType == BuildingData.BuildingType.TownHall)
            return;
        if (deleteCooldown > 0)
            return;

        Tile tile = new Tile()
        {
            building = null,
            tileType = GridManager.Instance._grid[gridPosition].tileType
        };

        if (GridManager.Instance._grid[gridPosition].building != null)
        {
            Destroy(GridManager.Instance._grid[gridPosition].building.gameObject);
            foreach (var item in GridManager.Instance._grid[gridPosition].building.buildingData.GetGridPositionList(gridPosition, GridManager.Instance._grid[gridPosition].building.direction))
            {
                GridManager.Instance._grid[item] = tile;
            }
        }      
    }
}
