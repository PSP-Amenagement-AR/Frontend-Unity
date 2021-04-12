using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>Class for camera management between Runtime or Editor mode.</summary>
public class CameraHandler : MonoBehaviour
{
    /// <summary>The AR camera.</summary>
    public Camera ARCamera;
    /// <summary>The default camera.</summary>
    public Camera Camera;

    /// Function executed when the script is started.
    /// Check the availability of the AR mode.
    /// @returns Return an ARSession object in terms of the availability of the execution mode.
    /// @see ARSession()
    public IEnumerator Start()
    {
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            Debug.Log("No AR mode");
            this.ARCamera.gameObject.SetActive(false);
            this.Camera.gameObject.SetActive(true);
        }
        else
        {
            this.ARCamera.gameObject.SetActive(true);
            this.Camera.gameObject.SetActive(false);
        }
    }

    /// Get the camera in terms of the mode of execution of the project.
    /// @returns The standard camera or the AR camera.
    public Camera GetCamera()
    {
        if (ARSession.state == ARSessionState.Unsupported)
            return Camera;
        else
            return ARCamera;
    }
}
