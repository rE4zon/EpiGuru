using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGeneration : MonoBehaviour
{
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject roadPrefab; 
    [SerializeField] private float triggerDistance = 20f; 
    [SerializeField] private float roadSegmentLength = 10f; 
    [SerializeField] private float destroyDistance = 30f; 

    public delegate void SpawnNewRoadSegmentHandler(Vector3 roadPosition);
    public static event SpawnNewRoadSegmentHandler OnSpawnNewRoadSegment;

    private Transform playerTransform;
    private List<GameObject> roadSegments = new List<GameObject>();

    public List<GameObject> RoadSegments 
    {
        get { return roadSegments; }
    }

    public float RoadSegmentLength 
    {
        get { return roadSegmentLength; }
    }

    void Start()
    {
        playerTransform = player.transform;

        GameObject initialRoad = Instantiate(roadPrefab, transform.position, Quaternion.identity);
        roadSegments.Add(initialRoad);

        if (OnSpawnNewRoadSegment != null)
        {
            OnSpawnNewRoadSegment(initialRoad.transform.position);
        }

        transform.position += Vector3.forward * roadSegmentLength;
    }

    void Update()
    {
        if (roadSegments.Count > 0 && Vector3.Distance(playerTransform.position, roadSegments[roadSegments.Count - 1].transform.position) < triggerDistance)
        {
            SpawnNewRoadSegment();
        }

        if (roadSegments.Count > 1 && Vector3.Distance(playerTransform.position, roadSegments[0].transform.position) > destroyDistance)
        {
            Destroy(roadSegments[0]);
            roadSegments.RemoveAt(0);
        }
    }

    void SpawnNewRoadSegment()
    {
        GameObject newRoad = Instantiate(roadPrefab, roadSegments[roadSegments.Count - 1].transform.position + Vector3.forward * roadSegmentLength, Quaternion.identity);
        roadSegments.Add(newRoad);

        if (OnSpawnNewRoadSegment != null)
        {
            OnSpawnNewRoadSegment(newRoad.transform.position);
        }

        transform.position += Vector3.forward * roadSegmentLength;
    }
}
