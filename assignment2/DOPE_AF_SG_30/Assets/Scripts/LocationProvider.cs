using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LocationProvider : MonoBehaviour
{
    public static float latitude = 48.262450f;
    public static float longitude = 11.669223f;
    public static float altitude = 0;
    public static float horizontalAccuracy = 0;
    
    IEnumerator loadLocation()
    {
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
                yield break;

            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                latitude = Input.location.lastData.latitude;
                longitude = Input.location.lastData.longitude;
                altitude = Input.location.lastData.altitude;
                horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
            }
        
        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
    void Start()
    {
        StartCoroutine(loadLocation());    
    }

    int count = 0;
    private void Update()
    {
        if (count >= 50)
        {
            StartCoroutine(loadLocation());
        }
    }

}