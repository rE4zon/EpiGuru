using OneSignalSDK;
using UnityEngine;

public class OneSignalController : MonoBehaviour
{
    [SerializeField] private string projectId = "635c5f12-bb1c-4e8a-92a5-65636c604328";
    void Start()
    {
        OneSignal.Initialize(projectId);
        DontDestroyOnLoad(gameObject);
    }
}

