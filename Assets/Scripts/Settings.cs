using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Class specifying the application settings.
public class Settings : MonoBehaviour
{
    /// Set the target frame rate.
    public void Start()
    {
        /* QualitySettings.vSyncCount = 2;*/
        Application.targetFrameRate = 120;
    }
}
