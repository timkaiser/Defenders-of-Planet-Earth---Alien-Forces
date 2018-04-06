using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopicControl : MonoBehaviour
{
    PlayerNetworkManager netMan;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    float rotX = 0.0f;
    float rotY = 0.0f;

    void Start()
    {
        netMan = transform.root.GetComponent<PlayerNetworkManager>();
        if (!netMan.isLocalPlayer)
        {
            //Destroy(this);
            return;
        }
        rotX = transform.eulerAngles.x;
        rotY = transform.eulerAngles.y;

#if UNITY_EDITOR || UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.eulerAngles = new Vector3(0, 180, 0);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.Log("Gyroscope not supported!");
            Destroy(this);
        }
        else
        {
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
            StartCoroutine(startGyroControl());
        }
#endif
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (netMan.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (!Input.GetKey(KeyCode.LeftControl))
                pcRotateCamera();
        }
#endif
    }

    void pcRotateCamera()
    {
        rotY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0.0f);
    }

    IEnumerator startGyroControl()
    {
        while (true)
        {
            Quaternion transQuat = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y,
                             -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0) * transQuat, 0.75f);
            yield return new WaitForEndOfFrame();
        }
    }

    void OnDestroy()
    {
        Input.gyro.enabled = false;
        Input.compass.enabled = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
    }
}