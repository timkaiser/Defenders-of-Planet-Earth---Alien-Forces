using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GPSWalker : MonoBehaviour
{

    PlayerNetworkManager netMan;

    bool locationServiceStarted = false;
    float startLat;
    float currLat;
    float startLon;
    float currLon;
    double startTime;
    float lastUpdate;
    float latlenPerDeg = 111321;
    public bool enableLogDisplay = false;

    GameObject gyroController;
    public float pcMovementSpeed = 2;
    GameManager gm;

    void Start()
    {
        netMan = transform.root.GetComponent<PlayerNetworkManager>();
        if (!netMan.isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
#if UNITY_ANDROID
        //StartCoroutine(StartLoc());
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        gyroController = HelperFunctions.getChildGameObject(gameObject, "GyroController");
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
#endif
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        PCMovement();
#endif
    }

//    void FixedUpdate()
//    {
//#if UNITY_ANDROID
//        //if (locationServiceStarted)
//        //{
//        //    Vector2 offset = distance(startLat, startLon, currLat, currLon);
//        //    if (offset.magnitude < 200)
//        //        transform.position = Vector3.Slerp(transform.position, new Vector3(offset.x, 1, offset.y), Time.fixedDeltaTime);
//        //}
//#endif
//    }

    void PCMovement()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float multiplier = 1;
        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = 3;
        try{
            transform.Translate((gyroController.transform.right * movement.x + Quaternion.AngleAxis(gyroController.transform.eulerAngles.y, Vector3.up) * Vector3.forward * movement.y) * Time.deltaTime * pcMovementSpeed * multiplier);
        }
        catch(System.Exception e){}
    }

    //IEnumerator StartLoc()
    //{
    //    if (enableLogDisplay)
    //        LogDisplay.setText("Starting Location Service");
    //    // First, check if user has location service enabled
    //    if (!Input.location.isEnabledByUser)
    //    {
    //        if (enableLogDisplay)
    //            LogDisplay.setText("Location Service not enabled.");
    //        yield break;
    //    }

    //    // Start service before querying location
    //    Input.location.Start(5, 0.1f);

    //    // Wait until service initializes
    //    int maxWait = 20;
    //    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    //    {
    //        if (enableLogDisplay)
    //            LogDisplay.setText("Waiting for LocServices... " + maxWait);
    //        yield return new WaitForSeconds(1);
    //        maxWait--;
    //    }

    //    // Service didn't initialize in 20 seconds
    //    if (maxWait < 1)
    //    {
    //        if (enableLogDisplay)
    //            LogDisplay.setText("Timed out");
    //        yield break;
    //    }

    //    // Connection has failed
    //    if (Input.location.status == LocationServiceStatus.Failed)
    //    {
    //        if (enableLogDisplay)
    //            LogDisplay.setText("Unable to determine device location");
    //        yield break;
    //    }
    //    else
    //    {
    //        int counter = 0;
    //        while (Input.location.lastData.horizontalAccuracy > 10)
    //        {
    //            if (enableLogDisplay)
    //                LogDisplay.setText("Waiting for accurate location reading... " + counter + "\nCurrent Accuracy: " + Input.location.lastData.horizontalAccuracy);
    //            LogDisplay.addText("Waiting for accurate location reading..." + counter
    //                + "\nCurrent Accuracy: " + Input.location.lastData.horizontalAccuracy
    //                + "\nExpecting Accuracy < 10");
    //            yield return new WaitForEndOfFrame();
    //            counter++;
    //        }
    //        currLat = startLat = Input.location.lastData.latitude;
    //        currLon = startLon = Input.location.lastData.longitude;
    //        netMan.CmdSetLatLon(startLat, startLon);
    //        if (!netMan.isServer)
    //        {
    //            while (!gm.centerCoordinatesSet)
    //            {
    //                yield return new WaitForEndOfFrame();
    //            }
    //            float[] startLatLon = gm.getStartLatLon();
    //            startLat = startLatLon[0];
    //            startLon = startLatLon[1];
    //        }
    //        lastUpdate = Time.time;
    //        locationServiceStarted = true;
    //        startTime = Input.location.lastData.timestamp;

    //        // Access granted and location value could be retrieved
    //        if (enableLogDisplay)
    //            LogDisplay.setText("Loc: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude
    //            + " \nAcc:" + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.verticalAccuracy + " \nlast update: " + (Input.location.lastData.timestamp - startTime));
    //    }
    //    //StartCoroutine(updateLocation());
    //    // Stop service if there is no need to query location updates continuously
    //    // Input.location.Stop();
    //}

    //IEnumerator updateLocation()
    //{
    //    while (true)
    //    {
    //        if (Time.time - lastUpdate < 1)
    //        {
    //            yield return new WaitForFixedUpdate();
    //        }
    //        else
    //        {
    //            LogDisplay.setText("");
    //            //if(Input.location.lastData.horizontalAccuracy <= 4)
    //            //{
    //            //LogDisplay.addText("Accurate location reading!\n");
    //            currLat = Input.location.lastData.latitude;
    //            currLon = Input.location.lastData.longitude;
    //            //}
    //            lastUpdate = Time.time;
    //            Vector2 offset = distance(startLat, startLon, currLat, currLon);
    //            if (enableLogDisplay)
    //                LogDisplay.addText("Updating at " + Time.time + "\n"
    //                + "Loc: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude
    //                + "\nAccuracy:" + Input.location.lastData.horizontalAccuracy + " \nlastupdate: " + (Input.location.lastData.timestamp - startTime)
    //                + "\nOffset from start: " + offset
    //                + "\nPosition: " + transform.position);
    //            yield return new WaitForFixedUpdate();
    //        }
    //    }
    //}


    //Vector2 distance(float sLat, float sLon, float cLat, float cLon)
    //{
    //    float lonLenPerDeg = lonLenAtDeg((sLat + cLat) / 2);
    //    Vector2 dist = Vector2.zero;
    //    float x = (cLon - sLon) * lonLenPerDeg;
    //    float y = (cLat - sLat) * latlenPerDeg;
    //    dist = new Vector2(x, y);
    //    return dist;
    //}

    //float lonLenAtDeg(float lat)
    //{
    //    return Mathf.Cos(Mathf.Deg2Rad * lat) * 111321;
    //}

    //void OnDestroy()
    //{
    //    Input.location.Stop();
    //}
}
