using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float obstacleSpawnProbability = 0.2f;
    [SerializeField] private float obstacleDestroyDistance = 30f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Transform obstaclesParent;
    [SerializeField] private LayerMask coinLayer;


    private GroundGeneration groundGeneration;

    void Start()
    {
        groundGeneration = FindObjectOfType<GroundGeneration>();
        if (groundGeneration == null)
        {
            Debug.LogError("GroundGeneration script not found in the scene.");
        }

        GroundGeneration.OnSpawnNewRoadSegment += SpawnObstaclesOnRoad;

        obstaclesParent = new GameObject("ObstaclesParent").transform;
    }

    void SpawnObstaclesOnRoad(Vector3 roadPosition)
    {
        SpawnObstacles(roadPosition);
        RemoveOldObstacles(roadPosition);
    }

    void SpawnObstacles(Vector3 roadPosition)
    {
        GameObject currentRoadSegment = GetCurrentRoadSegment(roadPosition);

        if (currentRoadSegment == null)
            return;

        float roadWidth = 10f;
        float obstacleWidth = 2f;
        float obstacleSpawnRange = (roadWidth - obstacleWidth) / 2;

        if (Random.value < obstacleSpawnProbability)
        {
            float randomOffset = Random.Range(-obstacleSpawnRange, obstacleSpawnRange);

            if (currentRoadSegment != null)
            {
                Vector3 obstaclePosition = currentRoadSegment.transform.position + Vector3.up * 0.5f + Vector3.right * randomOffset;

                Collider[] coinColliders = Physics.OverlapBox(obstaclePosition, new Vector3(obstacleWidth / 2, 0.5f, 0.5f), Quaternion.identity, coinLayer);

                if (coinColliders.Length == 0)
                {
                    GameObject obstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);

                    obstacle.transform.parent = obstaclesParent;

                    obstacle.tag = "Obstacle";
                }
            }
        }
    }

    void RemoveOldObstacles(Vector3 roadPosition)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle == null)
                continue;

            if (Vector3.Distance(roadPosition, obstacle.transform.position) > obstacleDestroyDistance)
            {
                Destroy(obstacle);
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