using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnProbability = 0.5f;
    [SerializeField] private float coinDestroyDistance = 30f;

    private GroundGeneration groundGeneration;

    void Start()
    {
        groundGeneration = FindObjectOfType<GroundGeneration>();
        if (groundGeneration == null)
        {
            Debug.LogError("GroundGeneration script not found in the scene");
        }

        GroundGeneration.OnSpawnNewRoadSegment += SpawnCoinsOnRoad;
    }

    void SpawnCoinsOnRoad(Vector3 roadPosition)
    {
        SpawnCoins(roadPosition);
        RemoveOldCoins(roadPosition);
    }

    void SpawnCoins(Vector3 roadPosition)
    {
        GameObject currentRoadSegment = GetCurrentRoadSegment(roadPosition);

        if (currentRoadSegment == null)
            return;

        float roadWidth = 10f;
        float leftBoundary = currentRoadSegment.transform.position.x - roadWidth / 2;
        float rightBoundary = currentRoadSegment.transform.position.x + roadWidth / 2;

        if (Random.value < coinSpawnProbability)
        {
            float randomOffset = Random.Range(leftBoundary, rightBoundary);

            if (currentRoadSegment != null)
            {
                Vector3 coinPosition = currentRoadSegment.transform.position + Vector3.up * 0.5f + Vector3.right * randomOffset;
                GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
                coin.tag = "Coin";
            }
        }
    }

    void RemoveOldCoins(Vector3 roadPosition)
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject coin in coins)
        {
            if (coin == null)
                continue;

            if (Vector3.Distance(roadPosition, coin.transform.position) > coinDestroyDistance)
            {
                Destroy(coin);
            }
        }
    }

    GameObject GetCurrentRoadSegment(Vector3 roadPosition)
    {
        foreach (GameObject roadSegment in groundGeneration.RoadSegments)
        {
            if (roadSegment == null)
                continue;

            float distanceToRoadSegment = Vector3.Distance(roadPosition, roadSegment.transform.position);
            if (distanceToRoadSegment < groundGeneration.RoadSegmentLength / 2)
            {
                return roadSegment;
            }
        }
        return null;
    }
}
