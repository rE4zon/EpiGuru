using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class GeoCheckerLocation : MonoBehaviour
{
    [SerializeField] private GameObject ScreenOffPanel;

    private const string apiUrl = "https://ipinfo.io/json";
    private const string ukraineCountryCode = "UA";
    private const string wikipediaUrl = "https://uk.wikipedia.org/";

    void Start()
    {
        StartCoroutine(CheckGeoLocation());
    }

    IEnumerator CheckGeoLocation()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error while checking geo location: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            GeoInfo geoInfo = JsonUtility.FromJson<GeoInfo>(json);

            if (geoInfo != null && geoInfo.country == ukraineCountryCode)
            {
                Debug.Log("Player is in Ukraine. Show game screen.");
            }
            else
            {
                ScreenOffPanel.SetActive(true);
                Application.OpenURL(wikipediaUrl);
            }
        }
    }
}

[System.Serializable]
public class GeoInfo
{
    public string country;
}

