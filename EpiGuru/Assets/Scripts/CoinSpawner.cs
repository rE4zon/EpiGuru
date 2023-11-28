using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnProbability = 0.5f;
    [SerializeField] private float coinDestroyDistance = 30f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Transform coinsParent;

    private GroundGeneration groundGeneration;

    void Start()
    {
        groundGeneration = FindObjectOfType<GroundGeneration>();
        if (groundGeneration == null)
        {
            Debug.LogError("GroundGeneration script not found in the scene");
        }

        GroundGeneration.OnSpawnNewRoadSegment += SpawnCoinsOnRoad;

        coinsParent = new GameObject("CoinsParent").transform;
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
        float coinWidth = 1f;
        float coinSpawnRange = (roadWidth - coinWidth) / 2;

        if (Random.value < coinSpawnProbability)
        {
            float randomOffset = Random.Range(-coinSpawnRange, coinSpawnRange);

            if (currentRoadSegment != null)
            {
                Vector3 coinPosition = currentRoadSegment.transform.position + Vector3.up * 0.5f + Vector3.right * randomOffset;

                float minSpawnX = currentRoadSegment.transform.position.x - roadWidth / 2 + coinWidth / 2;
                float maxSpawnX = currentRoadSegment.transform.position.x + roadWidth / 2 - coinWidth / 2;

                coinPosition.x = Mathf.Clamp(coinPosition.x, minSpawnX, maxSpawnX);

                Collider[] obstacleColliders = Physics.OverlapBox(coinPosition, new Vector3(coinWidth / 2, 0.5f, 0.5f), Quaternion.identity, obstacleLayer);

                if (obstacleColliders.Length == 0)
                {
                    GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);

                    coin.transform.parent = coinsParent;

                    coin.tag = "Coin";
                }
            }
        }
    }

    //bool ObstacleExistsAtPosition(Vector3 position)
    //{
    //    Collider[] colliders = Physics.OverlapSphere(position, 0.5f);

    //    foreach (Collider collider in colliders)
    //    {
    //        if (collider.CompareTag("Obstacle"))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

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