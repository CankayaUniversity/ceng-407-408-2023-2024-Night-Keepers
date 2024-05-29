using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface meshSurface;
    private float bakeDelay = 0.02f;

    private WaitForSeconds waitForSeconds;
    private Coroutine bakeCoroutine;

    private void Awake()
    {
        meshSurface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        //BakeNavMesh();

        waitForSeconds = new WaitForSeconds(bakeDelay);
    }

    private void OnEnable()
    {
        TestBuilding.onBuildingDestroyed += OnBuildingDestroyed;
        GridManager.onWorldGenerationDone += OnWorldGenerationDone;
        BuildingManager.OnBuildingPlaced += OnBuildingPlaced;
    }

    private void OnDisable()
    {
        TestBuilding.onBuildingDestroyed -= OnBuildingDestroyed;
        GridManager.onWorldGenerationDone -= OnWorldGenerationDone;
        BuildingManager.OnBuildingPlaced -= OnBuildingPlaced;
    }

    private void OnBuildingPlaced()
    {
        meshSurface.UpdateNavMesh(meshSurface.navMeshData);
    }

    private void OnBuildingDestroyed()
    {
        if (bakeCoroutine != null)
        {
            StopCoroutine(bakeCoroutine);
        }
        bakeCoroutine = StartCoroutine(BakeNavMeshWithDelay());
    }

    private void OnWorldGenerationDone()
    {
        BakeNavMesh();
    }

    private IEnumerator BakeNavMeshWithDelay()
    {
        yield return waitForSeconds;

        meshSurface.UpdateNavMesh(meshSurface.navMeshData);
    }

    private void BakeNavMesh()
    {
        meshSurface.BuildNavMesh();
    }
}