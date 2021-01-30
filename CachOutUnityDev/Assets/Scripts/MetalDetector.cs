using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDetector : MonoBehaviour
{
    public DetectorRange CloseProxy;
    public DetectorRange MedProxy;
    public DetectorRange FarProxy;

    public int DetectionFrequency = 0;

    void Update()
    {
        if (CloseProxy.GetTreasureStatus())
            DetectionFrequency = 3;
        else if (MedProxy.GetTreasureStatus())
            DetectionFrequency = 2;
        else if (FarProxy.GetTreasureStatus())
            DetectionFrequency = 1;
        else
            DetectionFrequency = 0;

        Debug.Log("DetectionFrequency is " + DetectionFrequency);
    }
}
