using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface meshSurface;
    private float bakeDelay = 0.01f;
    private WaitForSeconds waitForSeconds;

    private Coroutine bakeCoroutine;

    private void Start()
    {
        meshSurface = GetComponent<NavMeshSurface>();
        BakeNavMesh();

        waitForSeconds = new WaitForSeconds(bakeDelay);
    }

    private void OnEnable()
    {
        Unit.onBuildingDestroyed += OnBuildingDestroyed;
        GridManager.onWorldGenerationDone += OnWorldGenerationDone;
    }

    private void OnDisable()
    {
        Unit.onBuildingDestroyed -= OnBuildingDestroyed;
        GridManager.onWorldGenerationDone -= OnWorldGenerationDone;
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
        Debug.Log("bake done");
    }

    private IEnumerator BakeNavMeshWithDelay()
    {
        yield return waitForSeconds;
        BakeNavMesh();
    }

    private void BakeNavMesh()
    {
        meshSurface.BuildNavMesh();
    }
}