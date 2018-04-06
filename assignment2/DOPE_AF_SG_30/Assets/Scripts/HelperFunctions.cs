using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour {
    public static float getAngleDiff(float compDir, float objDir)
    {
        while (compDir < 0)
            compDir += 360;
        while (compDir >= 360)
            compDir -= 360;
        while (objDir < 0)
            objDir += 360;
        while (objDir >= 360)
            objDir -= 360;

        float ret = (compDir - objDir);
        if (ret < -180)
            ret = (360 + ret);
        else if (ret > 180)
            ret = -(360 - ret);

        return ret;
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
