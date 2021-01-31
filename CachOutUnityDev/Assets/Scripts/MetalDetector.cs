using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDetector : MonoBehaviour
{
    public DetectorRange CloseProxy;
    public DetectorRange MedProxy;
    public DetectorRange FarProxy;

    public AudioSource Beeper;

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

        switch (DetectionFrequency)
        {
            case 0: Beeper.pitch = 0f;
            break;
            case 1: Beeper.pitch = 0.1f;
            break;
            case 2: Beeper.pitch = 0.5f;
            break;
            case 3: Beeper.pitch = 1f;
            break;
        }


        Debug.Log("DetectionFrequency is " + DetectionFrequency);
    }
}
