using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackground : MonoBehaviour {

    GUITexture BackgroundTexture;
    WebCamTexture CameraTexture;

    void Awake()
    {
        BackgroundTexture = gameObject.AddComponent<GUITexture>();
        int W = Screen.width;
        int H = Screen.height;
        BackgroundTexture.pixelInset = new Rect(0, 0, Screen.height / Screen.width, 1);

        WebCamDevice[] devices = WebCamTexture.devices;
        string backCamName = "";
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCamName = devices[i].name;
                Debug.Log("Rear-facing camera found.");
            }
            else
            {
                Debug.Log("Front-facing camera found.");
            }
        }

        CameraTexture = new WebCamTexture(backCamName, 1280, 720, 60);
        CameraTexture.Play();
        BackgroundTexture.texture = CameraTexture;
    }

    void Update()
    {
        if (!CameraTexture.isPlaying && Camera.main != null && Camera.main.clearFlags != CameraClearFlags.Skybox)
        {
            Debug.Log("Camera unavailabe, using Skybox.");
            Camera.main.clearFlags = CameraClearFlags.Skybox;
        }
    }

    void OnDestroy()
    {
        CameraTexture.Stop();
    }
}
