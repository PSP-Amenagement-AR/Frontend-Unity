﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    void Start()
    {
        /* QualitySettings.vSyncCount = 2;*/
        Application.targetFrameRate = 120;
    }
}
