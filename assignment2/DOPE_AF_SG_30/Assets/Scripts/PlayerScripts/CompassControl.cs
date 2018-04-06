using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassControl : MonoBehaviour {

    public float compassSmoothTime = 0.5f;
    float compassSmooth;
    float compassVelocity = 0.0f;
    float lookOffsetAngleVelocity = 0.0f;
    float lookOffsetAngle = 0.0f;

    void Start () {
        Input.compass.enabled = true;
        StartCoroutine(startCompass());
    }

    IEnumerator startCompass()
    {
        compassSmooth = Input.compass.trueHeading;

        yield return null;
        while (true)
        {
            compassSmooth = Mathf.SmoothDampAngle(compassSmooth, Input.compass.magneticHeading, ref compassVelocity, compassSmoothTime);
            if (compassSmooth > 360)
                compassSmooth -= 360;
            else if (compassSmooth < 0)
                compassSmooth += 360;

            //float angleDiff = getAngleDiff(compassSmooth, transform.eulerAngles.y);
            ////if (Mathf.Abs(angleDiff) > 40)
            ////    lookOffsetAngle = angleDiff;
            ////else
            ////{
            ////    if(lookOffsetAngle > angleDiff)
            ////        lookOffsetAngle -= Mathf.Min(new float[]{ Mathf.Abs(angleDiff - lookOffsetAngle), Time.deltaTime * 2});
            ////    else if(lookOffsetAngle < angleDiff)
            ////        lookOffsetAngle += Mathf.Min(new float[]{ Mathf.Abs(angleDiff - lookOffsetAngle), Time.deltaTime * 2 });
            ////}
            //LogDisplay.setText("LookOffsetAngle: " + lookOffsetAngle 
            //    + "\nAngleDiff: " + angleDiff + "\n");
            //if (lookOffsetAngle > angleDiff)
            //{
            //    lookOffsetAngle -= Mathf.Min(new float[] { Mathf.Abs(angleDiff - lookOffsetAngle), Time.deltaTime * angleDiff / 4 });
            //    LogDisplay.addText("Decreasing LOA");
            //}
            //else if (lookOffsetAngle < angleDiff)
            //{
            //    lookOffsetAngle += Mathf.Min(new float[] { Mathf.Abs(angleDiff - lookOffsetAngle), Time.deltaTime * angleDiff / 4 });
            //    LogDisplay.addText("Increasing LOA");
            //}
            //lookOffsetAngle = angleDiff;

            lookOffsetAngle = Mathf.SmoothDamp(lookOffsetAngle, HelperFunctions.getAngleDiff(compassSmooth, transform.eulerAngles.y), ref lookOffsetAngleVelocity, 2 * compassSmoothTime);
            LogDisplay.setText("Compass: "+ Input.compass.trueHeading
               + "\nCompass Smooth: " + compassSmooth
               + "\nEuler Y: " + transform.eulerAngles.y
               + "\nDifference: " + HelperFunctions.getAngleDiff(compassSmooth, transform.eulerAngles.y)
               + "\nCompass raw: " + Input.compass.rawVector);
            yield return new WaitForFixedUpdate();
        }
    }
}
