using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Light : MonoBehaviour
{

    public GameObject mLight;

    public void Start()
    {
        mLight.SetActive(false);
    }

    public void LightOn()
    {
        mLight.SetActive(true);
    }

    public void LigtOff()
    {
        mLight.SetActive(false);
    }

}
